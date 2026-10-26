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

        PlayerCarry carry = other.GetComponentInParent<PlayerCarry>();
        if (carry != null && carry.IsCarrying()) Deposit(carry.HandOver(), b);
    }

    void Deposit(Crystal crystal, Bounds b)
    {
        if (crystal == null) return;

        // Leave it glowing in a corner of the platform; PickUp turns its colliders off so it stays put
        var spot = new GameObject("DepositedCrystal").transform;
        spot.SetParent(transform, false);
        spot.position = new Vector3(b.min.x + 0.5f + 0.8f * deposited, b.min.y + 0.45f, b.max.z - 0.5f);
        crystal.PickUp(spot);
        deposited++;

        GameEvents.RaiseCrystalDeposited();
    }
}
