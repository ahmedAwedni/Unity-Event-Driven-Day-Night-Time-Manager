// 2. DayNightController.cs
using UnityEngine;

// Listens to the TimeManager and smoothly updates the Directional Light's rotation, color, and intensity.
public class DayNightController : MonoBehaviour
{
    [Header("Sun / Moon Light")]
    public Light directionalLight;

    [Header("Lighting Settings")]
    [Tooltip("The color of the light based on the time of day (0.0 = Midnight, 0.5 = Noon, 1.0 = Midnight)")]
    public Gradient lightColor;
    
    [Tooltip("The intensity of the light. Map this so it dips to 0 at night and peaks at noon.")]
    public AnimationCurve lightIntensity;

    [Header("Rotation Settings")]
    [Tooltip("The axis the sun rotates around. Usually X.")]
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
        if (directionalLight == null) return;

        // Get a value from 0.0 to 1.0 representing the day
        float timePercent = TimeManager.Instance.GetNormalizedTimeOfDay();

        // 1. Update Rotation (360 degrees over 24 hours). 
        // We subtract 90 so that 0.0 (Midnight) points straight up from the bottom.
        float currentAngle = (timePercent * 360f) - 90f;
        directionalLight.transform.rotation = Quaternion.AngleAxis(currentAngle, rotationAxis);

        // 2. Update Color
        directionalLight.color = lightColor.Evaluate(timePercent);

        // 3. Update Intensity
        directionalLight.intensity = lightIntensity.Evaluate(timePercent);
    }
}
