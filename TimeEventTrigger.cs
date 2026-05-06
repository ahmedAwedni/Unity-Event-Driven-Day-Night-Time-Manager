// 3. TimeEventTrigger.cs
using UnityEngine;
using UnityEngine.Events;


// Attach this to objects that need to react to a specific time of day 
// (e.g., Street lamps turning on at 18:00, or an NPC leaving their house at 08:00).
public class TimeEventTrigger : MonoBehaviour
{
    [Header("Trigger Time")]
    [Range(0, 23)] public int triggerHour = 18;
    [Range(0, 59)] public int triggerMinute = 0;

    [Header("Action")]
    public UnityEvent onTimeReached;

    // Optional: Only trigger once per day, or every single day?
    public bool triggerEveryDay = true;
    private bool hasTriggeredToday = false;

    private void OnEnable()
    {
        TimeManager.OnTimeChanged += CheckTime;
        TimeManager.OnDayChanged += ResetTrigger;
    }

    private void OnDisable()
    {
        TimeManager.OnTimeChanged -= CheckTime;
        TimeManager.OnDayChanged -= ResetTrigger;
    }

    private void CheckTime(int hour, int minute)
    {
        if (hasTriggeredToday && !triggerEveryDay) return;

        if (hour == triggerHour && minute == triggerMinute)
        {
            hasTriggeredToday = true;
            onTimeReached?.Invoke();
        }
    }

    private void ResetTrigger(int newDay)
    {
        if (triggerEveryDay)
        {
            hasTriggeredToday = false;
        }
    }
}
