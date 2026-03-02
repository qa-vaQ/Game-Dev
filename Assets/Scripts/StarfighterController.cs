using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class StarfighterController : MonoBehaviour
{
    [Header("Speed Settings")]
    public float forwardThrust = 100f;
    public float maxSpeed = 50f;
    public float strafeThrust = 30f; // Minimal side-to-side for "space" feel

    [Header("Rotation Sensitivity")]
    public float pitchForce = 60f;
    public float yawForce = 40f;
    public float rollForce = 50f;

    [Header("Jet Handling")]
    [Tooltip("How much the ship tilts when turning")]
    public float bankAmount = 0.5f; 
    [Tooltip("How much the ship 'slips' in turns. Higher = sharper turns.")]
    public float gripLevel = 5f; 

    private Rigidbody rb;
    private float activeForwardSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // High drag prevents the ship from spinning forever
        rb.angularDamping = 2.5f; 
        rb.linearDamping = 0.5f;
    }

    void FixedUpdate()
    {
        HandleMovement();
        HandleRotation();
        ApplyAerodynamics();
    }

    void HandleMovement()
    {
        // New Input System way to get W/S or Up/Down keys
        float thrustInput = 0;
        if (Keyboard.current.wKey.isPressed) thrustInput = 1f;
        if (Keyboard.current.sKey.isPressed) thrustInput = -1f;
        
        rb.AddRelativeForce(Vector3.forward * thrustInput * forwardThrust);

        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }

    void HandleRotation()
    {
        // Get Mouse Delta (how much the mouse moved this frame)
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();
        
        // Get Roll (A/D keys)
        float rollInput = 0;
        if (Keyboard.current.aKey.isPressed) rollInput = 1f;
        if (Keyboard.current.dKey.isPressed) rollInput = -1f;

        // Apply forces
        float p = -mouseDelta.y * pitchForce * 0.1f; // Sensitivity adjustment
        float y = mouseDelta.x * yawForce * 0.1f;
        float r = rollInput * rollForce;

        rb.AddRelativeTorque(Vector3.right * p);
        rb.AddRelativeTorque(Vector3.up * y);
        rb.AddRelativeTorque(Vector3.forward * r);

        // Auto-Banking logic remains the same
        float bankRotation = -y * bankAmount;
        rb.AddRelativeTorque(Vector3.forward * bankRotation);
    }

    void ApplyAerodynamics()
    {
        // This is the "Secret Sauce"
        // It pushes the ship's velocity vector toward the direction it's facing
        // Without this, you'd just slide sideways like a bar of soap.
        if (rb.linearVelocity.magnitude > 1f)
        {
            rb.linearVelocity = Vector3.Lerp(
                rb.linearVelocity, 
                transform.forward * rb.linearVelocity.magnitude, 
                Time.fixedDeltaTime * gripLevel
            );
        }
    }
}
