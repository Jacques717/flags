#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

/// <summary>
/// Automatically checks if UI setup is needed and prompts the user
/// </summary>
[InitializeOnLoad]
public class AutoSetupChecker
{
    static AutoSetupChecker()
    {
        // Check when Unity loads or recompiles
        EditorApplication.delayCall += CheckSetupNeeded;
    }
    
    private static void CheckSetupNeeded()
    {
        // Only check in GameScene
        if (UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().name == "GameScene")
        {
            // Check if WrongAnswerPanel exists
            GameObject wrongAnswerPanel = GameObject.Find("WrongAnswerPanel");
            if (wrongAnswerPanel == null)
            {
                // Show a dialog asking if they want to run setup
                if (EditorUtility.DisplayDialog(
                    "UI Setup Needed",
                    "Some UI elements are missing in the GameScene.\n\nWould you like to run the UI setup now?",
                    "Yes, Setup Now",
                    "No, I'll Do It Later"))
                {
                    SceneSetupHelper.SetupGameSceneUI();
                }
            }
        }
    }
}
#endif

