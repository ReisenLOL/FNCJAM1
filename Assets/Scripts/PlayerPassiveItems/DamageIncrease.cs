using UnityEngine;

public class DamageIncrease : Passive
{
    public WeaponAttack[] weapons;
    private void Start()
    {
        ApplyModifierToWeapons();
    }
    private void Update()
    {
        if (refreshWeaponList)
        {
            ApplyModifierToWeapons();
            refreshWeaponList = false;
        }
    }
    public void ApplyModifierToWeapons()
    {
        weapons = FindObjectsByType<WeaponAttack>(FindObjectsSortMode.None);
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].damageModifier = modifierValue;
        }
    }
}
