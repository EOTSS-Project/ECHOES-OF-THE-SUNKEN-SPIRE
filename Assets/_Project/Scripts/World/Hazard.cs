using UnityEngine;

public class Hazard : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Crystal"))
        {
            Debug.Log("Hazard touched! Dropping cargo/crystal.");
        }
    }
}