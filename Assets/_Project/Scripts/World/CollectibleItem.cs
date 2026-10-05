using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private float rotateSpeed = 90f;

    [Header("Floating Settings")]
    [SerializeField] private float floatSpeed = 2f;
    [SerializeField] private float floatAmount = 0.25f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Y ekseninde sürekli dönme hareketi
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime, Space.World);

        // Sinüs dalgası ile yukarı-aşağı süzülme
        float newY = startPos.y + (Mathf.Sin(Time.time * floatSpeed) * floatAmount);
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}