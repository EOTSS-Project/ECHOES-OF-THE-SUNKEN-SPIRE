using UnityEngine;

public class RotatingObstacle : MonoBehaviour
{
    [Header("Obstacle Spin Settings")]
    [SerializeField] private float spinSpeed = 60f;
    [SerializeField] private Vector3 rotationAxis = Vector3.up;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (rb != null)
        {
            Quaternion deltaRotation = Quaternion.Euler(rotationAxis.normalized * spinSpeed * Time.fixedDeltaTime);
            rb.MoveRotation(rb.rotation * deltaRotation);
        }
    }

    void Update()
    {
        if (rb == null)
        {
            transform.Rotate(rotationAxis * spinSpeed * Time.deltaTime, Space.Self);
        }
    }
}