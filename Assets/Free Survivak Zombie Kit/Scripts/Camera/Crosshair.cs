using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour
{
    // Crosshair preset enum
    public enum Preset { None, PistolPreset }
    public Preset crosshairPreset = Preset.None;

    public bool showCrosshair = true;
    public Texture2D verticalTexture;
    public Texture2D horizontalTexture;

    // Crosshair dimensions
    public float crosshairLength = 10.0f;
    public float crosshairWidth = 3.0f;

    // Spread setup
    public float minSpread = 45.0f;
    public float maxSpread = 250.0f;
    public float spreadPerSecond = 150.0f;

    // Rotation
    public float rotationAngle = 0.0f;
    public float rotationSpeed = 0.0f;

    [HideInInspector] public float spread;   // Current spread value

    void Update()
    {
        // Update the rotation angle based on speed
        rotationAngle = (rotationAngle + rotationSpeed * Time.deltaTime) % 360;
    }

    public void IncreaseSpread(float multiplier)
    {
        spread = Mathf.Clamp(spread + spreadPerSecond * multiplier * Time.deltaTime, minSpread, maxSpread);
        Debug.Log("Increase spread with multiplier: " + multiplier + " Current Spread: " + spread);
    }

    public void DecreaseSpread(float multiplier)
    {
        spread = Mathf.Clamp(spread - spreadPerSecond * multiplier * Time.deltaTime, minSpread, maxSpread);
        Debug.Log("Decrease spread with multiplier: " + multiplier + " Current Spread: " + spread);
    }

    void OnGUI()
    {
        if (showCrosshair && verticalTexture && horizontalTexture)
        {
            // Set the pivot point to the center of the screen
            Vector2 pivot = new Vector2(Screen.width / 2, Screen.height / 2);

            // Apply rotation
            GUIUtility.RotateAroundPivot(rotationAngle, pivot);

            // Draw the crosshair based on the preset
            if (crosshairPreset == Preset.PistolPreset)
            {
                DrawCrosshair(14, 3); // Pistol crosshair size
            }
            else if (crosshairPreset == Preset.None)
            {
                DrawCrosshair(crosshairLength, crosshairWidth); // Default crosshair size
            }
        }
    }

    // Helper method to draw the crosshair
    private void DrawCrosshair(float length, float width)
    {
        GUIStyle verticalStyle = new GUIStyle();
        GUIStyle horizontalStyle = new GUIStyle();
        verticalStyle.normal.background = verticalTexture;
        horizontalStyle.normal.background = horizontalTexture;

        // Horizontal bars
        GUI.Box(new Rect((Screen.width - width) / 2, (Screen.height - spread) / 2 - length, width, length), GUIContent.none, horizontalStyle);
        GUI.Box(new Rect((Screen.width - width) / 2, (Screen.height + spread) / 2, width, length), GUIContent.none, horizontalStyle);

        // Vertical bars
        GUI.Box(new Rect((Screen.width - spread) / 2 - length, (Screen.height - width) / 2, length, width), GUIContent.none, verticalStyle);
        GUI.Box(new Rect((Screen.width + spread) / 2, (Screen.height - width) / 2, length, width), GUIContent.none, verticalStyle);
    }
}