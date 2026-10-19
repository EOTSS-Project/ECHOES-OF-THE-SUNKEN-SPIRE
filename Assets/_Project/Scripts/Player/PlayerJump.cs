using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] private float jumpForce = 4f;
    [SerializeField] private LayerMask groundLayers; // Which layers count as ground

    private Rigidbody rb;
    private InputAction jumpAction;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        jumpAction = InputSystem.actions.FindAction("Jump");
    }

    void Update()
    {
        // Only in the frame Space / South button goes down
        if (!jumpAction.WasPressedThisFrame()) return;

        // Is there Ground right under our feet?
        if (!IsGrounded()) return; // No jumping in the air

        // One short kick upwards
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    bool IsGrounded()
    {
        // From our middle, 1.1 m down: half our height + 0.1
        return Physics.Raycast(transform.position, Vector3.down, 1.1f, groundLayers);
    }
}
