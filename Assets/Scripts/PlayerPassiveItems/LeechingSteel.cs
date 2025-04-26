using UnityEngine;

public class LeechingSteel : Passive
{
    public PlayerUnit player;
    public float regenTime;
    private void Start()
    {
        SetModifierValues();
        player = GameObject.Find("Player").GetComponentInChildren<PlayerUnit>();
    }
    private void Update()
    {
        if (refreshWeaponList)
        {
            WeaponAttack[] weaponList = FindObjectsByType<WeaponAttack>(FindObjectsSortMode.None);
            foreach (WeaponAttack weapon in weaponList)
            {
                weapon.hasLeeching = true;
                weapon.leechingItem = this;
            }
            refreshWeaponList = false;
        }
    }
    public void ApplyModifierToPlayer()
    {
        if (player.CurrentHealth < player.MaxHealth)
        {
            player.ChangeHealth(modifierValue);
        }
    }
}
