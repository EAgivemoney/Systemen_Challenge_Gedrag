using System;
using UnityEngine;

// An Item that can be consumed, providing gains to health, hunger, and thirst
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Consumable")]
public class Consumable : MonoBehaviour
{
    public VitalsController.VitalType vitalType; // Reference to your VitalType enum

    public void UseItem(int amount)
    {
        // Ensure the string you are parsing is in lower case
        string vitalTypeString = vitalType.ToString().ToLower();

        // Parse the string back to VitalType enum
        if (Enum.TryParse(typeof(VitalsController.VitalType), vitalTypeString, true, out object result))
        {
            VitalsController.VitalType parsedVitalType = (VitalsController.VitalType)result;

            // Now you can use parsedVitalType to affect health, hunger, or thirst
            switch (parsedVitalType)
            {
                case VitalsController.VitalType.Health:
                    // Call method to increase health
                    break;
                case VitalsController.VitalType.Hunger:
                    // Call method to increase hunger
                    break;
                case VitalsController.VitalType.Thirst:
                    // Call method to increase thirst
                    break;
            }
        }
        else
        {
            Debug.LogError("Failed to parse VitalType from string: " + vitalTypeString);
        }
    }
}