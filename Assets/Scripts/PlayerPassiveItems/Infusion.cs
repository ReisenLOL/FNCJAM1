using UnityEngine;

public class Infusion : Passive
{
    public PlayerUnit player;
    private void Start()
    {
        SetModifierValues();
        player = GameObject.Find("Player").GetComponentInChildren<PlayerUnit>();
        ApplyModifierToPlayer();
    }
    private void Update()
    {
        if (refreshWeaponList)
        {
            ApplyModifierToPlayer();
            refreshWeaponList = false;
        }
    }
    public void ApplyModifierToPlayer()
    {
        float pastMaxHealth = player.MaxHealth;
        player.MaxHealth = modifierValue;
        player.ChangeHealth(modifierValue - pastMaxHealth);
    }
}
