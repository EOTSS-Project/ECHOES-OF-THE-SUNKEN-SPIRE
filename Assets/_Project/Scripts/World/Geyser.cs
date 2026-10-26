using UnityEngine;

// A water jet in the floor: stepping on the pad launches the player straight up.
// The trigger sits on top of the pad; the bubbles (children of "bubbles") rise and loop for the look.
public class Geyser : MonoBehaviour
{
    [SerializeField] private float launchSpeed = 10.5f;   // m/s upwards, about 5.6 m of height
    [SerializeField] private Transform bubbles;
    [SerializeField] private float bubbleHeight = 5f;
    [SerializeField] private float bubbleSpeed = 3f;

    private float readyAt;

    void OnTriggerEnter(Collider other) { Launch(other); }
    void OnTriggerStay(Collider other) { Launch(other); }

    void Launch(Collider other)
    {
        if (Time.time < readyAt) return;
        Rigidbody rb = other.attachedRigidbody;
        if (rb == null || other.GetComponentInParent<PlayerRespawn>() == null) return;
        if (rb.linearVelocity.y > 1f) return; // already flying up

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, launchSpeed, rb.linearVelocity.z);
        readyAt = Time.time + 0.4f;
    }

    void Update()
    {
        if (bubbles == null) return;
        for (int i = 0; i < bubbles.childCount; i++)
        {
            Transform b = bubbles.GetChild(i);
            Vector3 p = b.localPosition;
            p.y = Mathf.Repeat(p.y + bubbleSpeed * Time.deltaTime, bubbleHeight);
            b.localPosition = p;
        }
    }
}
