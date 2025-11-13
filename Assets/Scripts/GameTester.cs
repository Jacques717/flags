#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Helper script for testing game logic without full UI setup
/// Attach to a GameObject in the scene for quick testing
/// </summary>
public class GameTester : MonoBehaviour
{
    [Header("Test Settings")]
    [SerializeField] private bool autoStartTest = false;
    [SerializeField] private int testQuestionCount = 3;
    
    private FlagGameController flagController;
    private GameManager gameManager;
    
    private void Start()
    {
        if (autoStartTest)
        {
            StartTest();
        }
    }
    
    [ContextMenu("Start Test Game")]
    public void StartTest()
    {
        Debug.Log("=== Starting Game Test ===");
        
        // Ensure GameManager exists
        if (gameManager == null)
        {
            gameManager = FindFirstObjectByType<GameManager>();
            if (gameManager == null)
            {
                GameObject gmObj = new GameObject("GameManager");
                gameManager = gmObj.AddComponent<GameManager>();
            }
        }
        
        // Ensure FlagGameController exists
        if (flagController == null)
        {
            flagController = FindFirstObjectByType<FlagGameController>();
            if (flagController == null)
            {
                GameObject fcObj = new GameObject("FlagGameController");
                flagController = fcObj.AddComponent<FlagGameController>();
            }
        }
        
        // Start game
        gameManager.StartGame(GameCategory.Flags, GameMode.SinglePlayer);
        
        Debug.Log($"Test game started with {testQuestionCount} questions");
        Debug.Log("In Editor: Type country name and press Enter to answer");
    }
    
    [ContextMenu("Test Answer Matching")]
    public void TestAnswerMatching()
    {
        Debug.Log("=== Testing Answer Matching ===");
        
        // Create a test flag question
        FlagQuestion testQuestion = new FlagQuestion(
            "United States",
            new List<string> { "united states", "usa", "america", "us" },
            null
        );
        
        // Test various inputs
        string[] testInputs = {
            "united states",
            "USA",
            "america",
            "us",
            "United States",
            "united state", // typo
            "france", // wrong answer
            "usa"
        };
        
        foreach (string input in testInputs)
        {
            bool isCorrect = AnswerMatcher.IsAnswerCorrect(input, testQuestion);
            Debug.Log($"Input: '{input}' -> Correct: {isCorrect}");
        }
    }
}
#endif

