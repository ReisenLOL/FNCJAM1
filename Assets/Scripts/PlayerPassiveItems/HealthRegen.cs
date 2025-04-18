using UnityEngine;

public class HealthRegen : Passive
{
    public PlayerUnit player;
    public float regenTime;
    private void Start()
    {
        LevelUp();
        player = GameObject.Find("Player").GetComponentInChildren<PlayerUnit>();
        ApplyModifierToPlayer();
    }
    private void Update()
    {
        regenTime += Time.deltaTime;
        if (regenTime > specialValueA)
        {
            ApplyModifierToPlayer();
            regenTime -= specialValueA;
        }
    }
    public void ApplyModifierToPlayer()
    {
        player.ChangeHealth(modifierValue);
    }
}
