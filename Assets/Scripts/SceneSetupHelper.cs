#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

/// <summary>
/// Helper script to quickly set up game scenes with required components
/// </summary>
public class SceneSetupHelper : EditorWindow
{
    [MenuItem("Game/Setup Scenes")]
    public static void ShowWindow()
    {
        GetWindow<SceneSetupHelper>("Scene Setup Helper");
    }
    
    private void OnGUI()
    {
        GUILayout.Label("Scene Setup Helper", EditorStyles.boldLabel);
        GUILayout.Space(10);
        
        if (GUILayout.Button("Setup MainMenu Scene"))
        {
            SetupMainMenuScene();
        }
        
        GUILayout.Space(5);
        
        if (GUILayout.Button("Setup GameScene"))
        {
            SetupGameScene();
        }
        
        GUILayout.Space(10);
        
        if (GUILayout.Button("Setup MainMenu UI"))
        {
            SetupMainMenuUI();
        }
        
        GUILayout.Space(5);
        
        if (GUILayout.Button("Setup GameScene UI"))
        {
            SetupGameSceneUI();
        }
        
        GUILayout.Space(10);
        
        if (GUILayout.Button("Add Scenes to Build Settings"))
        {
            AddScenesToBuildSettings();
        }
        
        GUILayout.Space(10);
        GUILayout.Label("Note: This creates basic scene structure.", EditorStyles.helpBox);
        GUILayout.Label("Use the UI setup buttons to automatically create UI elements.", EditorStyles.helpBox);
        GUILayout.Label("IMPORTANT: Add scenes to Build Settings before testing!", EditorStyles.helpBox);
    }
    
    [MenuItem("Game/Setup Build Settings")]
    private static void AddScenesToBuildSettings()
    {
        List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>();
        
        // Add MainMenu scene (index 0)
        string mainMenuPath = "Assets/Scenes/MainMenu.unity";
        if (System.IO.File.Exists(mainMenuPath))
        {
            scenes.Add(new EditorBuildSettingsScene(mainMenuPath, true));
            Debug.Log($"Added MainMenu to Build Settings");
        }
        else
        {
            Debug.LogWarning($"MainMenu scene not found at {mainMenuPath}");
        }
        
        // Add GameScene (index 1)
        string gameScenePath = "Assets/Scenes/GameScene.unity";
        if (System.IO.File.Exists(gameScenePath))
        {
            scenes.Add(new EditorBuildSettingsScene(gameScenePath, true));
            Debug.Log($"Added GameScene to Build Settings");
        }
        else
        {
            Debug.LogWarning($"GameScene not found at {gameScenePath}");
        }
        
        // Apply the changes
        EditorBuildSettings.scenes = scenes.ToArray();
        
        EditorUtility.DisplayDialog("Success", $"Added {scenes.Count} scene(s) to Build Settings:\n- MainMenu (index 0)\n- GameScene (index 1)", "OK");
        Debug.Log("Build Settings updated successfully!");
    }
    
