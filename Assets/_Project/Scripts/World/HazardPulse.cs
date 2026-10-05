using UnityEngine;

public class HazardPulse : MonoBehaviour
{
    [Header("Pulse Settings")]
    [SerializeField] private float pulseSpeed = 4f;
    [SerializeField] private float scaleAmount = 0.2f;

    private Vector3 defaultScale;

    void Start()
    {
        defaultScale = transform.localScale;
    }

    void Update()
    {
        float scaleOffset = Mathf.Sin(Time.time * pulseSpeed) * scaleAmount;
        transform.localScale = defaultScale + new Vector3(scaleOffset, 0f, scaleOffset);
    }
}
