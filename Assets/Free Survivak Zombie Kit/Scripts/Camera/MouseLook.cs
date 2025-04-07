using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public GameObject player;         // Reference to the player object
    public float xSensitivity = 10;   // Sensitivity for horizontal (X) mouse movement
    public float ySensitivity = 10;   // Sensitivity for vertical (Y) mouse movement
    public float smoothing = 0.4f;    // Controls how smooth the rotation transitions are
    public int min = -60;             // Minimum clamp for vertical look angle (pitch)
    public int max = 60;              // Maximum clamp for vertical look angle (pitch)

    private float yRotation = 0f;     // Vertical rotation (up/down), clamped
    private float xRotation = 0f;     // Horizontal rotation (left/right)

    void Start()
    {
        // Initialize rotation based on current rotation at the start
        yRotation = transform.localEulerAngles.x;
        xRotation = player.transform.localEulerAngles.y;
    }

    void Update()
    {
        // Get mouse input for vertical and horizontal movement
        float mouseY = Input.GetAxis("Mouse Y") * ySensitivity;
        float mouseX = Input.GetAxis("Mouse X") * xSensitivity;

        // Update and clamp vertical rotation (pitch)
        yRotation -= mouseY;
        yRotation = Mathf.Clamp(yRotation, min, max);

        // Update horizontal rotation (yaw)
        xRotation += mouseX;

        // Apply the vertical rotation to the camera
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(yRotation, 0f, 0f), Time.deltaTime / smoothing);

        // Apply the horizontal rotation to the player
        player.transform.rotation = Quaternion.Lerp(player.transform.rotation, Quaternion.Euler(0f, xRotation, 0f), Time.deltaTime / smoothing);
    }
}