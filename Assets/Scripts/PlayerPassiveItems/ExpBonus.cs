using UnityEngine;

public class ExpBonus : Passive
{
    public PlayerLevelManager levelManager;
    private void Start()
    {
        SetModifierValues();
        levelManager = GameObject.Find("PlayerLevelManager").GetComponentInChildren<PlayerLevelManager>();
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
        levelManager.powerModifier = modifierValue;
    }
}
