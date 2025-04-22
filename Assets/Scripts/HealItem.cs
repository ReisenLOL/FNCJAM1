using UnityEngine;

public class HealItem : Item
{
    public PlayerUnit player;
    void Start()
    {
        player = GameObject.Find("Player").GetComponentInChildren<PlayerUnit>();
        player.ChangeHealth(player.MaxHealth - player.CurrentHealth);
    }
    private void Update()
    {
        if (refreshWeaponList)
        {
            player.ChangeHealth(player.MaxHealth - player.CurrentHealth);
        }
    }
}
