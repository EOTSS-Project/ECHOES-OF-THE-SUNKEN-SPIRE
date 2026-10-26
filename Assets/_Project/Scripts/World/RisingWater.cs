using System.Collections.Generic;
using UnityEngine;

// The water starts under the floor, rises once the player starts climbing and drowns whoever it touches.
// A dropped crystal that falls in goes back to where it was found, unless that spot is already under water.
[RequireComponent(typeof(Rigidbody))]
public class RisingWater : MonoBehaviour
{
    [Header("Rising")]
    [SerializeField] private float riseSpeed = 0.2f;              // metres per second
    [SerializeField] private float catchUpSpeed = 1f;             // when the player is far above
    [SerializeField] private float maxDistanceBelowPlayer = 10f;

    [Header("Start and Respawn")]
    [SerializeField] private float startDelay = 6f;               // starts after this many seconds...
    [SerializeField] private float startWhenPlayerAbove = 3.5f;   // ...or once the player's feet pass this height (the first ledges)
    [SerializeField] private float pauseAfterRespawn = 3f;
    [SerializeField] private float resetBelowCheckpoint = 6f;

    [Header("Final Rush")]
    [SerializeField] private float finalRushHeight = 30f;         // once the player's feet pass this height...
    [SerializeField] private float finalRushSpeed = 0.45f;        // ...the water rises this fast

    [SerializeField] private PlayerRespawn player;

    private float startTop;
    private float pauseUntil;
    private bool rising;
    private bool rushing;
    private bool stopped;
    private readonly Dictionary<Crystal, Vector3> crystalHomes = new Dictionary<Crystal, Vector3>();

    public float TopY => transform.position.y + transform.lossyScale.y * 0.5f;
    public bool IsRising => rising && !stopped && Time.time >= pauseUntil;

    void Start()
    {
        startTop = TopY;
        pauseUntil = Time.time + startDelay;
        GetComponent<Rigidbody>().isKinematic = true; // a moving trigger needs a kinematic body to report reliably
        foreach (var c in FindObjectsByType<Crystal>(FindObjectsSortMode.None)) crystalHomes[c] = c.transform.position;
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
            if (Time.time < pauseUntil && feet < startWhenPlayerAbove) return;
            rising = true;
            pauseUntil = 0f;
        }
        if (Time.time < pauseUntil) return;

        if (!rushing && feet >= finalRushHeight)
        {
            rushing = true;
            GameEvents.RaiseFinalRush();
        }

        // Rise steadily, but never let the player get too far ahead
        float baseSpeed = rushing ? finalRushSpeed : riseSpeed;
        float speed = feet - TopY > maxDistanceBelowPlayer ? catchUpSpeed : baseSpeed;
        SetTop(TopY + speed * Time.deltaTime);

        if (feet < TopY - 0.05f) player.Die("drowned");
    }

    void OnTriggerEnter(Collider other)
    {
        PlayerRespawn respawn = other.GetComponentInParent<PlayerRespawn>();
        if (respawn != null)
        {
            respawn.Die("drowned");
            return;
        }

        // A loose crystal (not carried, not left at a sanctuary) fell in
        Crystal crystal = other.GetComponentInParent<Crystal>();
        if (crystal == null || crystal.transform.parent != null && crystal.transform.parent.name != "Collectibles") return;
        if (crystalHomes.TryGetValue(crystal, out Vector3 home) && home.y > TopY + 0.5f)
        {
            Rigidbody crb = crystal.GetComponent<Rigidbody>();
            if (crb != null) crb.linearVelocity = Vector3.zero;
            crystal.transform.position = home;
        }
        else
        {
            Destroy(crystal.gameObject);
            GameEvents.RaiseCrystalLost();
        }
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
