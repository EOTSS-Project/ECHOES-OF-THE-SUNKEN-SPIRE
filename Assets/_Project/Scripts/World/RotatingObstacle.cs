using UnityEngine;

public class RotatingObstacle : MonoBehaviour
{
    [Header("Obstacle Spin Settings")]
    [SerializeField] private float spinSpeed = 60f;
    [SerializeField] private Vector3 rotationAxis = Vector3.up;

    void Update()
    {
        transform.Rotate(rotationAxis * spinSpeed * Time.deltaTime);
    }
}
