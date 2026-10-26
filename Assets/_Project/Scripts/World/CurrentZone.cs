using UnityEngine;

// An underwater current: while the player is inside, it drifts them along "flow" whatever they press.
// The streaks (children of "streaks") slide along the flow and loop, so the current can be seen.
[RequireComponent(typeof(BoxCollider))]
public class CurrentZone : MonoBehaviour
{
    [SerializeField] private Vector3 flow = new Vector3(0f, 0f, -1.6f); // world m/s
    [SerializeField] private Transform streaks;

    private BoxCollider box;
    private Rigidbody player;
    private int playerColliders; // the player has several colliders (body, lantern, nose)

    void Awake()
    {
        box = GetComponent<BoxCollider>();
        box.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<PlayerRespawn>() == null) return;
        player = other.attachedRigidbody;
        playerColliders++;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<PlayerRespawn>() == null) return;
        playerColliders = Mathf.Max(0, playerColliders - 1);
    }

    void FixedUpdate()
    {
        if (playerColliders > 0 && player != null)
            player.position += flow * Time.fixedDeltaTime;
    }

    void Update()
    {
        if (streaks == null) return;
        Bounds b = box.bounds;
        Vector3 dir = flow.normalized;
        float length = Mathf.Abs(Vector3.Dot(b.size, new Vector3(Mathf.Abs(dir.x), Mathf.Abs(dir.y), Mathf.Abs(dir.z))));
        for (int i = 0; i < streaks.childCount; i++)
        {
            Transform s = streaks.GetChild(i);
            Vector3 p = s.position + flow * Time.deltaTime;
            // Wrap around once a streak leaves the zone on the downstream side
            if (Vector3.Dot(p - b.center, dir) > length * 0.5f) p -= dir * length;
            s.position = p;
        }
    }
}
