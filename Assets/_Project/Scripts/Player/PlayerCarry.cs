using UnityEngine;

public class PlayerCarry : MonoBehaviour
{
    [SerializeField] private Transform carryPoint;
    public Transform CarryPoint => carryPoint;

    private Crystal carried;
    public Crystal CarriedCrystal => carried; // GameHUD'un kargo rengini bilmesi için eklendi

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
    }

    public void DropCargo()
    {
        if (!IsCarrying()) return;

        carried.Drop();
        carried = null;
        rb.mass = baseMass;
    }

    public void ResetCarriedToSpawn()
    {
        if (!IsCarrying()) return;

        carried.ResetToSpawn();
        carried = null;
        rb.mass = baseMass;
    }

    public Crystal HandOver()
    {
        if (!IsCarrying()) return null;

        Crystal crystal = carried;
        carried = null;
        rb.mass = baseMass;

        // Teslim edilen kargoyu türünü ve puanını GameHUD'a bildir
        if (GameHUD.Instance != null)
        {
            bool isHeavy = crystal.Weight() > 1f || (crystal.Type != null && crystal.Type.name.ToLower().Contains("heavy"));
            GameHUD.Instance.RegisterDeposit(isHeavy, crystal.Points());
        }

        return crystal;
    }

    public Crystal Handover()
    {
        return HandOver();
    }
}