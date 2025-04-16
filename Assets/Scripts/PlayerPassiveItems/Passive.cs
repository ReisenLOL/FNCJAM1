using Core.Extensions;
using UnityEngine;

public class Passive : MonoBehaviour
{
    [field: SerializeField] public string WeaponName { get; private set; } = "Headhunter, Leather Belt";
    public BaseUnit Owner;
    public float modifierValue;
    public int level;
    public PassiveItemLevelData[] passiveLevels;
    public bool refreshWeaponList = false;
    public void LevelUp()
    {
        if (level >= passiveLevels.Length - 1) { return; }
        level++;
        modifierValue = passiveLevels[level.Clamp(0, passiveLevels.Length)].modifierValue;
    }
    public void SetOwner(BaseUnit owner)
    {
        Owner = owner;
    }
}
