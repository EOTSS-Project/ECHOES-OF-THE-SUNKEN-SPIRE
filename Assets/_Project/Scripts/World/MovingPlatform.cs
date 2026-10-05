using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Platform Elevator Settings")]
    [SerializeField] private float moveDistance = 5f; // metres travelled upward from the start position
    [SerializeField] private float speed = 2f;        // ride frequency: one full up-and-down takes 2*PI/speed seconds

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position; // the lowest point of the ride
    }

    void Update()
    {
        // 0 at the start, 1 at the top: the platform never sinks below where it began
        float t = (1f - Mathf.Cos(Time.time * speed)) * 0.5f;
        float newY = startPos.y + t * moveDistance;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}
