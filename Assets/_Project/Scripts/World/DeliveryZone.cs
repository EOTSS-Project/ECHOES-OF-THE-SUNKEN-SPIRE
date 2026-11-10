using UnityEngine;
using UnityEngine.SceneManagement;

public class DeliveryZone : MonoBehaviour
{
    private int score;

    void OnTriggerEnter(Collider other)
    {
        PlayerCarry carry = other.GetComponent<PlayerCarry>();
        if (carry == null) return;

        if (!carry.IsCarrying()) return;

        Crystal crystal = carry.HandOver();
        score += crystal.Points();
        Debug.Log("Delivered! +" + crystal.Points() + " Score: " + score);
        Destroy(crystal.gameObject);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}