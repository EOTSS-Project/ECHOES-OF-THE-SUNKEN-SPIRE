
using UnityEngine;

public class DeliveryZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerCarry carry = other.GetComponent<PlayerCarry>();

        if (carry == null) return;

        // The exit is the last sanctuary: a carried crystal counts, and reaching it ends the climb
        Crystal crystal = carry.HandOver();
        if (crystal != null)
        {
            Destroy(crystal.gameObject);
            Debug.Log("Delivered!");
            GameEvents.RaiseCrystalDeposited();
        }

        GameEvents.RaiseWon();
    }
}

