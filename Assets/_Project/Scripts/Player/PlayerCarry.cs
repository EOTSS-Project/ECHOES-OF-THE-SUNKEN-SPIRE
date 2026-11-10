using UnityEngine;

public class PlayerCarry : MonoBehaviour
{
    [SerializeField] private Transform carryPoint;
    public Transform CarryPoint => carryPoint;

    private Crystal carried;
    private Rigidbody rb;
    private float baseMass;

    public float CurrentSpeedMultiplier => (carried != null && carried.Type != null) ? carried.Type.SpeedMultiplier : 1f;
    public float CurrentJumpMultiplier => (carried != null && carried.Type != null) ? carried.Type.JumpMultiplier : 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        baseMass = rb.mass;
    }

    public bool IsCarrying()
    {
        return carried != null;
    }

    void OnTriggerEnter(Collider other)
    {
        Crystal crystal = other.GetComponent<Crystal>();
        if (crystal == null) return;
        if (IsCarrying()) return;
        if (!crystal.CanBePickedUp()) return;

        carried = crystal;
        crystal.PickUp(carryPoint);

        rb.mass = baseMass + crystal.Weight();

        Debug.Log($"Collected Crystal! Puan: {crystal.Points()}, Ağırlık: {crystal.Weight()}, Kütle: {rb.mass}");
    }

    public void DropCargo()
    {
        if (!IsCarrying()) return;

        carried.Drop();
        carried = null;

        rb.mass = baseMass;

        Debug.Log("Knocked the cargo off!");
    }

    public void ResetCarriedToSpawn()
    {
        if (!IsCarrying()) return;

        carried.ResetToSpawn();
        carried = null;

        rb.mass = baseMass;

        Debug.Log("Carried crystal reset to its spawn point on death/respawn.");
    }

    public Crystal HandOver()
    {
        if (!IsCarrying()) return null;

        Crystal crystal = carried;
        carried = null;

        rb.mass = baseMass;

        return crystal;
    }

    public Crystal Handover()
    {
        return HandOver();
    }
}