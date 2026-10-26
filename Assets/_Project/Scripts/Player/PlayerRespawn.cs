using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private float safeTimeAfterRespawn = 1f; // seconds in which nothing can kill us again

    private Rigidbody rb;
    private float halfHeight;
    private Vector3 checkpointFeet;
    private float safeUntil;
    private bool won;

    public Vector3 CheckpointFeet => checkpointFeet;
    public float FeetY => transform.position.y - halfHeight;
    public bool JustRespawned => Time.time < safeUntil;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        CapsuleCollider capsule = GetComponent<CapsuleCollider>();
        halfHeight = capsule != null ? capsule.height * 0.5f * transform.lossyScale.y : 1f;
        checkpointFeet = transform.position - Vector3.up * halfHeight; // the spawn is the first checkpoint
    }

    void OnEnable() { GameEvents.Won += OnWon; }
    void OnDisable() { GameEvents.Won -= OnWon; }

    // Only a higher checkpoint replaces the current one
    public void SetCheckpoint(Vector3 feet)
    {
        if (feet.y <= checkpointFeet.y + 0.5f) return;
        checkpointFeet = feet;
        GameEvents.RaiseCheckpointReached();
    }

    public void Die(string reason)
    {
        if (won || Time.time < safeUntil) return;

        GameEvents.RaisePlayerDied(reason);

        Vector3 pos = checkpointFeet + Vector3.up * (halfHeight + 0.05f);
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.position = pos;
        transform.position = pos;
        safeUntil = Time.time + safeTimeAfterRespawn;

        GameEvents.RaisePlayerRespawned(checkpointFeet);
    }

    // The climb is over: hold the player where they are, even mid-jump at the summit edge
    void OnWon()
    {
        won = true;
        foreach (var name in new[] { "PlayerMove", "PlayerJump", "PlayerDrop" })
        {
            var b = GetComponent(name) as Behaviour;
            if (b != null) b.enabled = false;
        }
        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true;
    }
}
