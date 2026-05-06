// 1. TimeManager.cs
using System;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    // Global Events for other scripts to listen to
    public static event Action<int, int> OnTimeChanged; // Passes Hour, Minute
    public static event Action<int> OnHourChanged;      // Passes Hour
    public static event Action<int> OnDayChanged;       // Passes Day

    [Header("Time Settings")]
    [Tooltip("How many real-life seconds does it take for one in-game hour to pass?")]
    public float realSecondsPerHour = 10f;
    
    [Header("Starting Time")]
    [Range(0, 23)] public int startingHour = 8;
    [Range(0, 59)] public int startingMinute = 0;
    public int startingDay = 1;

    // Internal tracking
    private float timer;
    public int CurrentMinute { get; private set; }
    public int CurrentHour { get; private set; }
    public int CurrentDay { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;

        // Initialize Time
        CurrentHour = startingHour;
        CurrentMinute = startingMinute;
        CurrentDay = startingDay;
    }

    private void Start()
    {
        // Fire initial events so UI and Lighting update immediately
        OnTimeChanged?.Invoke(CurrentHour, CurrentMinute);
        OnHourChanged?.Invoke(CurrentHour);
        OnDayChanged?.Invoke(CurrentDay);
    }

    private void Update()
    {
        // Calculate how much real time represents one in-game minute
        float realSecondsPerMinute = realSecondsPerHour / 60f;
        timer += Time.deltaTime;

        if (timer >= realSecondsPerMinute)
        {
            timer -= realSecondsPerMinute;
            AdvanceTime();
        }
    }

    private void AdvanceTime()
    {
        CurrentMinute++;

        if (CurrentMinute >= 60)
        {
            CurrentMinute = 0;
            CurrentHour++;
            OnHourChanged?.Invoke(CurrentHour);

            if (CurrentHour >= 24)
            {
                CurrentHour = 0;
                CurrentDay++;
                OnDayChanged?.Invoke(CurrentDay);
            }
        }

        OnTimeChanged?.Invoke(CurrentHour, CurrentMinute);
    }

    /// Returns the current time as a normalized float between 0.0 (Midnight) and 1.0 (Next Midnight).
    /// Perfect for lerping lighting colors or sun rotations.
    public float GetNormalizedTimeOfDay()
    {
        return (CurrentHour + (CurrentMinute / 60f)) / 24f;
    }
}
