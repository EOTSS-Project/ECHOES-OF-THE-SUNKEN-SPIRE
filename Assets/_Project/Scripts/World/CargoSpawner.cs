using UnityEngine;

public class CargoSpawner : MonoBehaviour
{
    [SerializeField] private GameObject cargoPrefab;
    [SerializeField] private float delay = 4f;

    private float timer = 0f;
    private GameObject currentCargo;

    void Update()
    {
        // Mevcut kargo hala sahnede varsa (yok edilmediyse) bekle, yenisini üretme
        if (currentCargo != null)
        {
            return;
        }

        timer -= Time.deltaTime;
        if (timer > 0f) return;

        if (cargoPrefab == null) return;

        Vector3 spawnPos = transform.position + Vector3.up * 1.0f;
        currentCargo = Instantiate(cargoPrefab, spawnPos, Quaternion.identity);

        timer = delay;
    }
}