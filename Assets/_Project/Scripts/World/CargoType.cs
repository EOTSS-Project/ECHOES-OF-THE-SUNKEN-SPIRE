using UnityEngine;

[CreateAssetMenu(menuName = "Cube Courier/Cargo Type")]
public class CargoType : ScriptableObject
{
    [SerializeField] private int points = 1;
    [SerializeField] private float weight = 0f;

    public int Points()
    {
        return points;
    }

    public float Weight()
    {
        return weight;
    }
}
