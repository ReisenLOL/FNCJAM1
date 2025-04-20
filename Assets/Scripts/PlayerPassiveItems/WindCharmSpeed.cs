using UnityEngine;

public class WindCharmSpeed : Passive
{
    public PlayerController player;
    private void Start()
    {
        LevelUp();
        player = GameObject.Find("Player").GetComponentInChildren<PlayerController>();
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
        player.speedModifier = modifierValue;
    }
}
