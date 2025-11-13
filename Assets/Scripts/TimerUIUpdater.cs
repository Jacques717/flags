using UnityEngine;

/// <summary>
/// Helper component to automatically update UI timer display
/// Attach this to the same GameObject as Timer component
/// </summary>
[RequireComponent(typeof(Timer))]
public class TimerUIUpdater : MonoBehaviour
{
    private Timer timer;
    private UIManager uiManager;
    
    private void Start()
    {
        timer = GetComponent<Timer>();
        uiManager = UIManager.Instance;
        
        if (timer != null)
        {
            timer.OnTimeUpdate.AddListener(OnTimerUpdate);
        }
    }
    
    private void OnTimerUpdate(float timeRemaining)
    {
        if (uiManager != null)
        {
            uiManager.UpdateTimer(timeRemaining);
        }
    }
    
    private void OnDestroy()
    {
        if (timer != null)
        {
            timer.OnTimeUpdate.RemoveListener(OnTimerUpdate);
        }
    }
}

