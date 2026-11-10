using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float acceleration = 40f;
    [SerializeField] private float turnSpeed = 720f;
    [SerializeField] private Transform cameraTransform;

    private Rigidbody rb;
    private InputAction moveAction;
    private Vector3 direction;
    private float grip = 1f;
    private PlayerCarry carry;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        carry = GetComponent<PlayerCarry>();
        moveAction = InputSystem.actions.FindAction("Move");

        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();

        if (input.sqrMagnitude > 0.01f && cameraTransform != null)
        {
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;

            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            direction = (camRight * input.x + camForward * input.y).normalized;
        }
        else
        {
            direction = Vector3.zero;
        }
    }

    void FixedUpdate()
    {
        float speedMultiplier = carry != null ? carry.CurrentSpeedMultiplier : 1f;
        Vector3 wanted = direction * (maxSpeed * speedMultiplier);
        Vector3 v = rb.linearVelocity;
        Vector3 flat = new Vector3(v.x, 0f, v.z);

        float step = acceleration * grip * Time.fixedDeltaTime;
        flat = Vector3.MoveTowards(flat, wanted, step);
        rb.linearVelocity = new Vector3(flat.x, v.y, flat.z);

        if (direction == Vector3.zero) return;

        Quaternion look = Quaternion.LookRotation(direction);
        float turn = turnSpeed * Time.fixedDeltaTime;
        look = Quaternion.RotateTowards(rb.rotation, look, turn);
        rb.MoveRotation(look);
    }

    public void SetGrip(float value)
    {
        grip = value;
    }
}