
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Platform Elevator Settings")]
    [SerializeField] private float moveDistance = 5f;
    [SerializeField] private float speed = 2f;

    private Vector3 startPos;
    private float startTime;

    void Start()
    {
        startPos = transform.position;
        startTime = Time.time;
    }

    void Update()
    {
        // Normalize sine value to keep the platform between 0 and 1
        float elapsedTime = Time.time - startTime;

        float t = (Mathf.Sin(elapsedTime * speed - Mathf.PI / 2f) + 1f) / 2f;

        // Move upward from the starting position without going below it
        float newY = startPos.y + t * moveDistance;

        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}

