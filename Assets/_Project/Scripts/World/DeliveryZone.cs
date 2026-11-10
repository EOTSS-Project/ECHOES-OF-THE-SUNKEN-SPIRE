using UnityEngine;

public class DeliveryZone : MonoBehaviour
{
    private int score; // Toplam toplanan puan

    void OnTriggerEnter(Collider other)
    {
        // Oyuncu kristal taşıyor mu kontrol et
        PlayerCarry carry = other.GetComponent<PlayerCarry>();
        if (carry == null) return;

        if (!carry.IsCarrying()) return;

        // Kristali teslim al, puanı ekle ve kristali yok et
        Crystal crystal = carry.HandOver();
        score += crystal.Points();
        Debug.Log("Delivered! +" + crystal.Points() + " Score: " + score);
        Destroy(crystal.gameObject);
    }
}
