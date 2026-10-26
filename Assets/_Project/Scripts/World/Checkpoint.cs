using UnityEngine;

// A trigger box standing on a green platform (a "sanctuary").
// The bottom of the box is where the player respawns; a carried crystal is left here and counted.
[RequireComponent(typeof(BoxCollider))]
public class Checkpoint : MonoBehaviour
{
    private BoxCollider box;
    private int deposited;

    void Awake()
    {
        box = GetComponent<BoxCollider>();
        box.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        PlayerRespawn respawn = other.GetComponentInParent<PlayerRespawn>();
        if (respawn == null) return;

        Bounds b = box.bounds;
        respawn.SetCheckpoint(new Vector3(b.center.x, b.min.y, b.center.z));
    }

    // Deposit while the player stands here, but not in the first moment after a respawn,
    // so the death message is seen and a crystal picked up inside the sanctuary still counts
    void OnTriggerStay(Collider other)
    {
        PlayerRespawn respawn = other.GetComponentInParent<PlayerRespawn>();
        if (respawn == null || respawn.JustRespawned) return;

        PlayerCarry carry = other.GetComponentInParent<PlayerCarry>();
        if (carry != null && carry.IsCarrying()) Deposit(carry.HandOver());
    }

    void Deposit(Crystal crystal)
    {
        if (crystal == null) return;

        // Leave it glowing in a corner of the platform; PickUp turns its colliders off so it stays put
        Bounds b = box.bounds;
        var spot = new GameObject("DepositedCrystal").transform;
        spot.SetParent(transform, false);
        spot.position = new Vector3(b.min.x + 0.5f + 0.8f * deposited, b.min.y + 0.45f, b.max.z - 0.5f);
        crystal.PickUp(spot);
        deposited++;

        GameEvents.RaiseCrystalDeposited();
    }
}
