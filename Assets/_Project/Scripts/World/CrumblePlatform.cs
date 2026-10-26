using UnityEngine;

// An old ledge: it shakes when the player lands on it, falls away, then grows back.
public class CrumblePlatform : MonoBehaviour
{
    [SerializeField] private float shakeTime = 1f;
    [SerializeField] private float shakeAmount = 0.06f;
    [SerializeField] private float goneTime = 3f;

    private Vector3 home;
    private Collider col;
    private Renderer rend;
    private float steppedAt = -1f;
    private float fellAt = -1f;

    public bool IsSolid => col.enabled;

    void Awake()
    {
        home = transform.position;
        col = GetComponent<Collider>();
        rend = GetComponent<Renderer>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (steppedAt >= 0f || fellAt >= 0f) return;
        if (collision.collider.GetComponentInParent<PlayerRespawn>() != null) steppedAt = Time.time;
    }

    void Update()
    {
        if (steppedAt >= 0f)
        {
            float e = Time.time - steppedAt;
            float s = shakeAmount * Mathf.Clamp01(e / shakeTime);
            transform.position = home + new Vector3(Mathf.Sin(e * 70f) * s, 0f, Mathf.Cos(e * 55f) * s);
            if (e >= shakeTime)
            {
                steppedAt = -1f;
                fellAt = Time.time;
                col.enabled = false;
            }
        }
        else if (fellAt >= 0f)
        {
            float e = Time.time - fellAt;
            transform.position = home + Vector3.down * (4.9f * e * e);
            rend.enabled = e < 1f;
            if (e >= goneTime)
            {
                fellAt = -1f;
                transform.position = home;
                col.enabled = true;
                rend.enabled = true;
            }
        }
    }
}
