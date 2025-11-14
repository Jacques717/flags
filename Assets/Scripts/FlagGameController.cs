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
        lastRecognizedText = null; // Reset recognized text for new question
        
        Debug.Log($"StartNextQuestion: Question {currentQuestionIndex + 1}, Country: {currentQuestion.countryName}, Sprite: {(currentQuestion.flagSprite != null ? "LOADED" : "NULL")}");
        
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
            speechHandler.StartListening();
        }
        else
        {
            Debug.LogWarning("Speech recognition not initialized. Using fallback.");
        }
    }
    
    private void OnSpeechRecognized(string recognizedText)
    {
        if (!waitingForAnswer || questionAnswered)
            return;
        
        Debug.Log($"=== SPEECH RECOGNIZED ===\nRecognized: '{recognizedText}'\nCurrent Question: {currentQuestion.countryName}\nAccepted Answers: {string.Join(", ", currentQuestion.acceptedAnswers)}");
        
        // Store what was recognized
        lastRecognizedText = recognizedText;
        
        // Check if answer is correct
        bool isCorrect = AnswerMatcher.IsAnswerCorrect(recognizedText, currentQuestion);
        Debug.Log($"=== ANSWER CHECK ===\nRecognized: '{recognizedText}'\nQuestion: '{currentQuestion.countryName}'\nResult: {(isCorrect ? "CORRECT" : "WRONG")}");
        
        HandleAnswer(isCorrect, recognizedText);
    }
    
    private void OnSpeechError(string error)
    {
        Debug.LogWarning($"Speech recognition error: {error}");
        // Continue listening or handle error
    }
    
    private void OnTimeExpired()
    {
        if (!waitingForAnswer || questionAnswered)
            return;
        
        Debug.Log("Time expired!");
        lastRecognizedText = null; // No recognition on timeout
        HandleAnswer(false, null);
    }
    
    private void HandleAnswer(bool isCorrect, string recognizedText)
    {
        if (questionAnswered)
            return;
        
        questionAnswered = true;
        waitingForAnswer = false;
        
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

