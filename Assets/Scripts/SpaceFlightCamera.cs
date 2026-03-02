using UnityEngine;

public class SpaceFlightCamera : MonoBehaviour
{
    [Header("Targeting")]
    public Transform target; // Drag your ship here
    public Vector3 offset = new Vector3(0, 2.5f, -7.5f); // Position relative to ship

    [Header("Smoothing")]
    public float positionSmoothSpeed = 0.125f;
    public float rotationSmoothSpeed = 5f;

    [Header("Dynamic FOV")]
    public Camera cam;
    public float baseFOV = 60f;
    public float maxFOV = 75f;
    public float fovSpeedMultiplier = 0.2f;

    private Rigidbody targetRb;

    void Start()
    {
        if (target != null)
            targetRb = target.GetComponent<Rigidbody>();
        
        if (cam == null)
            cam = GetComponent<Camera>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (target == null) return;

        HandlePosition();
        HandleRotation();
        HandleFOV();
    }

    void HandlePosition()
    {
        // Calculate the ideal position in world space
        Vector3 desiredPosition = target.TransformPoint(offset);
        
        // Smoothly move the camera toward that position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, positionSmoothSpeed);
    }

    void HandleRotation()
    {
        // Look at a point slightly in front of the ship for better leading
        Vector3 lookTarget = target.position + (target.forward * 10f);
        Quaternion desiredRotation = Quaternion.LookRotation(lookTarget - transform.position, target.up);
        
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSmoothSpeed * Time.deltaTime);
    }

    void HandleFOV()
    {
        if (targetRb == null) return;

        // Increase FOV based on ship velocity
        float currentSpeed = targetRb.linearVelocity.magnitude;
        float targetFOV = baseFOV + (currentSpeed * fovSpeedMultiplier);
        
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, Mathf.Min(targetFOV, maxFOV), Time.deltaTime * 2f);
    }
}