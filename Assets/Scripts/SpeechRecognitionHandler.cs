using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
using UnityEngine.Windows.Speech;
#endif

public class SpeechRecognitionHandler : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool useKeyboardInputInEditor = true;
    // maxRecordingLength removed - not currently used
    
    [Header("Events")]
    public UnityEvent<string> OnSpeechRecognized;
    public UnityEvent OnSpeechStarted;
    public UnityEvent OnSpeechStopped;
    public UnityEvent<string> OnSpeechError;
    
    private bool isRecording = false;
    private bool isInitialized = false;
    
    // For editor testing - keyboard input
    private string keyboardInput = "";
    private bool waitingForKeyboardInput = false;
    
#if UNITY_ANDROID && !UNITY_EDITOR
    private AndroidJavaObject speechRecognizer;
    private AndroidJavaObject recognitionListener;
#elif UNITY_IOS && !UNITY_EDITOR
    // iOS speech recognition will be implemented via native plugin
#elif UNITY_STANDALONE_WIN && !UNITY_EDITOR
    // Windows: Using Unity's built-in KeywordRecognizer
    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, System.Action> keywordActions;
#endif
    
    private void Start()
    {
        InitializeSpeechRecognition();
    }
    
    private void OnDestroy()
    {
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
        if (keywordRecognizer != null)
        {
            if (keywordRecognizer.IsRunning)
            {
                keywordRecognizer.Stop();
            }
            keywordRecognizer.Dispose();
        }
#endif
    }
    
    private void InitializeSpeechRecognition()
    {
#if UNITY_EDITOR
        // In editor, use keyboard input for testing
        isInitialized = true;
        Debug.Log("Speech Recognition: Using keyboard input mode in Editor");
#elif UNITY_ANDROID && !UNITY_EDITOR
        InitializeAndroidSpeechRecognition();
#elif UNITY_IOS && !UNITY_EDITOR
        InitializeIOSSpeechRecognition();
#elif UNITY_STANDALONE_WIN && !UNITY_EDITOR
        InitializeWindowsSpeechRecognition();
#else
        Debug.LogWarning("Speech recognition not supported on this platform");
        isInitialized = false;
#endif
    }
    
#if UNITY_ANDROID && !UNITY_EDITOR
    private void InitializeAndroidSpeechRecognition()
    {
        try
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass speechRecognizerClass = new AndroidJavaClass("android.speech.SpeechRecognizer");
            
            recognitionListener = new AndroidJavaObject("com.yasirkula.unity.SpeechToTextListener", gameObject.name, "OnSpeechResult", "OnSpeechError");
            speechRecognizer = speechRecognizerClass.CallStatic<AndroidJavaObject>("createSpeechRecognizer", currentActivity);
            
            if (speechRecognizer != null)
            {
                speechRecognizer.Call("setRecognitionListener", recognitionListener);
                isInitialized = true;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to initialize Android speech recognition: {e.Message}");
            isInitialized = false;
        }
    }
    
    public void OnSpeechResult(string result)
    {
        OnSpeechRecognized?.Invoke(result);
        isRecording = false;
        OnSpeechStopped?.Invoke();
    }
    
    public void OnSpeechError(string error)
    {
        OnSpeechError?.Invoke(error);
        isRecording = false;
        OnSpeechStopped?.Invoke();
    }
#elif UNITY_IOS && !UNITY_EDITOR
    private void InitializeIOSSpeechRecognition()
    {
        // iOS implementation would go here using native plugin
        // For now, mark as not initialized
        isInitialized = false;
        Debug.LogWarning("iOS speech recognition not yet implemented - requires native plugin");
    }
