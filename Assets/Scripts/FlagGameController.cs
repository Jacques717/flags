using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FlagGameController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FlagData flagData;
    [SerializeField] private Timer timer;
    [SerializeField] private SpeechRecognitionHandler speechHandler;
    [SerializeField] private UIManager uiManager;
    
    [Header("Settings")]
    [SerializeField] private float delayBetweenQuestions = 1.5f;
    
    private List<FlagQuestion> availableFlags;
    private List<FlagQuestion> currentGameFlags;
    private FlagQuestion currentQuestion;
    private int currentQuestionIndex = 0;
    private int totalQuestions = 10;
    private bool waitingForAnswer = false;
    private bool questionAnswered = false;
    private string lastRecognizedText = null; // Store what was recognized
    private int currentQuestionId = 0; // Track question ID to ignore late recognitions
    
    // Track unrecognized speech attempts (for adding time)
    private int unrecognizedAttempts = 0;
    private const int MAX_UNRECOGNIZED_ATTEMPTS = 2;
    private bool speechWasRecognizedThisAttempt = false; // Track if we got any recognition
    private System.Collections.IEnumerator unrecognizedSpeechCheckCoroutine = null; // Track the coroutine
    private System.Collections.IEnumerator microphoneMonitorCoroutine = null; // Track microphone monitoring coroutine
    
    // Track microphone activity for feedback
    private float lastMicActivityTime = 0f;
    private bool micActivityDetected = false;
    private float micActivityStartTime = 0f;
    private const float MIC_ACTIVITY_TIMEOUT = 1.0f; // If mic is active but no recognition after 1 second, treat as unrecognized
    private const float MIC_ACTIVITY_THRESHOLD = 0.005f; // Very low threshold to detect any mic movement
    private const float MIC_SILENCE_DELAY = 0.3f; // Wait 0.3s after mic stops to see if recognition comes
    private const float MIC_INIT_DELAY = 0.2f; // Wait 0.2s after mic starts before checking for activity
    
    // Track answers for summary
    private List<QuestionResult> questionResults = new List<QuestionResult>();
    
    [System.Serializable]
    public class QuestionResult
    {
        public string countryName;
        public string recognizedText;
        public bool isCorrect;
        
        public QuestionResult(string country, string recognized, bool correct)
        {
            countryName = country;
            recognizedText = recognized;
            isCorrect = correct;
        }
    }
    
    private void Start()
    {
        InitializeComponents();
    }
    
    private void InitializeComponents()
    {
        if (flagData == null)
        {
            flagData = Resources.Load<FlagData>("FlagData");
            if (flagData == null)
            {
                Debug.LogError("FlagData not found in Resources! Creating default data.");
                CreateDefaultFlagData();
            }
        }
        
        if (timer == null)
        {
            timer = FindFirstObjectByType<Timer>();
        }
        
        if (speechHandler == null)
        {
            speechHandler = FindFirstObjectByType<SpeechRecognitionHandler>();
        }
        
        if (uiManager == null)
        {
            uiManager = FindFirstObjectByType<UIManager>();
        }
        
        // Subscribe to events
        if (timer != null)
        {
            timer.OnTimeExpired.AddListener(OnTimeExpired);
        }
        
        if (speechHandler != null)
        {
            speechHandler.OnSpeechRecognized.AddListener(OnSpeechRecognized);
            speechHandler.OnSpeechError.AddListener(OnSpeechError);
            speechHandler.OnSpeechStopped.AddListener(OnSpeechStopped);
        }
    }
    
    private void CreateDefaultFlagData()
    {
        flagData = ScriptableObject.CreateInstance<FlagData>();
        flagData.InitializeDefaultFlags();
    }
    
    public void StartGame(int questionCount)
    {
        totalQuestions = questionCount;
        currentQuestionIndex = 0;
        questionAnswered = false;
        questionResults.Clear(); // Reset results for new game
        
        // Ensure GamePanel is visible
        if (uiManager != null)
        {
            uiManager.ShowGameUI();
        }
        
        // Create a shuffled list of flags
        availableFlags = new List<FlagQuestion>(flagData.flags);
        currentGameFlags = new List<FlagQuestion>();
        
        // Randomly select flags for this game
        for (int i = 0; i < totalQuestions && availableFlags.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, availableFlags.Count);
            currentGameFlags.Add(availableFlags[randomIndex]);
            availableFlags.RemoveAt(randomIndex);
        }
        
        // Load flag sprites if not already loaded
        LoadFlagSprites();
        
        // Start first question
        StartNextQuestion();
    }
    
    private void LoadFlagSprites()
    {
        // Load flag sprites from Resources/Sprites/Flags/
        // Note: Resources.Load requires files to be in a Resources folder
        // Alternative: Load from Assets/Sprites/Flags/ using AssetDatabase (Editor only)
        // For runtime, sprites should be in Resources/Sprites/Flags/ or assigned in Inspector
        
        Debug.Log($"LoadFlagSprites: Loading sprites for {currentGameFlags.Count} flags");
        
        foreach (var flag in currentGameFlags)
        {
            if (flag.flagSprite == null)
            {
                // Try loading from Resources first
                string spriteName = flag.countryName.Replace(" ", "").ToLower();
                string resourcePath = $"Sprites/Flags/{spriteName}";
                Sprite sprite = Resources.Load<Sprite>(resourcePath);
                
                if (sprite != null)
                {
                    flag.flagSprite = sprite;
                    Debug.Log($"✓ Loaded sprite for {flag.countryName} from {resourcePath}");
                }
                else
                {
                    Debug.LogWarning($"✗ Flag sprite not found for {flag.countryName}. Expected at Resources/{resourcePath}.png");
                    Debug.LogWarning("Make sure the file exists and is imported as 'Sprite (2D and UI)' in Unity.");
                }
            }
            else
            {
                Debug.Log($"Flag sprite already assigned for {flag.countryName}");
            }
        }
    }
    
    private void StartNextQuestion()
    {
        if (currentQuestionIndex >= currentGameFlags.Count)
        {
            // Game complete
            return;
        }
        
        currentQuestion = currentGameFlags[currentQuestionIndex];
        questionAnswered = false;
        waitingForAnswer = true;
        currentQuestionId++; // Increment question ID to track late recognitions
        lastRecognizedText = null; // Reset recognized text for new question
        unrecognizedAttempts = 0; // Reset unrecognized attempts for new question
        speechWasRecognizedThisAttempt = false; // Reset recognition flag
        micActivityDetected = false; // Reset mic activity tracking
        lastMicActivityTime = 0f;
        micActivityStartTime = 0f;
        
        Debug.Log($"StartNextQuestion: Question {currentQuestionIndex + 1} (ID: {currentQuestionId}), Country: {currentQuestion.countryName}, Sprite: {(currentQuestion.flagSprite != null ? "LOADED" : "NULL")}");
        
        // Update UI
        if (uiManager != null)
        {
            uiManager.DisplayQuestion(currentQuestion, currentQuestionIndex + 1, totalQuestions);
        }
        else
        {
            Debug.LogError("StartNextQuestion: uiManager is null!");
        }
        
        // Start timer
        if (timer != null)
        {
            timer.ResetTimer();
            timer.StartTimer(10f);
        }
        
        // Start speech recognition
        if (speechHandler != null && speechHandler.IsInitialized)
        {
            speechWasRecognizedThisAttempt = false; // Reset before starting
            micActivityDetected = false;
            lastMicActivityTime = 0f;
            micActivityStartTime = 0f;
            
            // Stop any existing coroutines
            if (unrecognizedSpeechCheckCoroutine != null)
            {
                StopCoroutine(unrecognizedSpeechCheckCoroutine);
            }
            if (microphoneMonitorCoroutine != null)
            {
                StopCoroutine(microphoneMonitorCoroutine);
            }
            
            speechHandler.StartListening();
            
            // Start continuous microphone monitoring for feedback
            microphoneMonitorCoroutine = MonitorMicrophoneActivity();
            StartCoroutine(microphoneMonitorCoroutine);
        }
        else
        {
            Debug.LogWarning("Speech recognition not initialized. Using fallback.");
        }
    }
    
    private void OnSpeechRecognized(string recognizedText)
    {
        if (!waitingForAnswer || questionAnswered)
        {
            Debug.Log($"OnSpeechRecognized: Ignoring '{recognizedText}' - waitingForAnswer: {waitingForAnswer}, questionAnswered: {questionAnswered}");
            return;
        }
        
        // Store the question ID when recognition happens to detect late recognitions
        int recognitionQuestionId = currentQuestionId;
        
        Debug.Log($"=== SPEECH RECOGNIZED ===\nRecognized: '{recognizedText}'\nCurrent Question: {currentQuestion.countryName} (ID: {recognitionQuestionId})\nAccepted Answers: {string.Join(", ", currentQuestion.acceptedAnswers)}");
        
        // Stop the unrecognized speech check since we got recognition
        if (unrecognizedSpeechCheckCoroutine != null)
        {
            StopCoroutine(unrecognizedSpeechCheckCoroutine);
            unrecognizedSpeechCheckCoroutine = null;
        }
        
        // First check: Is this a valid country name at all?
        bool isValidCountry = IsValidCountryName(recognizedText);
        
        if (!isValidCountry)
        {
            // Not a country name at all - treat as unrecognized (game misheard)
            Debug.Log($"Recognized text '{recognizedText}' is not a valid country name - treating as unrecognized");
            HandleUnrecognizedSpeech();
            return;
        }
        
        // Mark that we got recognition - this will prevent mic activity timeout from triggering
        speechWasRecognizedThisAttempt = true;
        micActivityDetected = false; // Reset mic activity since we got recognition
        micActivityStartTime = 0f; // Reset start time
        
        // Store what was recognized
        lastRecognizedText = recognizedText;
        
        // Check if this recognition is for the current question (not a late recognition from previous question)
        if (recognitionQuestionId != currentQuestionId)
        {
            Debug.LogWarning($"OnSpeechRecognized: Ignoring late recognition '{recognizedText}' for question ID {recognitionQuestionId} (current is {currentQuestionId})");
            return;
        }
        
        // Check if answer is correct
        bool isCorrect = AnswerMatcher.IsAnswerCorrect(recognizedText, currentQuestion);
        Debug.Log($"=== ANSWER CHECK ===\nRecognized: '{recognizedText}'\nQuestion: '{currentQuestion.countryName}'\nResult: {(isCorrect ? "CORRECT" : "WRONG")}");
        
        if (isCorrect)
        {
            // Correct answer - proceed normally
            HandleAnswer(true, recognizedText);
        }
        else
        {
            // Wrong answer - show IMMEDIATE feedback on the side but keep listening
            Debug.Log($"WRONG ANSWER: Showing '{recognizedText}' as wrong answer for {currentQuestion.countryName}");
            
            // Show feedback immediately (don't wait for anything)
            if (uiManager != null)
            {
                uiManager.ShowWrongAnswerFeedback(recognizedText, currentQuestion.countryName);
            }
            
            // Mark that we got recognition (so mic monitor doesn't trigger "didn't get that")
            speechWasRecognizedThisAttempt = true;
            
            // Reset mic activity tracking for next attempt
            micActivityDetected = false;
            lastMicActivityTime = 0f;
            micActivityStartTime = 0f;
            
            // Reset recognition flag after a brief moment so we can detect new attempts
            StartCoroutine(ResetRecognitionFlagAfterDelay(0.2f));
            
            // Windows KeywordRecognizer continues listening automatically, no need to restart
            // Don't call HandleAnswer yet - let them try again or wait for timeout
        }
    }
    
    private System.Collections.IEnumerator ResetRecognitionFlagAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        speechWasRecognizedThisAttempt = false; // Allow detection of new attempts
    }
    
    private bool IsValidCountryName(string input)
    {
        if (string.IsNullOrEmpty(input) || flagData == null || flagData.flags == null)
            return false;
        
        string normalizedInput = input.ToLower().Trim();
        
        // Check against all available flags to see if this matches any country
        foreach (var flag in flagData.flags)
        {
            // Check country name
            if (flag.countryName != null && flag.countryName.ToLower().Trim() == normalizedInput)
                return true;
            
            // Check accepted answers
            if (flag.acceptedAnswers != null)
            {
                foreach (string acceptedAnswer in flag.acceptedAnswers)
                {
                    if (acceptedAnswer != null && acceptedAnswer.ToLower().Trim() == normalizedInput)
                        return true;
                }
            }
        }
        
        return false;
    }
    
    private void OnSpeechStopped()
    {
        if (!waitingForAnswer || questionAnswered)
            return;
        
        // Check if speech stopped without recognition (unrecognized speech)
        // Wait a brief moment to see if recognition comes through
        StartCoroutine(CheckForUnrecognizedSpeech());
    }
    
    private System.Collections.IEnumerator CheckForUnrecognizedSpeech()
    {
        // Wait a moment to see if recognition comes through (recognition might come slightly after stop)
        yield return new WaitForSeconds(0.3f);
        
        // If we're still waiting and didn't get recognition, it's unrecognized
        if (waitingForAnswer && !questionAnswered && !speechWasRecognizedThisAttempt)
        {
            Debug.Log("Speech stopped without recognition - treating as unrecognized");
            HandleUnrecognizedSpeech();
        }
        
        // Reset flag for next attempt (will be set to true if recognition comes)
        speechWasRecognizedThisAttempt = false;
    }
    
    private System.Collections.IEnumerator MonitorMicrophoneActivity()
    {
        Debug.Log("=== Microphone monitoring started ===");
        
        // Wait a moment for microphone to initialize
        yield return new WaitForSeconds(MIC_INIT_DELAY);
        
        int monitorQuestionId = currentQuestionId; // Store question ID for this monitoring session
        
        while (waitingForAnswer && !questionAnswered && monitorQuestionId == currentQuestionId)
        {
            yield return new WaitForSeconds(0.03f); // Check very frequently (every 0.03 seconds = ~33 times per second)
            
            if (uiManager != null)
            {
                float micVolume = uiManager.GetMicrophoneVolume();
                
                // Debug: Log mic volume periodically (every 1 second) and when activity is detected
                if (Time.frameCount % 60 == 0 || micVolume > MIC_ACTIVITY_THRESHOLD) // Every ~1 second OR when activity detected
                {
                    Debug.Log($"Mic volume: {micVolume:F3}, Activity detected: {micActivityDetected}, Recognition: {speechWasRecognizedThisAttempt}, Question ID: {monitorQuestionId}");
                }
                
                // Detect any microphone activity (mic indicator moving)
                if (micVolume > MIC_ACTIVITY_THRESHOLD)
                {
                    if (!micActivityDetected)
                    {
                        // Mic activity just started - mic indicator is moving
                        micActivityDetected = true;
                        micActivityStartTime = Time.time;
                        lastMicActivityTime = Time.time;
                        Debug.Log($"Microphone indicator moving detected (volume: {micVolume:F3}) - waiting for recognition...");
                    }
                    else
                    {
                        // Mic is still active - update time
                        lastMicActivityTime = Time.time;
                    }
                    
                    // Check for timeout: if mic was active for too long without recognition
                    float activeDuration = Time.time - micActivityStartTime;
                    if (!speechWasRecognizedThisAttempt && activeDuration > MIC_ACTIVITY_TIMEOUT && monitorQuestionId == currentQuestionId)
                    {
                        Debug.Log($"Microphone was active for {activeDuration:F2}s but no country recognized - asking user to say again (Question ID: {monitorQuestionId})");
                        HandleUnrecognizedSpeech();
                        // Wait a bit before checking again to avoid rapid triggers
                        yield return new WaitForSeconds(0.5f);
                        micActivityDetected = false;
                        micActivityStartTime = 0f;
                    }
                }
                else
                {
                    // Mic activity stopped - check if we got recognition
                    if (micActivityDetected && !speechWasRecognizedThisAttempt && monitorQuestionId == currentQuestionId)
                    {
                        // Mic was active (indicator moved) but no recognition happened
                        float timeSinceActivity = Time.time - lastMicActivityTime;
                        if (timeSinceActivity > MIC_SILENCE_DELAY) // Wait for recognition to come through
                        {
                            float activeDuration = Time.time - micActivityStartTime;
                            Debug.Log($"Microphone indicator moved (was active for {activeDuration:F2}s, silent for {timeSinceActivity:F2}s) but no country recognized - asking user to say again (Question ID: {monitorQuestionId})");
                            HandleUnrecognizedSpeech();
                            // Wait a bit before checking again
                            yield return new WaitForSeconds(0.5f);
                            micActivityDetected = false;
                            micActivityStartTime = 0f;
                        }
                    }
                }
            }
        }
    }
    
    private void OnSpeechError(string error)
    {
        Debug.LogWarning($"Speech recognition error: {error}");
        // Treat as unrecognized speech
        HandleUnrecognizedSpeech();
    }
    
    private void HandleUnrecognizedSpeech()
    {
        if (!waitingForAnswer || questionAnswered)
            return;
        
        // CRITICAL: Only handle as unrecognized if we truly didn't get any recognition
        // If we got recognition (even if wrong), we should have already shown it
        if (lastRecognizedText != null)
        {
            Debug.LogWarning($"HandleUnrecognizedSpeech called but lastRecognizedText is '{lastRecognizedText}' - this should not happen! Ignoring.");
            return;
        }
        
        // Check if we can still add time (max 2 attempts)
        if (unrecognizedAttempts < MAX_UNRECOGNIZED_ATTEMPTS)
        {
            unrecognizedAttempts++;
            Debug.Log($"Unrecognized speech attempt {unrecognizedAttempts}/{MAX_UNRECOGNIZED_ATTEMPTS}");
            
            // Show prompt
            if (uiManager != null)
            {
                uiManager.ShowUnrecognizedSpeechPrompt();
            }
            
            // Add 2 seconds to timer
            if (timer != null)
            {
                timer.AddTime(2f);
                Debug.Log($"Added 2 seconds to timer. Unrecognized attempts: {unrecognizedAttempts}");
            }
            
            // Reset tracking for next attempt
            speechWasRecognizedThisAttempt = false;
            micActivityDetected = false; // Reset mic activity tracking
            lastMicActivityTime = 0f;
            micActivityStartTime = 0f;
            
            // Windows KeywordRecognizer continues listening automatically, no need to restart
            Debug.Log("Reset tracking after unrecognized speech - user can try again");
        }
        else
        {
            Debug.Log("Max unrecognized attempts reached. Not adding more time.");
            // Still show the prompt but don't add time
            if (uiManager != null)
            {
                uiManager.ShowUnrecognizedSpeechPrompt();
            }
            
            // Reset tracking - Windows KeywordRecognizer continues listening automatically
            speechWasRecognizedThisAttempt = false;
            micActivityDetected = false;
            lastMicActivityTime = 0f;
            micActivityStartTime = 0f;
        }
    }
    
    private void OnTimeExpired()
    {
        if (!waitingForAnswer || questionAnswered)
            return;
        
        Debug.Log("Time expired!");
        // Use the last recognized text if available (even if wrong), otherwise null
        string finalAnswer = lastRecognizedText; // Could be wrong answer or null
        lastRecognizedText = null;
        HandleAnswer(false, finalAnswer);
    }
    
    private void HandleAnswer(bool isCorrect, string recognizedText)
    {
        if (questionAnswered)
            return;
        
        questionAnswered = true;
        waitingForAnswer = false;
        
        // Stop microphone monitoring since question is answered
        if (microphoneMonitorCoroutine != null)
        {
            StopCoroutine(microphoneMonitorCoroutine);
            microphoneMonitorCoroutine = null;
        }
        
        Debug.Log($"HandleAnswer called with isCorrect={isCorrect} for country: {currentQuestion.countryName}, recognized: {recognizedText ?? "none"}");
        
        // Track this answer for summary
        questionResults.Add(new QuestionResult(currentQuestion.countryName, recognizedText ?? "No answer", isCorrect));
        
        // Stop timer and speech recognition
        if (timer != null)
        {
            timer.StopTimer();
        }
        
        if (speechHandler != null)
        {
            speechHandler.StopListening();
        }
        
        // Update score
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnQuestionAnswered(isCorrect);
        }
        
        // Show feedback with recognized text
        if (uiManager != null)
        {
            uiManager.ShowAnswerFeedback(isCorrect, currentQuestion.countryName, recognizedText);
            Debug.Log($"ShowAnswerFeedback called with isCorrect={isCorrect}, recognized: {recognizedText ?? "none"}");
        }
        
        // Move to next question after delay
        currentQuestionIndex++;
        StartCoroutine(DelayedNextQuestion());
    }
    
    private IEnumerator DelayedNextQuestion()
    {
        yield return new WaitForSeconds(delayBetweenQuestions);
        
        if (currentQuestionIndex < totalQuestions)
        {
            StartNextQuestion();
        }
        else
        {
            // All questions answered - show score screen with summary
            Debug.Log($"=== GAME COMPLETE ===\nScore: {GameManager.Instance.CurrentScore}/{totalQuestions}\nResults count: {questionResults.Count}");
            
            // Log all results for debugging
            foreach (var result in questionResults)
            {
                Debug.Log($"Result: {result.countryName} - {(result.isCorrect ? "CORRECT" : "WRONG")} (said: '{result.recognizedText}')");
            }
            
            if (uiManager != null)
            {
                uiManager.ShowScoreScreen(GameManager.Instance.CurrentScore, totalQuestions, questionResults);
                Debug.Log($"ShowScoreScreen called with score: {GameManager.Instance.CurrentScore}/{totalQuestions}, results: {questionResults.Count}");
            }
            else
            {
                Debug.LogError("UIManager is null! Cannot show score screen.");
            }
        }
    }
}

