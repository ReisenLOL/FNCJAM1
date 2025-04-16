using UnityEngine;

public class Infusion : Passive
{
    public PlayerUnit player;
    public float regenTime;
    private void Start()
    {
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
        player.MaxHealth = modifierValue;
    }
}
