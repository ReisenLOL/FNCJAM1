using Core.Extensions;
using UnityEngine;

public class Passive : Item
{
    public bool isOnPassiveList = true;
    public float modifierValue;
    public float specialValueA;
    public int level;
    public PassiveItemLevelData[] passiveLevels;
    public void LevelUp()
    {
        if (level >= passiveLevels.Length - 1) { return; }
        level++;
        SetModifierValues();
    }
    public void SetModifierValues()
    {
        modifierValue = passiveLevels[level.Clamp(0, passiveLevels.Length)].modifierValue;
        specialValueA = passiveLevels[level.Clamp(0, passiveLevels.Length)].specialValueA;
    }
}
