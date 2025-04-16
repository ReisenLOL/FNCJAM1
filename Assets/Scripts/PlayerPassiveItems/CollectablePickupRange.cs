using UnityEngine;

public class CollectablePickupRange : Passive
{
    public PlayerController player;
    private void Start()
    {
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
        player.collectionRadiusMultiplier = modifierValue;
    }
}
