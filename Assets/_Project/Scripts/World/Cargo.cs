using UnityEngine;

public class Cargo : MonoBehaviour
{
    // what kind of cargo this is: points and weight live in a CargoType asset (Data/CT_Light, Data/CT_Heavy)
    [SerializeField] private CargoType type;

    public CargoType Type()
    {
        return type;
    }

    // no type set yet: count it as a light cargo
    public int Points()
    {
        return type != null ? type.Points() : 1;
    }

    public float Weight()
    {
        return type != null ? type.Weight() : 0f;
    }
}
