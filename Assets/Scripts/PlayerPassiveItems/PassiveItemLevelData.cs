using UnityEngine;
[CreateAssetMenu(fileName = "New Weapon Level Data", menuName = "Scripts/PassiveLevelData")]

public class PassiveItemLevelData : ScriptableObject
{
    public float modifierValue;
    [Tooltip("Optional")]
    public float specialValueA;
}
