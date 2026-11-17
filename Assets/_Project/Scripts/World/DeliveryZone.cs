using UnityEngine;

public class DeliveryZone : MonoBehaviour
{
    [Tooltip("Eğer bu platform oyunun en sonundaki BİTİŞ platformu ise bu kutucuğu işaretleyin")]
    public bool isFinishPlatform = false;

    void OnTriggerEnter(Collider other)
    {
        PlayerCarry carry = other.GetComponent<PlayerCarry>();
        if (carry == null) return; // Oyuncu değilse işlem yapma

        // EĞER BU BİTİŞ PLATFORMUYSA:
        if (isFinishPlatform)
        {
            // Elinde kargoyla atladıysa kargoyu teslim al
            if (carry.IsCarrying())
            {
                Crystal crystal = carry.HandOver();
                Destroy(crystal.gameObject);
            }

            // Oyunu anında yeniden başlatmak YERİNE, Kazanma ekranını çağır
            if (GameHUD.Instance != null)
            {
                GameHUD.Instance.TriggerWin();
            }
            return;
        }

        // Bitiş platformu DEĞİLSE ve elinde kargo yoksa hiçbir şey yapma
        if (!carry.IsCarrying()) return;

        // Normal yeşil alan teslimatı
        Crystal normalCrystal = carry.HandOver();
        Destroy(normalCrystal.gameObject);
    }
}