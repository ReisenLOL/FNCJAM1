using Core.Extensions;
using UnityEngine;

public class Passive : Item
{
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
}
