using UnityEngine;
using UnityEngine.UI;
using System.Linq;
// using TMPro; // Commented out - will use regular Text instead for now

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    
    [Header("Menu UI")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private Button flagsButton;
    [SerializeField] private Button singlePlayerButton;
    [SerializeField] private Button multiplayerButton;
    
    [Header("Game UI")]
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private Image flagImage;
    [SerializeField] private Text questionNumberText;
    [SerializeField] private Text timerText;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text listeningText;
    [SerializeField] private Image microphoneVolumeIndicator; // Visual indicator for mic input
    
    [Header("Feedback UI")]
    [SerializeField] private GameObject feedbackPanel;
    [SerializeField] private Text feedbackText;
    [SerializeField] private Color correctColor = Color.green;
    [SerializeField] private Color incorrectColor = Color.red;
    
    [Header("Score Screen UI")]
    [SerializeField] private GameObject scoreScreenPanel;
    [SerializeField] private Text finalScoreText;
    [SerializeField] private Button returnToMenuButton;
    [SerializeField] private Button playAgainButton;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void FindUIElementsIfMissing()
    {
        UnityEngine.SceneManagement.Scene currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        
        // Only search for MainMenu UI elements if we're in MainMenu scene
        if (currentScene.name == "MainMenu")
        {
            if (menuPanel == null)
                menuPanel = FindGameObjectInScene("MenuPanel");
            
            if (flagsButton == null)
            {
                GameObject found = FindGameObjectInScene("FlagsButton");
                if (found != null)
                    flagsButton = found.GetComponent<Button>();
            }
            
            if (singlePlayerButton == null)
            {
                GameObject found = FindGameObjectInScene("SinglePlayerButton");
                if (found != null)
                    singlePlayerButton = found.GetComponent<Button>();
            }
            
            if (multiplayerButton == null)
            {
                GameObject found = FindGameObjectInScene("MultiplayerButton");
                if (found != null)
                    multiplayerButton = found.GetComponent<Button>();
            }
        }
        
        // Search for GameScene UI elements if we're in GameScene
        if (currentScene.name == "GameScene")
        {
            if (gamePanel == null)
                gamePanel = FindGameObjectInScene("GamePanel");
            
            if (flagImage == null)
            {
                GameObject found = FindGameObjectInScene("FlagImage");
                if (found != null)
                    flagImage = found.GetComponent<Image>();
            }
            
            if (microphoneVolumeIndicator == null)
            {
                GameObject found = FindGameObjectInScene("MicrophoneVolumeIndicator");
                if (found != null)
                    microphoneVolumeIndicator = found.GetComponent<Image>();
            }
            
            if (feedbackPanel == null)
            {
                feedbackPanel = FindGameObjectInScene("FeedbackPanel");
            }
            
            if (feedbackText == null && feedbackPanel != null)
            {
                GameObject found = FindGameObjectInScene("FeedbackText");
                if (found != null)
                    feedbackText = found.GetComponent<Text>();
            }
            
            if (scoreScreenPanel == null)
            {
                scoreScreenPanel = FindGameObjectInScene("ScoreScreenPanel");
            }
            
            if (finalScoreText == null)
            {
                GameObject found = FindGameObjectInScene("FinalScoreText");
                if (found != null)
                    finalScoreText = found.GetComponent<Text>();
            }
        }
    }
    
    // Helper method to find GameObjects recursively (searches all objects in scene)
    private GameObject FindGameObjectInScene(string name)
    {
        // First try direct find (for root objects)
        GameObject found = GameObject.Find(name);
        if (found != null)
            return found;
        
        // Search all objects in scene recursively (including inactive)
        UnityEngine.SceneManagement.Scene activeScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if (activeScene.IsValid())
        {
            GameObject[] rootObjects = activeScene.GetRootGameObjects();
            foreach (GameObject root in rootObjects)
            {
                found = FindInChildren(root.transform, name, true);
                if (found != null)
                    return found;
            }
        }
        
        return null;
    }
    
    // Recursive search in children (including inactive objects)
    private GameObject FindInChildren(Transform parent, string name, bool includeInactive = false)
    {
        if (parent.name == name)
        {
            if (includeInactive || parent.gameObject.activeInHierarchy)
                return parent.gameObject;
        }
        
        foreach (Transform child in parent)
        {
            GameObject found = FindInChildren(child, name, includeInactive);
            if (found != null)
                return found;
        }
        
        return null;
    }
    
    private void Start()
    {
        // Wait one frame to ensure all scene objects are loaded
        StartCoroutine(InitializeUIAfterFrame());
    }
    
    private System.Collections.IEnumerator InitializeUIAfterFrame()
    {
        // Wait one frame for scene to fully load
        yield return null;
        InitializeUI();
    }
    
    private void InitializeUI()
    {
        // Try to find UI elements if references are null (fallback)
        FindUIElementsIfMissing();
        
        // Hide game UI initially
        if (gamePanel != null)
            gamePanel.SetActive(false);
        
        if (feedbackPanel != null)
            feedbackPanel.SetActive(false);
        
        if (scoreScreenPanel != null)
            scoreScreenPanel.SetActive(false);
        
        // Setup button listeners
        if (flagsButton != null)
            flagsButton.onClick.AddListener(() => OnCategorySelected(GameCategory.Flags));
        
        if (singlePlayerButton != null)
            singlePlayerButton.onClick.AddListener(() => OnGameModeSelected(GameMode.SinglePlayer));
        
        if (multiplayerButton != null)
            multiplayerButton.onClick.AddListener(() => OnGameModeSelected(GameMode.Multiplayer));
        
        if (returnToMenuButton != null)
            returnToMenuButton.onClick.AddListener(OnReturnToMenu);
        
        if (playAgainButton != null)
            playAgainButton.onClick.AddListener(OnPlayAgain);
    }
    
    public void OnCategorySelected(GameCategory category)
    {
        // Show game mode selection
        if (singlePlayerButton != null)
            singlePlayerButton.gameObject.SetActive(true);
        
        if (multiplayerButton != null)
            multiplayerButton.gameObject.SetActive(true);
    }
    
    public void OnGameModeSelected(GameMode mode)
    {
        if (GameManager.Instance != null)
        {
            GameCategory category = GameCategory.Flags; // For now, only flags
            GameManager.Instance.StartGame(category, mode);
            
            // Switch to game scene
            ShowGameUI();
        }
    }
    
    public void ShowGameUI()
    {
        if (menuPanel != null)
            menuPanel.SetActive(false);
        
        if (gamePanel != null)
            gamePanel.SetActive(true);
        
        if (scoreScreenPanel != null)
            scoreScreenPanel.SetActive(false);
    }
    
    public void ShowMenuUI()
    {
        if (menuPanel != null)
            menuPanel.SetActive(true);
        
        if (gamePanel != null)
            gamePanel.SetActive(false);
        
        if (scoreScreenPanel != null)
            scoreScreenPanel.SetActive(false);
    }
    
    public void DisplayQuestion(FlagQuestion question, int questionNumber, int totalQuestions)
    {
        // Ensure GamePanel is active
        if (gamePanel != null && !gamePanel.activeSelf)
        {
            gamePanel.SetActive(true);
            Debug.Log("UIManager: Activated GamePanel");
        }
        
        if (flagImage == null)
        {
            Debug.LogError("UIManager: flagImage is null! Cannot display flag.");
            return;
        }
        
        // Ensure FlagImage GameObject is active
        if (flagImage.gameObject != null && !flagImage.gameObject.activeSelf)
        {
            flagImage.gameObject.SetActive(true);
            Debug.Log("UIManager: Activated FlagImage GameObject");
        }
        
        if (question.flagSprite == null)
        {
            Debug.LogWarning($"UIManager: Flag sprite is null for {question.countryName}. Sprite may not have loaded from Resources.");
        }
        else
        {
            flagImage.sprite = question.flagSprite;
            flagImage.preserveAspect = true; // Keep flag proportions correct
            flagImage.enabled = true; // Ensure Image component is enabled
            flagImage.color = Color.white; // Ensure color is fully opaque
            Debug.Log($"UIManager: Displaying flag for {question.countryName}. Sprite assigned: {flagImage.sprite != null}, Image enabled: {flagImage.enabled}, GameObject active: {flagImage.gameObject.activeSelf}, GamePanel active: {(gamePanel != null ? gamePanel.activeSelf.ToString() : "null")}");
        }
        
        if (questionNumberText != null)
        {
            questionNumberText.text = $"Question {questionNumber}/{totalQuestions}";
        }
        
        if (timerText != null)
        {
            timerText.text = "10";
        }
        
        if (listeningText != null)
        {
            listeningText.text = "🎤 Listening... Say a country name";
            listeningText.gameObject.SetActive(true);
            // Ensure text fits - make it wider if needed
            RectTransform textRect = listeningText.GetComponent<RectTransform>();
            if (textRect != null && textRect.sizeDelta.x < 400)
            {
                textRect.sizeDelta = new Vector2(400, textRect.sizeDelta.y);
            }
        }
        
        // Show microphone volume indicator
        if (microphoneVolumeIndicator != null)
        {
            microphoneVolumeIndicator.gameObject.SetActive(true);
            isMicrophoneActive = true;
        }
        
        // Hide feedback
        if (feedbackPanel != null)
            feedbackPanel.SetActive(false);
    }
    
    public void UpdateTimer(float timeRemaining)
    {
        if (timerText != null)
        {
            timerText.text = Mathf.Ceil(timeRemaining).ToString();
        }
    }
    
    public void UpdateScore(int score, int total)
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {score}/{total}";
        }
    }
    
    public void ShowAnswerFeedback(bool isCorrect, string correctAnswer, string recognizedText = null)
    {
        Debug.Log($"ShowAnswerFeedback: isCorrect={isCorrect}, correctAnswer={correctAnswer}, recognizedText={recognizedText ?? "null"}");
        
        // ALWAYS show what was recognized - make it very visible
        string displayText = "";
        
        if (isCorrect)
        {
            // Always show what was recognized, even when correct
            if (!string.IsNullOrEmpty(recognizedText))
            {
                displayText = $"✓ CORRECT!\nYou said: \"{recognizedText}\"\nAnswer: {correctAnswer}";
            }
            else
            {
                displayText = $"✓ CORRECT!\nAnswer: {correctAnswer}";
            }
        }
        else
        {
            // Show what was recognized (if available) with red X
            if (!string.IsNullOrEmpty(recognizedText))
            {
                displayText = $"✗ WRONG!\nYou said: \"{recognizedText}\"\nCorrect answer: {correctAnswer}";
            }
            else
            {
                // Time expired or no recognition
                displayText = $"✗ TIME EXPIRED!\nCorrect answer: {correctAnswer}";
            }
        }
        
        // Show in UI
        if (feedbackPanel != null)
        {
            // CRITICAL: Ensure parent is active first!
            Transform parent = feedbackPanel.transform.parent;
            if (parent != null && !parent.gameObject.activeSelf)
            {
                Debug.LogWarning($"=== PARENT WAS INACTIVE! ===\nParent: {parent.name}, activating now...");
                parent.gameObject.SetActive(true);
            }
            
            feedbackPanel.SetActive(true);
            Debug.Log($"=== SHOWING FEEDBACK ===\nPanel active: {feedbackPanel.activeSelf}\nParent active: {(parent != null ? parent.gameObject.activeSelf.ToString() : "null")}\nDisplay text: {displayText}");
            
            // Make sure panel is visible and on top
            feedbackPanel.transform.SetAsLastSibling();
            
            // Ensure panel is visible (check Canvas)
            Canvas canvas = feedbackPanel.GetComponentInParent<Canvas>();
            if (canvas != null)
            {
                Debug.Log($"Canvas found: {canvas.name}, enabled: {canvas.enabled}");
                if (!canvas.enabled)
                {
                    canvas.enabled = true;
                    Debug.LogWarning("Canvas was disabled! Enabled it.");
                }
            }
            
            // Find ALL text components in the feedback panel and update them
            Text[] allTextsInPanel = feedbackPanel.GetComponentsInChildren<Text>(true);
            Debug.Log($"=== FOUND {allTextsInPanel.Length} TEXT COMPONENTS IN FEEDBACK PANEL ===");
            
            foreach (Text text in allTextsInPanel)
            {
                Debug.Log($"Updating Text component: {text.gameObject.name}, current text: '{text.text}'");
                text.text = displayText;
                text.color = isCorrect ? correctColor : incorrectColor;
                text.fontSize = 24;
                text.gameObject.SetActive(true);
                
                // Ensure parent is active
                if (text.transform.parent != null && !text.transform.parent.gameObject.activeSelf)
                {
                    text.transform.parent.gameObject.SetActive(true);
                }
            }
            
            // Also update the main feedbackText reference if it exists
            if (feedbackText != null)
            {
                feedbackText.text = displayText;
                feedbackText.color = isCorrect ? correctColor : incorrectColor;
                feedbackText.fontSize = 24;
                feedbackText.gameObject.SetActive(true);
                
                Debug.Log($"=== MAIN FEEDBACK TEXT UPDATED ===\nText: '{feedbackText.text}'\nText visible: {feedbackText.gameObject.activeSelf}\nParent active: {feedbackText.transform.parent.gameObject.activeSelf}\nColor: {feedbackText.color}");
            }
            else
            {
                // If main reference is null, try to find and assign it
                if (allTextsInPanel.Length > 0)
                {
                    feedbackText = allTextsInPanel[0];
                    Debug.Log($"Assigned first text component to feedbackText: {feedbackText.gameObject.name}");
                }
                else
                {
                    Debug.LogError("=== ERROR: No text components found in feedback panel! ===");
                }
            }
        }
        else
        {
            Debug.LogError("=== ERROR: feedbackPanel is null! Cannot show feedback. ===");
        }
        
        if (listeningText != null)
        {
            listeningText.gameObject.SetActive(false);
        }
        
        // Hide microphone volume indicator
        if (microphoneVolumeIndicator != null)
        {
            microphoneVolumeIndicator.gameObject.SetActive(false);
        }
        
        // Stop microphone when not listening
        if (microphoneClip != null && !string.IsNullOrEmpty(microphoneDevice))
        {
            Microphone.End(microphoneDevice);
            microphoneClip = null;
            isMicrophoneActive = false;
        }
    }
    
    public void ShowScoreScreen(int score, int total, System.Collections.Generic.List<FlagGameController.QuestionResult> results = null, int player2Score = -1, System.Collections.Generic.List<FlagGameController.QuestionResult> player2Results = null)
    {
        Debug.Log($"=== ShowScoreScreen called ===\nScore: {score}/{total}\nResults: {(results != null ? results.Count.ToString() : "null")}");
        
        if (gamePanel != null)
            gamePanel.SetActive(false);
        
        if (scoreScreenPanel == null)
        {
            Debug.LogError("=== ERROR: scoreScreenPanel is null! ===");
            // Try to find it
            scoreScreenPanel = FindGameObjectInScene("ScoreScreenPanel");
            if (scoreScreenPanel == null)
            {
                Debug.LogError("ScoreScreenPanel not found in scene!");
                return;
            }
        }
        
        scoreScreenPanel.SetActive(true);
        Debug.Log($"ScoreScreenPanel activated: {scoreScreenPanel.activeSelf}");
        
        if (finalScoreText == null)
        {
            Debug.LogError("=== ERROR: finalScoreText is null! ===");
            // Try to find it
            GameObject found = FindGameObjectInScene("FinalScoreText");
            if (found != null)
            {
                finalScoreText = found.GetComponent<Text>();
                Debug.Log($"Found FinalScoreText: {finalScoreText != null}");
            }
            else
            {
                Debug.LogError("FinalScoreText not found in scene!");
                return;
            }
        }
        
        if (finalScoreText != null)
        {
            bool isMultiplayer = player2Score >= 0;
            string summaryText = "";
            
            if (isMultiplayer)
            {
                // Multiplayer mode - show both players' scores
                float p1Percentage = total > 0 ? (float)score / total * 100f : 0f;
                float p2Percentage = total > 0 ? (float)player2Score / total * 100f : 0f;
                string winner = score > player2Score ? "Player 1 Wins!" : (player2Score > score ? "Player 2 Wins!" : "It's a Tie!");
                
                summaryText = $"=== MULTIPLAYER RESULTS ===\n\n";
                summaryText += $"Player 1: {score}/{total} ({p1Percentage:F0}%)\n";
                summaryText += $"Player 2: {player2Score}/{total} ({p2Percentage:F0}%)\n";
                summaryText += $"\n{winner}\n";
            }
            else
            {
                // Single player mode
                float percentage = total > 0 ? (float)score / total * 100f : 0f;
                summaryText = $"Final Score: {score}/{total}\n{percentage:F0}%";
            }
            
            Debug.Log($"Creating summary text. Results count: {(results != null ? results.Count.ToString() : "null")}, Multiplayer: {isMultiplayer}");
            
            // Add summary of correct/incorrect answers
            if (isMultiplayer)
            {
                // Multiplayer summary - show both players
                if (results != null && results.Count > 0)
                {
                    summaryText += "\n--- Player 1 Summary ---\n";
                    
                    var correctAnswers = results.Where(r => r.isCorrect).ToList();
                    var incorrectAnswers = results.Where(r => !r.isCorrect).ToList();
                    
                    if (correctAnswers.Count > 0)
                    {
                        summaryText += $"✓ Correct ({correctAnswers.Count}): ";
                        summaryText += string.Join(", ", correctAnswers.Select(r => r.countryName));
                        summaryText += "\n";
                    }
                    
                    if (incorrectAnswers.Count > 0)
                    {
                        summaryText += $"✗ Wrong ({incorrectAnswers.Count}): ";
                        summaryText += string.Join(", ", incorrectAnswers.Select(r => r.countryName));
                        summaryText += "\n";
                    }
                }
                
                if (player2Results != null && player2Results.Count > 0)
                {
                    summaryText += "\n--- Player 2 Summary ---\n";
                    
                    var correctAnswers = player2Results.Where(r => r.isCorrect).ToList();
                    var incorrectAnswers = player2Results.Where(r => !r.isCorrect).ToList();
                    
                    if (correctAnswers.Count > 0)
                    {
                        summaryText += $"✓ Correct ({correctAnswers.Count}): ";
                        summaryText += string.Join(", ", correctAnswers.Select(r => r.countryName));
                        summaryText += "\n";
                    }
                    
                    if (incorrectAnswers.Count > 0)
                    {
                        summaryText += $"✗ Wrong ({incorrectAnswers.Count}): ";
                        summaryText += string.Join(", ", incorrectAnswers.Select(r => r.countryName));
                        summaryText += "\n";
                    }
                }
            }
            else
            {
                // Single player summary - detailed list
                if (results != null && results.Count > 0)
                {
                    summaryText += "\n--- Summary ---\n";
                    
                    // Show correct answers
                    var correctAnswers = results.Where(r => r.isCorrect).ToList();
                    if (correctAnswers.Count > 0)
                    {
                        summaryText += $"\n✓ Correct ({correctAnswers.Count}):\n";
                        foreach (var result in correctAnswers)
                        {
                            summaryText += $"  • {result.countryName}";
                            if (!string.IsNullOrEmpty(result.recognizedText))
                                summaryText += $" (you said: \"{result.recognizedText}\")";
                            summaryText += "\n";
                        }
                    }
                    
                    // Show incorrect answers
                    var incorrectAnswers = results.Where(r => !r.isCorrect).ToList();
                    if (incorrectAnswers.Count > 0)
                    {
                        summaryText += $"\n✗ Wrong ({incorrectAnswers.Count}):\n";
                        foreach (var result in incorrectAnswers)
                        {
                            summaryText += $"  • {result.countryName}";
                            if (!string.IsNullOrEmpty(result.recognizedText))
                                summaryText += $" (you said: \"{result.recognizedText}\")";
                            summaryText += "\n";
                        }
                    }
                }
            }
            
            Debug.Log($"=== SETTING FINAL SCORE TEXT ===\nText length: {summaryText.Length}\nFirst 200 chars: {summaryText.Substring(0, Mathf.Min(200, summaryText.Length))}");
            
            // Update ALL FinalScoreText components in the scene to prevent duplicates
            Text[] allFinalScoreTexts = scoreScreenPanel.GetComponentsInChildren<Text>(true);
            Debug.Log($"Found {allFinalScoreTexts.Length} Text components in ScoreScreenPanel");
            
            foreach (Text text in allFinalScoreTexts)
            {
                if (text.gameObject.name == "FinalScoreText" || text == finalScoreText)
                {
                    text.text = summaryText;
                    // Apply the same layout settings to all
                    RectTransform textRectComponent = text.GetComponent<RectTransform>();
                    if (textRectComponent != null)
                    {
                        if ((results != null && results.Count > 0) || isMultiplayer)
                        {
                            text.fontSize = isMultiplayer ? 18 : 14; // Smaller font to fit more
                            text.alignment = TextAnchor.UpperLeft; // Left align for better readability
                            // Leave room for buttons at bottom (buttons at 0.15 and 0.05)
                            textRectComponent.anchorMin = new Vector2(0.05f, 0.22f); // Start from 22% from bottom
                            textRectComponent.anchorMax = new Vector2(0.95f, 0.98f); // Go almost to top
                            textRectComponent.anchoredPosition = Vector2.zero;
                            textRectComponent.sizeDelta = Vector2.zero;
                            // Add padding
                            textRectComponent.offsetMin = new Vector2(20, 20);
                            textRectComponent.offsetMax = new Vector2(-20, -20);
                        }
                        else
                        {
                            // Center position for just score (no summary)
                            text.fontSize = 48;
                            text.alignment = TextAnchor.MiddleCenter;
                            textRectComponent.anchorMin = new Vector2(0.5f, 0.6f);
                            textRectComponent.anchorMax = new Vector2(0.5f, 0.6f);
                            textRectComponent.anchoredPosition = Vector2.zero;
                            textRectComponent.sizeDelta = new Vector2(400, 100);
                        }
                    }
                    Debug.Log($"Updated FinalScoreText: {text.gameObject.name}");
                }
            }
            
            // Also update the main reference
            finalScoreText.text = summaryText;
            
            // Verify it was set
            string actualText = finalScoreText.text;
            Debug.Log($"=== TEXT SET ===\nActual text length: {actualText.Length}\nFirst 200 chars: {actualText.Substring(0, Mathf.Min(200, actualText.Length))}\nMatch: {actualText == summaryText}");
            
            // Adjust text size and position based on content
            RectTransform textRect = finalScoreText.GetComponent<RectTransform>();
            if (textRect != null)
            {
                if ((results != null && results.Count > 0) || isMultiplayer)
                {
                    // Make text smaller and adjust position for summary
                    // Leave room for buttons at the bottom (buttons are at 0.15 and 0.05, so we use 0.22 as min to leave more room)
                    finalScoreText.fontSize = isMultiplayer ? 18 : 14; // Smaller font to fit more
                    finalScoreText.alignment = TextAnchor.UpperLeft; // Left align for better readability
                    // Make text area larger to fit summary, but leave bottom 22% for buttons
                    textRect.anchorMin = new Vector2(0.05f, 0.22f); // Start from 22% from bottom
                    textRect.anchorMax = new Vector2(0.95f, 0.98f); // Go almost to top
                    textRect.anchoredPosition = Vector2.zero;
                    textRect.sizeDelta = Vector2.zero;
                    // Add padding
                    textRect.offsetMin = new Vector2(20, 20);
                    textRect.offsetMax = new Vector2(-20, -20);
                    Debug.Log($"Text area expanded for summary. Rect: anchorMin={textRect.anchorMin}, anchorMax={textRect.anchorMax}, Multiplayer: {isMultiplayer}");
                }
                else
                {
                    // Center position for just score (no summary)
                    finalScoreText.fontSize = 48;
                    finalScoreText.alignment = TextAnchor.MiddleCenter;
                    textRect.anchorMin = new Vector2(0.5f, 0.6f);
                    textRect.anchorMax = new Vector2(0.5f, 0.6f);
                    textRect.anchoredPosition = Vector2.zero;
                    textRect.sizeDelta = new Vector2(400, 100);
                    Debug.Log($"Text area centered for score only. Rect: anchorMin={textRect.anchorMin}, anchorMax={textRect.anchorMax}");
                }
            }
            else
            {
                Debug.LogWarning("finalScoreText RectTransform is null!");
                // Fallback: just adjust font size
                if (results != null && results.Count > 5)
                {
                    finalScoreText.fontSize = 16;
                }
                else
                {
                    finalScoreText.fontSize = 20;
                }
            }
            
            // Ensure text is visible
            finalScoreText.gameObject.SetActive(true);
            if (finalScoreText.transform.parent != null)
            {
                finalScoreText.transform.parent.gameObject.SetActive(true);
            }
            
            Debug.Log($"=== FINAL CHECK ===\nText visible: {finalScoreText.gameObject.activeSelf}\nParent active: {(finalScoreText.transform.parent != null ? finalScoreText.transform.parent.gameObject.activeSelf.ToString() : "null")}\nText component enabled: {finalScoreText.enabled}");
        }
    }
    
    private void OnReturnToMenu()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ReturnToMenu();
        }
        ShowMenuUI();
    }
    
    private void OnPlayAgain()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RestartGame();
        }
        ShowGameUI();
    }
    
    private AudioClip microphoneClip;
    private string microphoneDevice;
    private int microphoneSampleRate = 44100;
    private int lastMicrophonePosition = 0;
    private bool isMicrophoneActive = false;
    
    private void Update()
    {
        // Update microphone volume indicator
        if (isMicrophoneActive)
        {
            UpdateMicrophoneVolume();
        }
        
        // Update timer display if game is in progress
        if (GameManager.Instance != null && GameManager.Instance.GameInProgress)
        {
            Timer timer = FindFirstObjectByType<Timer>();
            if (timer != null && timer.IsRunning)
            {
                UpdateTimer(timer.TimeRemaining);
            }
            
            // Update score
            UpdateScore(GameManager.Instance.CurrentScore, GameManager.Instance.QuestionsPerGame);
        }
    }
    
    private void UpdateMicrophoneVolume()
    {
        if (microphoneVolumeIndicator == null || !microphoneVolumeIndicator.gameObject.activeSelf)
            return;
        
        float volume = GetMicrophoneVolume();
        
        // Update visual indicator (scale or color based on volume)
        if (microphoneVolumeIndicator != null)
        {
            // Change color from green (quiet) to red (loud)
            Color indicatorColor = Color.Lerp(Color.green, Color.red, volume);
            microphoneVolumeIndicator.color = indicatorColor;
            
            // Scale the indicator based on volume (0.5 to 1.5 scale)
            RectTransform rect = microphoneVolumeIndicator.GetComponent<RectTransform>();
            if (rect != null)
            {
                float scale = 0.5f + (volume * 1.0f); // Scale from 0.5 to 1.5
                rect.localScale = new Vector3(scale, scale, 1f);
            }
        }
    }
    
    private float GetMicrophoneVolume()
    {
        if (microphoneClip == null)
        {
            // Start microphone recording
            if (Microphone.devices.Length > 0)
            {
                try
                {
                    microphoneDevice = Microphone.devices[0];
                    microphoneClip = Microphone.Start(microphoneDevice, true, 1, microphoneSampleRate);
                    lastMicrophonePosition = 0;
                    isMicrophoneActive = true;
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning($"Failed to start microphone: {e.Message}");
                    isMicrophoneActive = false;
                    return 0f;
                }
            }
            else
            {
                isMicrophoneActive = false;
                return 0f;
            }
        }
        
        if (microphoneClip == null || !isMicrophoneActive)
            return 0f;
        
        int currentPosition = Microphone.GetPosition(microphoneDevice);
        if (currentPosition < 0 || microphoneDevice == null)
            return 0f;
        
        // Get audio data
        int samplesToRead = currentPosition - lastMicrophonePosition;
        if (samplesToRead < 0)
            samplesToRead += microphoneClip.samples;
        
        if (samplesToRead > 0)
        {
            float[] samples = new float[samplesToRead];
            microphoneClip.GetData(samples, lastMicrophonePosition);
            
            // Calculate RMS (Root Mean Square) for volume
            float sum = 0f;
            for (int i = 0; i < samples.Length; i++)
            {
                sum += samples[i] * samples[i];
            }
            float rms = Mathf.Sqrt(sum / samples.Length);
            
            // Normalize to 0-1 range and amplify
            float volume = Mathf.Clamp01(rms * 10f);
            
            lastMicrophonePosition = currentPosition;
            return volume;
        }
        
        return 0f;
    }
    
    private void OnDestroy()
    {
        // Stop microphone when done
        if (microphoneClip != null && !string.IsNullOrEmpty(microphoneDevice))
        {
            Microphone.End(microphoneDevice);
            isMicrophoneActive = false;
        }
    }
}

