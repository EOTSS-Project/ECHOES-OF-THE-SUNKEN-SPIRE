using UnityEngine;

[CreateAssetMenu(fileName = "NewCargoType", menuName = "Game/Cargo Type")]
public class CargoType : ScriptableObject
{
    [SerializeField] private string typeName;
    [SerializeField] private float weight;
    [SerializeField] private float speedMultiplier = 1f;
    [SerializeField] private float jumpMultiplier = 1f;

    public string TypeName => typeName;
    public float Weight() => weight;
    public float SpeedMultiplier => speedMultiplier;
    public float JumpMultiplier => jumpMultiplier;
}