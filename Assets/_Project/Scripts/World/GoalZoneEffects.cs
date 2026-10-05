using UnityEngine;

public class GoalZoneEffects : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 20f;

    [Header("Pulse Settings")]
    [SerializeField] private float pulseSpeed = 2.5f;
    [SerializeField] private float pulseAmount = 0.15f;

    private Vector3 initialScale;

    void Start()
    {
        initialScale = transform.localScale;
    }

    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);

        float scaleOffset = Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
        transform.localScale = initialScale + new Vector3(scaleOffset, scaleOffset, scaleOffset);
    }
}
