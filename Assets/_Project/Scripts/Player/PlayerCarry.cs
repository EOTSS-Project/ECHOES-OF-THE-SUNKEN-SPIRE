
using UnityEngine;

public class PlayerCarry : MonoBehaviour
{
    [SerializeField] private Transform carryPoint;

    public Transform CarryPoint => carryPoint;

    private Crystal carried;

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

        Debug.Log("Collected!");
    }

    public void DropCargo()
    {
        if (!IsCarrying()) return;

        carried.Drop();
        carried = null;

        Debug.Log("Knocked the cargo off!");
    }

    public Crystal HandOver()
    {
        if (!IsCarrying()) return null;

        Crystal crystal = carried;
        carried = null;

        return crystal;
    }
}
