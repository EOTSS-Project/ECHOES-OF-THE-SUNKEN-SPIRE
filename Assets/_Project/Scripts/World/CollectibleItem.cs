using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotateSpeed = 90f;

    [Header("Floating Settings")]
    public float floatSpeed = 2f;
    public float floatAmount = 0.25f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Kendi etrafında dönme
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime, Space.World);

        // Sinüs dalgası ile süzülme
        float newY = startPos.y + (Mathf.Sin(Time.time * floatSpeed) * floatAmount);
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}