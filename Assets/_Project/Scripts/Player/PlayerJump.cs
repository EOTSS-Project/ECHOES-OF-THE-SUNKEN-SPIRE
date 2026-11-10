using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] private float jumpForce = 4f;
    [SerializeField] private LayerMask groundLayers;

    private Rigidbody rb;
    private InputAction jumpAction;
    private PlayerCarry carry;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        carry = GetComponent<PlayerCarry>();
        jumpAction = InputSystem.actions.FindAction("Jump");
    }

    void Update()
    {
        if (!jumpAction.WasPressedThisFrame()) return;
        if (!IsGrounded()) return;

        float jumpMultiplier = carry != null ? carry.CurrentJumpMultiplier : 1f;

        // rb.mass ile çarparak artan kütlenin zıplamayı sıfırlamasını engelliyoruz
        Vector3 jumpImpulse = Vector3.up * (jumpForce * jumpMultiplier * rb.mass);
        rb.AddForce(jumpImpulse, ForceMode.Impulse);
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f, groundLayers);
    }
}