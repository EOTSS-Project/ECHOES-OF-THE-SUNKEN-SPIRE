using UnityEngine;

public class FloatingLantern : MonoBehaviour
{
    [Header("Hover Settings")]
    public float hoverSpeed = 3f;
    public float hoverHeight = 0.15f;

    [Header("Rotation Settings")]
    public float spinSpeed = 30f;

    private Vector3 localStartPos;

    void Start()
    {
        localStartPos = transform.localPosition;
    }

    void Update()
    {
        float newY = localStartPos.y + (Mathf.Sin(Time.time * hoverSpeed) * hoverHeight);
        transform.localPosition = new Vector3(localStartPos.x, newY, localStartPos.z);

        transform.Rotate(Vector3.up * spinSpeed * Time.deltaTime);
    }
}
