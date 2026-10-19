

using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Platform Elevator Settings")]
    [SerializeField] private float moveDistance = 5f;
    [SerializeField] private float speed = 2f;

    private Vector3 startPos;
    private float startTime;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPos = rb.position;
        startTime = Time.time;
    }

    void FixedUpdate()
    {
        float elapsedTime = Time.fixedTime - startTime;

        float t = (Mathf.Sin(elapsedTime * speed - Mathf.PI / 2f) + 1f) / 2f;

        float newY = startPos.y + t * moveDistance;

        Vector3 targetPos = new Vector3(
            startPos.x,
            newY,
            startPos.z
        );

        rb.MovePosition(targetPos);
    }
}

