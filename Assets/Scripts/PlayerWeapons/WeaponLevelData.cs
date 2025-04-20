using UnityEngine;
[CreateAssetMenu(fileName = "New Weapon Level Data", menuName = "Scripts/WeaponLevelData")]

public class WeaponLevelData : ScriptableObject
{
    public string upgradeDescription;
    public float movementSpeed;
    public float attackCount;
    public float damage;
    public float attackRate;
    public float range;
    public float dissipationDelay;
    public float specialPropertyA;
    public float specialPropertyB;
    public float specialPropertyC;
}
