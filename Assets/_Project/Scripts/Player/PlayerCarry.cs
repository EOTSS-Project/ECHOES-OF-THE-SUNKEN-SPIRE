using UnityEngine;

public class PlayerCarry : MonoBehaviour
{
    [SerializeField] private Transform carryPoint;
    public Transform CarryPoint => carryPoint;

    private Crystal carried;
    private Rigidbody rb;
    private float baseMass;

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

        // 6. Hafta 5. Kişi Görevi: Ağır kristal kütleyi artırır, zıplama yüksekliğini düşürür
        rb.mass = baseMass + crystal.Weight();

        Debug.Log($"Collected Crystal! Puan: {crystal.Points()}, Ağırlık: {crystal.Weight()}, Kütle: {rb.mass}");
    }

    public void DropCargo()
    {
        if (!IsCarrying()) return;

        carried.Drop();
        carried = null;

        // Kristal bırakılınca kütle eski haline döner
        rb.mass = baseMass;

        Debug.Log("Knocked the cargo off!");
    }

    // Hem DeliveryZone hem Checkpoint HandOver() çağırdığı için burası çalışır:
    public Crystal HandOver()
    {
        if (!IsCarrying()) return null;

        Crystal crystal = carried;
        carried = null;

        // Kristal teslim edilince kütle eski haline döner
        rb.mass = baseMass;

        return crystal;
    }

    // Küçük harfle çağıran olası durumlar için yönlendirme
    public Crystal Handover()
    {
        return HandOver();
    }
}