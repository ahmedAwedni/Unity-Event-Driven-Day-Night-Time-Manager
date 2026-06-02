// DayNightController.cs
using UnityEngine;

// Listens to the TimeManager and smoothly updates the Sun and Moon's rotation, color, and intensity.
public class DayNightController : MonoBehaviour
{
    [Header("Sun Settings")]
    public Light sunLight;
    [Tooltip("The color of the sun based on the time of day (0.0 = Midnight, 0.5 = Noon, 1.0 = Midnight)")]
    public Gradient sunColor;
    [Tooltip("The intensity of the sun. Map this so it dips to 0 at night and peaks at noon.")]
    public AnimationCurve sunIntensity;

    [Header("Moon Settings")]
    public Light moonLight;
    [Tooltip("The color of the moon based on the time of day.")]
    public Gradient moonColor;
    [Tooltip("The intensity of the moon. Map this so it peaks at midnight (0.0 & 1.0) and dips to 0 at noon (0.5).")]
    public AnimationCurve moonIntensity;

    [Header("Rotation Settings")]
    [Tooltip("The axis the celestial bodies rotate around. Usually X.")]
    public Vector3 rotationAxis = Vector3.right;

    private void OnEnable()
    {
        TimeManager.OnTimeChanged += UpdateLighting;
    }

    private void OnDisable()
    {
        TimeManager.OnTimeChanged -= UpdateLighting;
    }

    private void UpdateLighting(int hour, int minute)
    {
        // Get a value from 0.0 to 1.0 representing the day
        float timePercent = TimeManager.Instance.GetNormalizedTimeOfDay();

        // Calculate base angle (0.0 = Midnight = -90 degrees so it points up from the bottom)
        float sunAngle = (timePercent * 360f) - 90f;

        // 1. Update Sun
        if (sunLight != null)
        {
            sunLight.transform.rotation = Quaternion.AngleAxis(sunAngle, rotationAxis);
            sunLight.color = sunColor.Evaluate(timePercent);
            sunLight.intensity = sunIntensity.Evaluate(timePercent);
        }

        // 2. Update Moon (Offset exactly 180 degrees from the sun)
        if (moonLight != null)
        {
            float moonAngle = sunAngle + 180f;
            moonLight.transform.rotation = Quaternion.AngleAxis(moonAngle, rotationAxis);
            moonLight.color = moonColor.Evaluate(timePercent);
            moonLight.intensity = moonIntensity.Evaluate(timePercent);
        }
    }
}