#elif UNITY_STANDALONE_WIN && !UNITY_EDITOR
    private void InitializeWindowsSpeechRecognition()
    {
        try
        {
            // Create list of keywords (country names and variations)
            List<string> keywords = new List<string>
            {
                "United States", "United Kingdom", "France", "Germany", "Japan",
                "Canada", "Australia", "Brazil", "India", "Italy",
                "USA", "US", "America", "UK", "Britain", "England", "Britannia",
                "Deutschland", "Brasil", "Italia"
            };
            
            // Create keyword actions dictionary
            keywordActions = new Dictionary<string, System.Action>();
            foreach (string keyword in keywords)
            {
                string capturedKeyword = keyword; // Capture for closure
                keywordActions[keyword] = () => OnWindowsKeywordRecognized(capturedKeyword);
            }
            
            // Create KeywordRecognizer
            keywordRecognizer = new KeywordRecognizer(keywords.ToArray());
            keywordRecognizer.OnPhraseRecognized += OnWindowsPhraseRecognized;
            
            isInitialized = true;
            Debug.Log($"Windows Speech Recognition initialized with {keywords.Count} keywords");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to initialize Windows speech recognition: {e.Message}");
            Debug.LogWarning("Make sure microphone permissions are granted in Windows Settings");
            isInitialized = false;
        }
    }
    
    private void OnWindowsPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        string recognizedPhrase = args.text?.Trim() ?? "";
        
        Debug.Log($"=== WINDOWS SPEECH RECOGNITION ===\nRecognized: '{recognizedPhrase}'\nConfidence: {args.confidence}");
        
        // Always invoke the recognized text so the game can display it and check if it's correct
        // The game logic will determine if it matches the current question
        if (keywordActions.ContainsKey(recognizedPhrase))
        {
            Debug.Log($"Recognized phrase '{recognizedPhrase}' is in keyword list - invoking");
            keywordActions[recognizedPhrase].Invoke();
        }
        else
        {
            Debug.LogWarning($"Recognized phrase '{recognizedPhrase}' NOT in keyword list - but still reporting to game");
            // Still report it so user can see what was heard and game can check if it matches
            OnSpeechRecognized?.Invoke(recognizedPhrase);
            isRecording = false;
            OnSpeechStopped?.Invoke();
        }
    }
    
    private void OnWindowsKeywordRecognized(string recognizedText)
    {
        Debug.Log($"OnWindowsKeywordRecognized called with: '{recognizedText}'");
        OnSpeechRecognized?.Invoke(recognizedText);
        isRecording = false;
        OnSpeechStopped?.Invoke();
    }
    
    private void StartWindowsListening()
    {
        try
        {
            if (keywordRecognizer == null)
            {
                Debug.LogError("KeywordRecognizer is null! Speech recognition not initialized.");
                OnSpeechError?.Invoke("Speech recognition not initialized");
                isRecording = false;
                return;
            }
            
            if (!keywordRecognizer.IsRunning)
            {
                keywordRecognizer.Start();
                Debug.Log("Windows Speech Recognition started - speak a country name");
                Debug.Log($"KeywordRecognizer is running: {keywordRecognizer.IsRunning}");
                Debug.Log("Ready to recognize: United States, USA, US, America, United Kingdom, UK, Britain, England, France, Germany, Japan, Canada, Australia, Brazil, India, Italy");
            }
            else
            {
                Debug.LogWarning("KeywordRecognizer is already running");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to start Windows listening: {e.Message}");
            Debug.LogError($"Exception details: {e}");
            OnSpeechError?.Invoke(e.Message);
            isRecording = false;
        }
    }
    
    private void StopWindowsListening()
    {
        try
        {
            if (keywordRecognizer != null && keywordRecognizer.IsRunning)
            {
                keywordRecognizer.Stop();
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to stop Windows listening: {e.Message}");
        }
    }
#endif
    
    public void StartListening()
    {
        if (!isInitialized)
        {
            Debug.LogWarning("Speech recognition not initialized");
            return;
        }
        
        if (isRecording)
        {
            StopListening();
        }
        
        isRecording = true;
        OnSpeechStarted?.Invoke();
        
#if UNITY_EDITOR
        if (useKeyboardInputInEditor)
        {
            StartKeyboardInput();
        }
#elif UNITY_ANDROID && !UNITY_EDITOR
        StartAndroidListening();
#elif UNITY_IOS && !UNITY_EDITOR
        StartIOSListening();
#elif UNITY_STANDALONE_WIN && !UNITY_EDITOR
        StartWindowsListening();
#endif
    }
    
    public void StopListening()
    {
        if (!isRecording) return;
        
        isRecording = false;
        OnSpeechStopped?.Invoke();
        
#if UNITY_ANDROID && !UNITY_EDITOR
        if (speechRecognizer != null)
        {
            speechRecognizer.Call("stopListening");
        }
#elif UNITY_IOS && !UNITY_EDITOR
        // Stop iOS listening
#elif UNITY_STANDALONE_WIN && !UNITY_EDITOR
        StopWindowsListening();
#endif
    }
    
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_WSA
    private void StartKeyboardInput()
    {
        waitingForKeyboardInput = true;
        keyboardInput = "";
        Debug.Log("Speech Recognition: Type the country name and press Enter");
    }
    
    private void Update()
    {
        if (waitingForKeyboardInput && isRecording)
        {
            // Capture keyboard input
            foreach (char c in Input.inputString)
            {
                if (c == '\n' || c == '\r') // Enter key
                {
                    if (!string.IsNullOrEmpty(keyboardInput))
                    {
                        OnSpeechRecognized?.Invoke(keyboardInput.Trim());
                        waitingForKeyboardInput = false;
                        isRecording = false;
                        OnSpeechStopped?.Invoke();
                    }
                }
                else if (c == '\b') // Backspace
                {
                    if (keyboardInput.Length > 0)
                    {
                        keyboardInput = keyboardInput.Substring(0, keyboardInput.Length - 1);
                    }
                }
                else if (c >= 32 && c <= 126) // Only printable ASCII characters
                {
                    keyboardInput += c;
                    Debug.Log($"Keyboard input: '{keyboardInput}' (added char: '{c}')");
                }
            }
        }
    }
    
    private void OnGUI()
    {
        if (waitingForKeyboardInput && isRecording)
        {
            GUI.Label(new Rect(10, 10, 300, 20), "Type country name and press Enter:");
            GUI.Label(new Rect(10, 30, 300, 20), keyboardInput);
        }
    }
#endif
    
#if UNITY_ANDROID && !UNITY_EDITOR
    private void StartAndroidListening()
    {
        try
        {
            AndroidJavaClass recognizerIntentClass = new AndroidJavaClass("android.speech.RecognizerIntent");
            AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent", recognizerIntentClass.GetStatic<string>("ACTION_RECOGNIZE_SPEECH"));
            
            intent.Call<AndroidJavaObject>("putExtra", recognizerIntentClass.GetStatic<string>("EXTRA_LANGUAGE_MODEL"), 
                recognizerIntentClass.GetStatic<string>("LANGUAGE_MODEL_FREE_FORM"));
            intent.Call<AndroidJavaObject>("putExtra", recognizerIntentClass.GetStatic<string>("EXTRA_MAX_RESULTS"), 1);
            
            speechRecognizer.Call("startListening", intent);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to start Android listening: {e.Message}");
            OnSpeechError?.Invoke(e.Message);
            isRecording = false;
        }
    }
#endif
    
    public bool IsRecording => isRecording;
    public bool IsInitialized => isInitialized;
}

