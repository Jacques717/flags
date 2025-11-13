using System;
using UnityEngine;
using UnityEngine.Events;

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
#elif UNITY_STANDALONE_WIN || UNITY_WSA
    // Windows: Using keyboard input fallback (System.Speech not available in Unity 6)
    // For real speech recognition, integrate UnitySpeechToText plugin
#endif
    
    private void Start()
    {
        InitializeSpeechRecognition();
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
#elif UNITY_STANDALONE_WIN || UNITY_WSA
        // Windows: Use keyboard input as fallback (System.Speech not available in Unity 6)
        // TODO: Integrate UnitySpeechToText plugin for real speech recognition
        isInitialized = true;
        Debug.Log("Speech Recognition: Using keyboard input mode on Windows (speech recognition requires plugin)");
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
#elif UNITY_STANDALONE_WIN || UNITY_WSA
        // Windows: Use keyboard input (same as editor)
        StartKeyboardInput();
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
#elif UNITY_STANDALONE_WIN || UNITY_WSA
        // Windows: Keyboard input doesn't need explicit stop
        waitingForKeyboardInput = false;
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
                else
                {
                    keyboardInput += c;
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

