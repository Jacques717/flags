using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private UIManager uiManager;
    
    private void Start()
    {
        if (uiManager == null)
        {
            uiManager = FindFirstObjectByType<UIManager>();
        }
        
        // Ensure we're showing the menu
        if (uiManager != null)
        {
            uiManager.ShowMenuUI();
        }
    }
    
    public void OnFlagsCategorySelected()
    {
        // Category selection is handled by UIManager
        // This method can be called from UI buttons if needed
        if (uiManager != null)
        {
            uiManager.OnCategorySelected(GameCategory.Flags);
        }
    }
    
    public void OnSinglePlayerSelected()
    {
        if (uiManager != null)
        {
            uiManager.OnGameModeSelected(GameMode.SinglePlayer);
        }
    }
    
    public void OnMultiplayerSelected()
    {
        if (uiManager != null)
        {
            uiManager.OnGameModeSelected(GameMode.Multiplayer);
        }
    }
}

