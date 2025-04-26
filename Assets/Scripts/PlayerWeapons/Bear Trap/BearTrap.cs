using UnityEngine;

public class BearTrap : Weapon
{
    public int maxHits;
    public int hitAmount;
    private void Start()
    {
        SetWeaponProperties();
        maxHits = (int)weaponLevelData.specialPropertyA;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        HitPacket packet = new(transform.position, damage);
        if (TryHitOther(packet, collision))
        {
            if (hasLeeching)
            {
                leechingItem.ApplyModifierToPlayer();
            }
            hitAmount++;
            if (hitAmount > maxHits)
            {
                Destroy(gameObject);
            }
        }
    }
}
