using UnityEngine;

public class AimDownSights : MonoBehaviour
{
    [SerializeField] private PlayerController playerController; // Reference to the PlayerController
    [SerializeField] private float defaultFOV = 60.0f;         // Default field of view
    [SerializeField] private float aimedFOV = 45.0f;           // Field of view when aiming
    [SerializeField] private float smoothFOV = 10.0f;           // Smoothness of FOV transition
    [SerializeField] private Vector3 hipPosition;               // Position of the weapon when not aiming
    [SerializeField] private Vector3 aimPosition;               // Position of the weapon when aiming
    [SerializeField] private float smoothAim = 12.5f;           // Smoothness of aim position transition
    [SerializeField] private Camera camera;                     // Reference to the camera

    void Update()
    {
        // Return early if the inventory is active, preventing aiming
        if (playerController.inventory.activeSelf)
            return;

        // Handle aiming based on right mouse button input
        if (Input.GetMouseButton(1))
        {
            Aim();
        }
        else
        {
            HipFire();
        }
    }

    private void Aim()
    {
        // Interpolates the weapon's position to the aim position
        transform.localPosition = Vector3.Lerp(transform.localPosition, aimPosition, Time.deltaTime * smoothAim);

        // Check if the camera is assigned before modifying the FOV
        if (camera != null)
        {
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, aimedFOV, Time.deltaTime * smoothFOV);
        }
        else
        {
            Debug.LogWarning("Camera reference is not assigned!", this);
        }
    }

    private void HipFire()
    {
        // Interpolates the weapon's position to the hip position
        transform.localPosition = Vector3.Lerp(transform.localPosition, hipPosition, Time.deltaTime * smoothAim);

        // Check if the camera is assigned before modifying the FOV
        if (camera != null)
        {
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, defaultFOV, Time.deltaTime * smoothFOV);
        }
    }
}