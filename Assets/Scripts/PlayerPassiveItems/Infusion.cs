using UnityEngine;

public class Infusion : Passive
{
    public PlayerUnit player;
    private void Start()
    {
        LevelUp();
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
        player.MaxHealth = modifierValue;
    }
}
