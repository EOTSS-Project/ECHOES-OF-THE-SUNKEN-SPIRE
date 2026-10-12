using UnityEngine;

public class DeliveryZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Crystal delivered successfully!");
        }
    }
}
