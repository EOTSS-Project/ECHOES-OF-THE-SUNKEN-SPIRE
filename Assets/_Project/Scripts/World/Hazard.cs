using UnityEngine;

public class Hazard : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Crystal"))
        {
            Debug.Log("Hazard touched! Dropping cargo/crystal.");
        }

        // Red burns: the player goes back to the last checkpoint
        PlayerRespawn respawn = other.GetComponentInParent<PlayerRespawn>();
        if (respawn != null) respawn.Die("burned");
    }
}