using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameMode
{
    SinglePlayer,
    Multiplayer
}

public enum GameCategory
{
    Flags
    // Add more categories here in the future
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [Header("Game Settings")]
    [SerializeField] private GameCategory currentCategory;
    [SerializeField] private GameMode currentGameMode;
    [SerializeField] private int questionsPerGame = 10;
    
    [Header("Components")]
    [SerializeField] private FlagGameController flagGameController;
    
    private int currentScore = 0;
    private int questionsAnswered = 0;
    private bool gameInProgress = false;
    
    public GameCategory CurrentCategory => currentCategory;
    public GameMode CurrentGameMode => currentGameMode;
    public int CurrentScore => currentScore;
    public int QuestionsAnswered => questionsAnswered;
    public int QuestionsPerGame => questionsPerGame;
    public bool GameInProgress => gameInProgress;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void StartGame(GameCategory category, GameMode mode)
    {
        currentCategory = category;
        currentGameMode = mode;
        currentScore = 0;
        questionsAnswered = 0;
        gameInProgress = true;
        Debug.Log($"Game started. Score reset to: {currentScore}/{questionsAnswered}");
        
        // Load appropriate game scene based on category
        switch (category)
        {
            case GameCategory.Flags:
                // Load GameScene if not already loaded
                if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "GameScene")
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
                    // StartFlagGame will be called after scene loads
                    StartCoroutine(WaitForSceneLoad());
                }
                else
                {
                    StartFlagGame();
                }
                break;
            default:
                Debug.LogError($"Unknown category: {category}");
                break;
        }
    }
    
    private System.Collections.IEnumerator WaitForSceneLoad()
    {
        // Wait until scene is fully loaded
        while (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "GameScene")
        {
            yield return null;
        }
        
        // Wait one more frame for all objects to initialize
        yield return null;
        
        StartFlagGame();
    }
    
    private void StartFlagGame()
    {
        if (flagGameController == null)
        {
            flagGameController = FindFirstObjectByType<FlagGameController>();
        }
        
        if (flagGameController != null)
        {
            flagGameController.StartGame(questionsPerGame);
        }
        else
        {
            Debug.LogError("FlagGameController not found in scene!");
        }
    }
    
    public void OnQuestionAnswered(bool isCorrect)
    {
        questionsAnswered++;
        
        if (isCorrect)
        {
            currentScore++;
            Debug.Log($"Score updated: {currentScore}/{questionsAnswered} (isCorrect: {isCorrect})");
        }
        else
        {
            Debug.Log($"Answer incorrect. Score: {currentScore}/{questionsAnswered}");
        }
        
        // Don't call EndGame here - let FlagGameController handle it when all questions are done
        // This ensures the results list is passed correctly
    }
    
    private void EndGame()
    {
        gameInProgress = false;
        // This is now only called as a fallback
        // FlagGameController should handle showing the score screen with results
        Debug.Log($"EndGame called. Final score: {currentScore}/{questionsAnswered}");
    }
    
    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
    public void RestartGame()
    {
        StartGame(currentCategory, currentGameMode);
    }
}

