using UnityEngine;

public class Crystal: MonoBehaviour
{
    private Rigidbody rb;
    private Collider box;
    private Collider range;

    private float canPickUpAt; // Yeniden toplanma gecikmesi için zaman tutucu

    void Awake()
    {
        // Kendi bileşenlerimizi başlangıçta cache'liyoruz (hafızada tutuyoruz)
        rb = GetComponent<Rigidbody>();
        box = GetComponent<BoxCollider>();
        range = GetComponent<SphereCollider>();
    }

    // "Şu an toplanabilir miyim?" sorusunun yanıtı
    public bool CanBePickedUp()
    {
        return Time.time >= canPickUpAt;
    }

    // Alındığında çalışacak metod (Kinematik ve çarpışmalar kapanır, oyuncuya bağlanır)
    public void PickUp(Transform carryPoint)
    {
        rb.isKinematic = true;          // Fizik motoru dondurulur, yerçekimi etkilemez
        box.enabled = false;            // Katı çarpışma kapatılır (oyuncuyu ittirmez)
        range.enabled = false;          // Toplama alanı kapatılır (tekrar üst üste tetiklenmez)

        // Oyuncunun başındaki CarryPoint'in çocuğu olur
        transform.SetParent(carryPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    // Yere bırakıldığında çalışacak metod
    public void Drop()
    {
        transform.SetParent(null);      // Ebeveyn-çocuk bağı koparılır
        box.enabled = true;             // Çarpışmalar tekrar açılır
        range.enabled = true;           // Toplama menzili aktifleşir
        rb.isKinematic = false;         // Fizik motoru ve yerçekimi geri verilir

        // Oyuncunun önüne doğru hafifçe yukarı ve ileri fırlatılır
        Vector3 push = transform.forward + Vector3.up;
        rb.AddForce(push * 3f, ForceMode.Impulse);

        // Oyuncu bırakır bırakmaz anında tekrar eline almasın diye 0.5 saniye kısıtlama konur
        canPickUpAt = Time.time + 0.5f;
    }
}