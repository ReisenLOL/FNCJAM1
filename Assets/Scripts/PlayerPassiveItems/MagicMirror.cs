using UnityEngine;

public class MagicMirror : Passive
{
    public WeaponAttack[] weapons;
    private void Start()
    {
        SetModifierValues();
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
            if (weapons[i].canDuplicateProjectiles)
            {
                weapons[i].attackCountModifier = modifierValue;
            }
        }
    }
}