    private void SetupMainMenuScene()
    {
        // Create or load MainMenu scene
        string scenePath = "Assets/Scenes/MainMenu.unity";
        UnityEngine.SceneManagement.Scene scene;
        
        if (System.IO.File.Exists(scenePath))
        {
            scene = EditorSceneManager.OpenScene(scenePath);
        }
        else
        {
            scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
            EditorSceneManager.SaveScene(scene, scenePath);
        }
        
        // Clear existing scene (optional - comment out if you want to keep existing objects)
        // GameObject[] allObjects = Object.FindObjectsOfType<GameObject>();
        // foreach (GameObject obj in allObjects) { DestroyImmediate(obj); }
        
        // Create GameManager
        GameObject gameManagerObj = new GameObject("GameManager");
        gameManagerObj.AddComponent<GameManager>();
        
        // Create UIManager
        GameObject uiManagerObj = new GameObject("UIManager");
        uiManagerObj.AddComponent<UIManager>();
        
        // Create MainMenuController
        GameObject menuControllerObj = new GameObject("MainMenuController");
        menuControllerObj.AddComponent<MainMenuController>();
        
        // Create Canvas
        GameObject canvasObj = new GameObject("Canvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObj.AddComponent<UnityEngine.UI.CanvasScaler>();
        canvasObj.AddComponent<UnityEngine.UI.GraphicRaycaster>();
        
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        
        Debug.Log("MainMenu scene setup complete!");
    }
    
    private void SetupGameScene()
    {
        // Create or load GameScene
        string scenePath = "Assets/Scenes/GameScene.unity";
        UnityEngine.SceneManagement.Scene scene;
        
        if (System.IO.File.Exists(scenePath))
        {
            scene = EditorSceneManager.OpenScene(scenePath);
        }
        else
        {
            scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
            EditorSceneManager.SaveScene(scene, scenePath);
        }
        
        // Create GameManager (or use DontDestroyOnLoad from menu)
        GameObject gameManagerObj = GameObject.Find("GameManager");
        if (gameManagerObj == null)
        {
            gameManagerObj = new GameObject("GameManager");
            gameManagerObj.AddComponent<GameManager>();
        }
        
        // Create FlagGameController
        GameObject flagControllerObj = new GameObject("FlagGameController");
        flagControllerObj.AddComponent<FlagGameController>();
        
        // Create Timer
        GameObject timerObj = new GameObject("Timer");
        Timer timer = timerObj.AddComponent<Timer>();
        timerObj.AddComponent<TimerUIUpdater>();
        
        // Create SpeechRecognitionHandler
        GameObject speechHandlerObj = new GameObject("SpeechRecognitionHandler");
        speechHandlerObj.AddComponent<SpeechRecognitionHandler>();
        
        // Create UIManager
        GameObject uiManagerObj = GameObject.Find("UIManager");
        if (uiManagerObj == null)
        {
            uiManagerObj = new GameObject("UIManager");
            uiManagerObj.AddComponent<UIManager>();
        }
        
        // Create Canvas
        GameObject canvasObj = GameObject.Find("Canvas");
        if (canvasObj == null)
        {
            canvasObj = new GameObject("Canvas");
            Canvas canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<UnityEngine.UI.CanvasScaler>();
            canvasObj.AddComponent<UnityEngine.UI.GraphicRaycaster>();
        }
        
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        
        Debug.Log("GameScene setup complete!");
    }
    
    [MenuItem("Game/Setup UI/MainMenu UI")]
    public static void SetupMainMenuUI()
    {
        // Check if in play mode
        if (Application.isPlaying)
        {
            EditorUtility.DisplayDialog("Error", "Cannot set up UI while in Play Mode. Please stop the game first.", "OK");
            return;
        }
        
        // Load MainMenu scene
        string scenePath = "Assets/Scenes/MainMenu.unity";
        if (!System.IO.File.Exists(scenePath))
        {
            EditorUtility.DisplayDialog("Error", "MainMenu scene not found! Please create it first using Game > Setup Scenes.", "OK");
            return;
        }
        
        UnityEngine.SceneManagement.Scene scene = EditorSceneManager.OpenScene(scenePath);
        
        // Find or create Canvas
        GameObject canvasObj = GameObject.Find("Canvas");
        if (canvasObj == null)
        {
            canvasObj = new GameObject("Canvas");
            Canvas canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<UnityEngine.UI.CanvasScaler>();
            canvasObj.AddComponent<UnityEngine.UI.GraphicRaycaster>();
        }
        
        // Find or create EventSystem
        if (GameObject.Find("EventSystem") == null)
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
            eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        }
        
        // Create Menu Panel
        GameObject menuPanel = GameObject.Find("MenuPanel");
        if (menuPanel == null)
        {
            menuPanel = new GameObject("MenuPanel");
            menuPanel.transform.SetParent(canvasObj.transform, false);
            UnityEngine.UI.Image panelImage = menuPanel.AddComponent<UnityEngine.UI.Image>();
            panelImage.color = new Color(0.2f, 0.2f, 0.2f, 0.8f);
            RectTransform panelRect = menuPanel.GetComponent<RectTransform>();
            panelRect.anchorMin = Vector2.zero;
            panelRect.anchorMax = Vector2.one;
            panelRect.sizeDelta = Vector2.zero;
        }
        
        // Create Title Text
        GameObject titleText = GameObject.Find("TitleText");
        if (titleText == null)
        {
            titleText = new GameObject("TitleText");
            titleText.transform.SetParent(menuPanel.transform, false);
            UnityEngine.UI.Text text = titleText.AddComponent<UnityEngine.UI.Text>();
            text.text = "Flag Game";
            // Font will use Unity's default (Unity 6 doesn't require explicit font assignment)
            text.fontSize = 48;
            text.alignment = TextAnchor.MiddleCenter;
            text.color = Color.white;
            RectTransform titleRect = titleText.GetComponent<RectTransform>();
            titleRect.anchorMin = new Vector2(0.5f, 0.8f);
            titleRect.anchorMax = new Vector2(0.5f, 0.8f);
            titleRect.anchoredPosition = Vector2.zero;
            titleRect.sizeDelta = new Vector2(400, 60);
        }
        
        // Create Flags Button
        GameObject flagsButton = GameObject.Find("FlagsButton");
        if (flagsButton == null)
        {
            flagsButton = new GameObject("FlagsButton");
            flagsButton.transform.SetParent(menuPanel.transform, false);
            UnityEngine.UI.Button button = flagsButton.AddComponent<UnityEngine.UI.Button>();
            UnityEngine.UI.Image buttonImage = flagsButton.AddComponent<UnityEngine.UI.Image>();
            buttonImage.color = new Color(0.2f, 0.6f, 0.9f);
            
            // Button text
            GameObject buttonTextObj = new GameObject("Text");
            buttonTextObj.transform.SetParent(flagsButton.transform, false);
            UnityEngine.UI.Text buttonText = buttonTextObj.AddComponent<UnityEngine.UI.Text>();
            buttonText.text = "Flags";
            // Font will use Unity's default (Unity 6 doesn't require explicit font assignment)
            buttonText.fontSize = 24;
            buttonText.alignment = TextAnchor.MiddleCenter;
            buttonText.color = Color.white;
            RectTransform textRect = buttonTextObj.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = Vector2.zero;
            
            RectTransform buttonRect = flagsButton.GetComponent<RectTransform>();
            buttonRect.anchorMin = new Vector2(0.5f, 0.6f);
            buttonRect.anchorMax = new Vector2(0.5f, 0.6f);
            buttonRect.anchoredPosition = Vector2.zero;
            buttonRect.sizeDelta = new Vector2(200, 50);
        }
        
        // Create Single Player Button
        GameObject singlePlayerButton = GameObject.Find("SinglePlayerButton");
        if (singlePlayerButton == null)
        {
            singlePlayerButton = new GameObject("SinglePlayerButton");
            singlePlayerButton.transform.SetParent(menuPanel.transform, false);
            UnityEngine.UI.Button button = singlePlayerButton.AddComponent<UnityEngine.UI.Button>();
            UnityEngine.UI.Image buttonImage = singlePlayerButton.AddComponent<UnityEngine.UI.Image>();
            buttonImage.color = new Color(0.2f, 0.6f, 0.9f);
            
            // Button text
            GameObject buttonTextObj = new GameObject("Text");
            buttonTextObj.transform.SetParent(singlePlayerButton.transform, false);
            UnityEngine.UI.Text buttonText = buttonTextObj.AddComponent<UnityEngine.UI.Text>();
            buttonText.text = "Single Player";
            // Font will use Unity's default (Unity 6 doesn't require explicit font assignment)
            buttonText.fontSize = 24;
            buttonText.alignment = TextAnchor.MiddleCenter;
            buttonText.color = Color.white;
            RectTransform textRect = buttonTextObj.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = Vector2.zero;
            
            RectTransform buttonRect = singlePlayerButton.GetComponent<RectTransform>();
            buttonRect.anchorMin = new Vector2(0.5f, 0.5f);
            buttonRect.anchorMax = new Vector2(0.5f, 0.5f);
            buttonRect.anchoredPosition = Vector2.zero;
            buttonRect.sizeDelta = new Vector2(200, 50);
            
            // Set inactive initially
            singlePlayerButton.SetActive(false);
        }
        
        // Create Multiplayer Button
        GameObject multiplayerButton = GameObject.Find("MultiplayerButton");
        if (multiplayerButton == null)
        {
            multiplayerButton = new GameObject("MultiplayerButton");
            multiplayerButton.transform.SetParent(menuPanel.transform, false);
            UnityEngine.UI.Button button = multiplayerButton.AddComponent<UnityEngine.UI.Button>();
            UnityEngine.UI.Image buttonImage = multiplayerButton.AddComponent<UnityEngine.UI.Image>();
            buttonImage.color = new Color(0.2f, 0.6f, 0.9f);
            
            // Button text
            GameObject buttonTextObj = new GameObject("Text");
            buttonTextObj.transform.SetParent(multiplayerButton.transform, false);
            UnityEngine.UI.Text buttonText = buttonTextObj.AddComponent<UnityEngine.UI.Text>();
            buttonText.text = "Multiplayer";
            // Font will use Unity's default (Unity 6 doesn't require explicit font assignment)
            buttonText.fontSize = 24;
            buttonText.alignment = TextAnchor.MiddleCenter;
            buttonText.color = Color.white;
            RectTransform textRect = buttonTextObj.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = Vector2.zero;
            
            RectTransform buttonRect = multiplayerButton.GetComponent<RectTransform>();
            buttonRect.anchorMin = new Vector2(0.5f, 0.4f);
            buttonRect.anchorMax = new Vector2(0.5f, 0.4f);
            buttonRect.anchoredPosition = Vector2.zero;
            buttonRect.sizeDelta = new Vector2(200, 50);
            
            // Set inactive initially
            multiplayerButton.SetActive(false);
        }
        
        // Try to assign references to UIManager
        GameObject uiManagerObj = GameObject.Find("UIManager");
        if (uiManagerObj != null)
        {
            UIManager uiManager = uiManagerObj.GetComponent<UIManager>();
            if (uiManager != null)
            {
                // Use reflection to set private fields
                System.Reflection.FieldInfo menuPanelField = typeof(UIManager).GetField("menuPanel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                System.Reflection.FieldInfo flagsButtonField = typeof(UIManager).GetField("flagsButton", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                System.Reflection.FieldInfo singlePlayerButtonField = typeof(UIManager).GetField("singlePlayerButton", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                System.Reflection.FieldInfo multiplayerButtonField = typeof(UIManager).GetField("multiplayerButton", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                
                if (menuPanelField != null) menuPanelField.SetValue(uiManager, menuPanel);
                if (flagsButtonField != null) flagsButtonField.SetValue(uiManager, flagsButton.GetComponent<UnityEngine.UI.Button>());
                if (singlePlayerButtonField != null) singlePlayerButtonField.SetValue(uiManager, singlePlayerButton.GetComponent<UnityEngine.UI.Button>());
                if (multiplayerButtonField != null) multiplayerButtonField.SetValue(uiManager, multiplayerButton.GetComponent<UnityEngine.UI.Button>());
                
                EditorUtility.SetDirty(uiManager);
            }
        }
        
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        
        EditorUtility.DisplayDialog("Success", "MainMenu UI setup complete!\n\nUI elements created:\n- TitleText\n- FlagsButton\n- SinglePlayerButton (inactive)\n- MultiplayerButton (inactive)\n\nReferences have been assigned to UIManager.", "OK");
        Debug.Log("MainMenu UI setup complete!");
    }
    
    [MenuItem("Game/Setup UI/GameScene UI")]
    public static void SetupGameSceneUI()
    {
        // Check if in play mode
        if (Application.isPlaying)
        {
            EditorUtility.DisplayDialog("Error", "Cannot set up UI while in Play Mode. Please stop the game first.", "OK");
            return;
        }
        
        // Load GameScene
        string scenePath = "Assets/Scenes/GameScene.unity";
        if (!System.IO.File.Exists(scenePath))
        {
            EditorUtility.DisplayDialog("Error", "GameScene not found! Please create it first using Game > Setup Scenes.", "OK");
            return;
        }
        
        UnityEngine.SceneManagement.Scene scene = EditorSceneManager.OpenScene(scenePath);
        
        // Find or create Canvas
        GameObject canvasObj = GameObject.Find("Canvas");
        if (canvasObj == null)
        {
            canvasObj = new GameObject("Canvas");
            Canvas canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<UnityEngine.UI.CanvasScaler>();
            canvasObj.AddComponent<UnityEngine.UI.GraphicRaycaster>();
        }
        
        // Find or create EventSystem
        if (GameObject.Find("EventSystem") == null)
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
            eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        }
        
        // Create Game Panel
        GameObject gamePanel = GameObject.Find("GamePanel");
        if (gamePanel == null)
        {
            gamePanel = new GameObject("GamePanel");
            gamePanel.transform.SetParent(canvasObj.transform, false);
            // Ensure RectTransform exists (should be added automatically when parented to Canvas)
            RectTransform panelRect = gamePanel.GetComponent<RectTransform>();
            if (panelRect == null)
            {
                panelRect = gamePanel.AddComponent<RectTransform>();
            }
            panelRect.anchorMin = Vector2.zero;
            panelRect.anchorMax = Vector2.one;
            panelRect.sizeDelta = Vector2.zero;
        }
        
        // Create Flag Image
        GameObject flagImage = GameObject.Find("FlagImage");
        if (flagImage == null)
        {
            flagImage = new GameObject("FlagImage");
            flagImage.transform.SetParent(gamePanel.transform, false);
            UnityEngine.UI.Image image = flagImage.AddComponent<UnityEngine.UI.Image>();
            image.color = Color.white;
            image.preserveAspect = true; // Keep flag proportions correct
            image.enabled = true; // Ensure Image is enabled
            RectTransform imageRect = flagImage.GetComponent<RectTransform>();
            imageRect.anchorMin = new Vector2(0.5f, 0.5f);
            imageRect.anchorMax = new Vector2(0.5f, 0.5f);
            imageRect.anchoredPosition = Vector2.zero;
            imageRect.sizeDelta = new Vector2(400, 300);
        }
        
        // Create Question Number Text
        GameObject questionNumberText = GameObject.Find("QuestionNumberText");
        if (questionNumberText == null)
        {
            questionNumberText = new GameObject("QuestionNumberText");
            questionNumberText.transform.SetParent(gamePanel.transform, false);
            UnityEngine.UI.Text text = questionNumberText.AddComponent<UnityEngine.UI.Text>();
            text.text = "Question 1/10";
            // Font will use Unity's default (Unity 6 doesn't require explicit font assignment)
            text.fontSize = 24;
            text.alignment = TextAnchor.MiddleCenter;
            text.color = Color.white;
            RectTransform textRect = questionNumberText.GetComponent<RectTransform>();
            textRect.anchorMin = new Vector2(0.5f, 0.9f);
            textRect.anchorMax = new Vector2(0.5f, 0.9f);
            textRect.anchoredPosition = Vector2.zero;
            textRect.sizeDelta = new Vector2(300, 40);
        }
        
        // Create Timer Text
        GameObject timerText = GameObject.Find("TimerText");
        if (timerText == null)
        {
            timerText = new GameObject("TimerText");
            timerText.transform.SetParent(gamePanel.transform, false);
            UnityEngine.UI.Text text = timerText.AddComponent<UnityEngine.UI.Text>();
            text.text = "10";
            // Font will use Unity's default (Unity 6 doesn't require explicit font assignment)
            text.fontSize = 48;
            text.alignment = TextAnchor.MiddleCenter;
            text.color = Color.yellow;
            RectTransform textRect = timerText.GetComponent<RectTransform>();
            textRect.anchorMin = new Vector2(0.5f, 0.3f);
            textRect.anchorMax = new Vector2(0.5f, 0.3f);
            textRect.anchoredPosition = Vector2.zero;
            textRect.sizeDelta = new Vector2(200, 60);
        }
        
        // Create Score Text
        GameObject scoreText = GameObject.Find("ScoreText");
        if (scoreText == null)
        {
            scoreText = new GameObject("ScoreText");
            scoreText.transform.SetParent(gamePanel.transform, false);
            UnityEngine.UI.Text text = scoreText.AddComponent<UnityEngine.UI.Text>();
            text.text = "Score: 0/10";
            // Font will use Unity's default (Unity 6 doesn't require explicit font assignment)
            text.fontSize = 24;
            text.alignment = TextAnchor.MiddleLeft;
            text.color = Color.white;
            RectTransform textRect = scoreText.GetComponent<RectTransform>();
            textRect.anchorMin = new Vector2(0.05f, 0.95f);
            textRect.anchorMax = new Vector2(0.05f, 0.95f);
            textRect.anchoredPosition = Vector2.zero;
            textRect.sizeDelta = new Vector2(200, 40);
        }
        
        // Create Listening Text
        GameObject listeningText = GameObject.Find("ListeningText");
        if (listeningText == null)
        {
            listeningText = new GameObject("ListeningText");
            listeningText.transform.SetParent(gamePanel.transform, false);
            UnityEngine.UI.Text text = listeningText.AddComponent<UnityEngine.UI.Text>();
            text.text = "🎤 Listening... Say a country name";
            // Font will use Unity's default (Unity 6 doesn't require explicit font assignment)
            text.fontSize = 24;
            text.alignment = TextAnchor.MiddleCenter;
            text.color = Color.cyan;
            RectTransform textRect = listeningText.GetComponent<RectTransform>();
            textRect.anchorMin = new Vector2(0.5f, 0.15f);
            textRect.anchorMax = new Vector2(0.5f, 0.15f);
            textRect.anchoredPosition = Vector2.zero;
            textRect.sizeDelta = new Vector2(400, 40); // Wider to fit full text
        }
        
        // Create Microphone Volume Indicator
        GameObject micIndicator = GameObject.Find("MicrophoneVolumeIndicator");
        if (micIndicator == null)
        {
            micIndicator = new GameObject("MicrophoneVolumeIndicator");
            micIndicator.transform.SetParent(gamePanel.transform, false);
            UnityEngine.UI.Image micImage = micIndicator.AddComponent<UnityEngine.UI.Image>();
            micImage.color = Color.green;
            RectTransform micRect = micIndicator.GetComponent<RectTransform>();
            micRect.anchorMin = new Vector2(0.5f, 0.08f);
            micRect.anchorMax = new Vector2(0.5f, 0.08f);
            micRect.anchoredPosition = Vector2.zero;
            micRect.sizeDelta = new Vector2(60, 60);
            micIndicator.SetActive(false);
        }
        
        // Create Feedback Panel
        GameObject feedbackPanel = GameObject.Find("FeedbackPanel");
        if (feedbackPanel == null)
        {
            feedbackPanel = new GameObject("FeedbackPanel");
            feedbackPanel.transform.SetParent(gamePanel.transform, false);
            UnityEngine.UI.Image panelImage = feedbackPanel.AddComponent<UnityEngine.UI.Image>();
            panelImage.color = new Color(0, 0, 0, 0.7f);
            RectTransform panelRect = feedbackPanel.GetComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(0.5f, 0.1f);
            panelRect.anchorMax = new Vector2(0.5f, 0.1f);
            panelRect.anchoredPosition = Vector2.zero;
            panelRect.sizeDelta = new Vector2(600, 120); // Larger to fit more text
            feedbackPanel.SetActive(false);
            
            // Feedback Text
            GameObject feedbackTextObj = new GameObject("FeedbackText");
            feedbackTextObj.transform.SetParent(feedbackPanel.transform, false);
            UnityEngine.UI.Text feedbackText = feedbackTextObj.AddComponent<UnityEngine.UI.Text>();
            feedbackText.text = "Correct!";
            // Font will use Unity's default (Unity 6 doesn't require explicit font assignment)
            feedbackText.fontSize = 28; // Slightly smaller to fit more text
            feedbackText.alignment = TextAnchor.MiddleCenter;
            feedbackText.color = Color.green;
            RectTransform feedbackRect = feedbackTextObj.GetComponent<RectTransform>();
            feedbackRect.anchorMin = Vector2.zero;
            feedbackRect.anchorMax = Vector2.one;
            feedbackRect.sizeDelta = Vector2.zero;
            feedbackRect.offsetMin = new Vector2(10, 10); // Padding
            feedbackRect.offsetMax = new Vector2(-10, -10);
        }
        
        // Create Wrong Answer Side Panel
        GameObject wrongAnswerPanel = GameObject.Find("WrongAnswerPanel");
        if (wrongAnswerPanel == null)
        {
            wrongAnswerPanel = new GameObject("WrongAnswerPanel");
            wrongAnswerPanel.transform.SetParent(gamePanel.transform, false);
            UnityEngine.UI.Image wrongPanelImage = wrongAnswerPanel.AddComponent<UnityEngine.UI.Image>();
            wrongPanelImage.color = new Color(0.3f, 0, 0, 0.9f); // Dark red background - more visible
            RectTransform wrongPanelRect = wrongAnswerPanel.GetComponent<RectTransform>();
            wrongPanelRect.anchorMin = new Vector2(0.85f, 0.2f); // Right side - wider (15% of screen)
            wrongPanelRect.anchorMax = new Vector2(0.98f, 0.85f); // Right side, top portion - taller
            wrongPanelRect.anchoredPosition = Vector2.zero;
            wrongPanelRect.sizeDelta = Vector2.zero;
            wrongAnswerPanel.SetActive(false);
            
            // Wrong Answer Text
            GameObject wrongAnswerTextObj = new GameObject("WrongAnswerText");
            wrongAnswerTextObj.transform.SetParent(wrongAnswerPanel.transform, false);
            UnityEngine.UI.Text wrongAnswerText = wrongAnswerTextObj.AddComponent<UnityEngine.UI.Text>();
            wrongAnswerText.text = "Wrong attempts:";
            wrongAnswerText.fontSize = 16;
            wrongAnswerText.alignment = TextAnchor.UpperLeft;
            wrongAnswerText.color = Color.red;
            RectTransform wrongTextRect = wrongAnswerTextObj.GetComponent<RectTransform>();
            wrongTextRect.anchorMin = Vector2.zero;
            wrongTextRect.anchorMax = Vector2.one;
            wrongTextRect.sizeDelta = Vector2.zero;
            wrongTextRect.offsetMin = new Vector2(5, 5); // Padding
            wrongTextRect.offsetMax = new Vector2(-5, -5);
        }
        
        // Create Score Screen Panel
        GameObject scoreScreenPanel = GameObject.Find("ScoreScreenPanel");
        if (scoreScreenPanel == null)
        {
            scoreScreenPanel = new GameObject("ScoreScreenPanel");
            scoreScreenPanel.transform.SetParent(canvasObj.transform, false);
            UnityEngine.UI.Image panelImage = scoreScreenPanel.AddComponent<UnityEngine.UI.Image>();
            panelImage.color = new Color(0.1f, 0.1f, 0.1f, 0.95f);
            RectTransform panelRect = scoreScreenPanel.GetComponent<RectTransform>();
            panelRect.anchorMin = Vector2.zero;
            panelRect.anchorMax = Vector2.one;
            panelRect.sizeDelta = Vector2.zero;
            scoreScreenPanel.SetActive(false);
            
            // Final Score Text
            GameObject finalScoreText = new GameObject("FinalScoreText");
            finalScoreText.transform.SetParent(scoreScreenPanel.transform, false);
            UnityEngine.UI.Text text = finalScoreText.AddComponent<UnityEngine.UI.Text>();
            text.text = "Final Score: 0/10\n0%";
            // Font will use Unity's default (Unity 6 doesn't require explicit font assignment)
            text.fontSize = 48;
            text.alignment = TextAnchor.MiddleCenter;
            text.color = Color.white;
            RectTransform textRect = finalScoreText.GetComponent<RectTransform>();
            textRect.anchorMin = new Vector2(0.5f, 0.6f);
            textRect.anchorMax = new Vector2(0.5f, 0.6f);
            textRect.anchoredPosition = Vector2.zero;
            textRect.sizeDelta = new Vector2(400, 100);
            
            // Return to Menu Button
            GameObject returnToMenuButton = new GameObject("ReturnToMenuButton");
            returnToMenuButton.transform.SetParent(scoreScreenPanel.transform, false);
            UnityEngine.UI.Button returnButton = returnToMenuButton.AddComponent<UnityEngine.UI.Button>();
            UnityEngine.UI.Image returnButtonImage = returnToMenuButton.AddComponent<UnityEngine.UI.Image>();
            returnButtonImage.color = new Color(0.2f, 0.6f, 0.9f);
            RectTransform returnButtonRect = returnToMenuButton.GetComponent<RectTransform>();
            returnButtonRect.anchorMin = new Vector2(0.5f, 0.15f);
            returnButtonRect.anchorMax = new Vector2(0.5f, 0.15f);
            returnButtonRect.anchoredPosition = Vector2.zero;
            returnButtonRect.sizeDelta = new Vector2(200, 50);
            
            GameObject returnButtonTextObj = new GameObject("Text");
            returnButtonTextObj.transform.SetParent(returnToMenuButton.transform, false);
            UnityEngine.UI.Text returnButtonText = returnButtonTextObj.AddComponent<UnityEngine.UI.Text>();
            returnButtonText.text = "Return to Menu";
            // Font will use Unity's default
            returnButtonText.fontSize = 24;
            returnButtonText.alignment = TextAnchor.MiddleCenter;
            returnButtonText.color = Color.white;
            RectTransform returnTextRect = returnButtonTextObj.GetComponent<RectTransform>();
            returnTextRect.anchorMin = Vector2.zero;
            returnTextRect.anchorMax = Vector2.one;
            returnTextRect.sizeDelta = Vector2.zero;
            
            // Play Again Button
            GameObject playAgainButton = new GameObject("PlayAgainButton");
            playAgainButton.transform.SetParent(scoreScreenPanel.transform, false);
            UnityEngine.UI.Button playButton = playAgainButton.AddComponent<UnityEngine.UI.Button>();
            UnityEngine.UI.Image playButtonImage = playAgainButton.AddComponent<UnityEngine.UI.Image>();
            playButtonImage.color = new Color(0.2f, 0.8f, 0.2f);
            RectTransform playButtonRect = playAgainButton.GetComponent<RectTransform>();
            playButtonRect.anchorMin = new Vector2(0.5f, 0.05f);
            playButtonRect.anchorMax = new Vector2(0.5f, 0.05f);
            playButtonRect.anchoredPosition = Vector2.zero;
            playButtonRect.sizeDelta = new Vector2(200, 50);
            
            GameObject playButtonTextObj = new GameObject("Text");
            playButtonTextObj.transform.SetParent(playAgainButton.transform, false);
            UnityEngine.UI.Text playButtonText = playButtonTextObj.AddComponent<UnityEngine.UI.Text>();
            playButtonText.text = "Play Again";
            // Font will use Unity's default
            playButtonText.fontSize = 24;
            playButtonText.alignment = TextAnchor.MiddleCenter;
            playButtonText.color = Color.white;
            RectTransform playTextRect = playButtonTextObj.GetComponent<RectTransform>();
            playTextRect.anchorMin = Vector2.zero;
            playTextRect.anchorMax = Vector2.one;
            playTextRect.sizeDelta = Vector2.zero;
        }
        
        // Try to assign references to UIManager
        GameObject uiManagerObj = GameObject.Find("UIManager");
        if (uiManagerObj != null)
        {
            UIManager uiManager = uiManagerObj.GetComponent<UIManager>();
            if (uiManager != null)
            {
                // Use reflection to set private fields
                System.Reflection.FieldInfo gamePanelField = typeof(UIManager).GetField("gamePanel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                System.Reflection.FieldInfo flagImageField = typeof(UIManager).GetField("flagImage", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                System.Reflection.FieldInfo questionNumberTextField = typeof(UIManager).GetField("questionNumberText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                System.Reflection.FieldInfo timerTextField = typeof(UIManager).GetField("timerText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                System.Reflection.FieldInfo scoreTextField = typeof(UIManager).GetField("scoreText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                System.Reflection.FieldInfo listeningTextField = typeof(UIManager).GetField("listeningText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                System.Reflection.FieldInfo micIndicatorField = typeof(UIManager).GetField("microphoneVolumeIndicator", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                System.Reflection.FieldInfo feedbackPanelField = typeof(UIManager).GetField("feedbackPanel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                System.Reflection.FieldInfo feedbackTextField = typeof(UIManager).GetField("feedbackText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                System.Reflection.FieldInfo wrongAnswerPanelField = typeof(UIManager).GetField("wrongAnswerPanel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                System.Reflection.FieldInfo wrongAnswerTextField = typeof(UIManager).GetField("wrongAnswerText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                System.Reflection.FieldInfo scoreScreenPanelField = typeof(UIManager).GetField("scoreScreenPanel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                System.Reflection.FieldInfo finalScoreTextField = typeof(UIManager).GetField("finalScoreText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                System.Reflection.FieldInfo returnToMenuButtonField = typeof(UIManager).GetField("returnToMenuButton", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                System.Reflection.FieldInfo playAgainButtonField = typeof(UIManager).GetField("playAgainButton", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                
                if (gamePanelField != null) gamePanelField.SetValue(uiManager, gamePanel);
                if (flagImageField != null) flagImageField.SetValue(uiManager, flagImage.GetComponent<UnityEngine.UI.Image>());
                if (questionNumberTextField != null) questionNumberTextField.SetValue(uiManager, questionNumberText.GetComponent<UnityEngine.UI.Text>());
                if (timerTextField != null) timerTextField.SetValue(uiManager, timerText.GetComponent<UnityEngine.UI.Text>());
                if (scoreTextField != null) scoreTextField.SetValue(uiManager, scoreText.GetComponent<UnityEngine.UI.Text>());
                if (listeningTextField != null) listeningTextField.SetValue(uiManager, listeningText.GetComponent<UnityEngine.UI.Text>());
                if (micIndicatorField != null) micIndicatorField.SetValue(uiManager, GameObject.Find("MicrophoneVolumeIndicator")?.GetComponent<UnityEngine.UI.Image>());
                if (feedbackPanelField != null) feedbackPanelField.SetValue(uiManager, feedbackPanel);
                if (feedbackTextField != null) feedbackTextField.SetValue(uiManager, GameObject.Find("FeedbackText")?.GetComponent<UnityEngine.UI.Text>());
                if (wrongAnswerPanelField != null) wrongAnswerPanelField.SetValue(uiManager, wrongAnswerPanel);
                if (wrongAnswerTextField != null) wrongAnswerTextField.SetValue(uiManager, GameObject.Find("WrongAnswerText")?.GetComponent<UnityEngine.UI.Text>());
                if (scoreScreenPanelField != null) scoreScreenPanelField.SetValue(uiManager, scoreScreenPanel);
                if (finalScoreTextField != null) finalScoreTextField.SetValue(uiManager, GameObject.Find("FinalScoreText")?.GetComponent<UnityEngine.UI.Text>());
                if (returnToMenuButtonField != null) returnToMenuButtonField.SetValue(uiManager, GameObject.Find("ReturnToMenuButton")?.GetComponent<UnityEngine.UI.Button>());
                if (playAgainButtonField != null) playAgainButtonField.SetValue(uiManager, GameObject.Find("PlayAgainButton")?.GetComponent<UnityEngine.UI.Button>());
                
                EditorUtility.SetDirty(uiManager);
            }
        }
        
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        
        EditorUtility.DisplayDialog("Success", "GameScene UI setup complete!\n\nUI elements created:\n- GamePanel\n- FlagImage\n- QuestionNumberText\n- TimerText\n- ScoreText\n- ListeningText\n- FeedbackPanel\n- ScoreScreenPanel\n\nReferences have been assigned to UIManager.", "OK");
        Debug.Log("GameScene UI setup complete!");
    }
}
#endif

