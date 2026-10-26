using UnityEngine;

// The water starts under the floor, rises once the player starts climbing and drowns whoever it touches.
public class RisingWater : MonoBehaviour
{
    [Header("Rising")]
    [SerializeField] private float riseSpeed = 0.2f;              // metres per second
    [SerializeField] private float catchUpSpeed = 1f;             // when the player is far above
    [SerializeField] private float maxDistanceBelowPlayer = 10f;

    [Header("Start and Respawn")]
    [SerializeField] private float startDelay = 6f;               // starts after this many seconds...
    [SerializeField] private float startWhenPlayerClimbs = 1.5f;  // ...or once the player's feet are this high above the water
    [SerializeField] private float pauseAfterRespawn = 3f;
    [SerializeField] private float resetBelowCheckpoint = 6f;

    [SerializeField] private PlayerRespawn player;

    private float startTop;
    private float pauseUntil;
    private bool rising;
    private bool stopped;

    public float TopY => transform.position.y + transform.lossyScale.y * 0.5f;

    void Start()
    {
        startTop = TopY;
        pauseUntil = Time.time + startDelay;
    }

    void OnEnable()
    {
        GameEvents.PlayerRespawned += OnRespawned;
        GameEvents.Won += OnWon;
    }

    void OnDisable()
    {
        GameEvents.PlayerRespawned -= OnRespawned;
        GameEvents.Won -= OnWon;
    }

    void Update()
    {
        if (stopped || player == null) return;

        float feet = player.FeetY;
        if (!rising)
        {
            if (Time.time < pauseUntil && feet < startTop + startWhenPlayerClimbs) return;
            rising = true;
        }
        if (Time.time < pauseUntil) return;

        // Rise slowly, but never let the player get too far ahead
        float speed = feet - TopY > maxDistanceBelowPlayer ? catchUpSpeed : riseSpeed;
        SetTop(TopY + speed * Time.deltaTime);

        if (feet < TopY - 0.05f) player.Die("drowned");
    }

    void OnTriggerEnter(Collider other)
    {
        PlayerRespawn respawn = other.GetComponentInParent<PlayerRespawn>();
        if (respawn != null) respawn.Die("drowned");
    }

    void OnRespawned(Vector3 checkpointFeet)
    {
        float target = Mathf.Max(startTop, checkpointFeet.y - resetBelowCheckpoint);
        if (TopY > target) SetTop(target);

        if (target <= startTop + 0.01f)
        {
            // Back at the start: same grace as the first try
            rising = false;
            pauseUntil = Time.time + startDelay;
        }
        else pauseUntil = Time.time + pauseAfterRespawn;
    }

    void OnWon() { stopped = true; }

    void SetTop(float y)
    {
        Vector3 p = transform.position;
        p.y = y - transform.lossyScale.y * 0.5f;
        transform.position = p;
    }
}
