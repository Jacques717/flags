using System;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [SerializeField] private float duration = 10f;
    [SerializeField] private bool autoStart = false;
    
    private float currentTime;
    private bool isRunning = false;
    
    public UnityEvent<float> OnTimeUpdate;
    public UnityEvent OnTimeExpired;
    
    public float CurrentTime => currentTime;
    public float Duration => duration;
    public bool IsRunning => isRunning;
    public float TimeRemaining => currentTime;
    public float Progress => 1f - (currentTime / duration);
    
    private void Start()
    {
        if (autoStart)
        {
            StartTimer();
        }
    }
    
    private void Update()
    {
        if (isRunning)
        {
            currentTime -= Time.deltaTime;
            OnTimeUpdate?.Invoke(currentTime);
            
            if (currentTime <= 0f)
            {
                currentTime = 0f;
                isRunning = false;
                OnTimeExpired?.Invoke();
            }
        }
    }
    
    public void StartTimer(float? customDuration = null)
    {
        duration = customDuration ?? duration;
        currentTime = duration;
        isRunning = true;
    }
    
    public void StopTimer()
    {
        isRunning = false;
    }
    
    public void ResetTimer()
    {
        currentTime = duration;
        isRunning = false;
    }
    
    public void PauseTimer()
    {
        isRunning = false;
    }
    
    public void ResumeTimer()
    {
        if (currentTime > 0f)
        {
            isRunning = true;
        }
    }
    
    public void AddTime(float additionalTime)
    {
        currentTime += additionalTime;
        Debug.Log($"Timer: Added {additionalTime} seconds. New time: {currentTime}");
    }
}

