
using UnityEngine;

public class DeliveryZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerCarry carry = other.GetComponent<PlayerCarry>();

        if (carry == null) return;
        if (!carry.IsCarrying()) return;

        Crystal crystal = carry.HandOver();

        if (crystal == null) return;

        Destroy(crystal.gameObject);

        Debug.Log("Delivered!");
    }
}

