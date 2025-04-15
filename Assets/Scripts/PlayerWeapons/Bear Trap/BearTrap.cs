using UnityEngine;

public class BearTrap : Weapon
{
    public int maxHits;
    public int hitAmount;
    private void Start()
    {
        SetWeaponProperties();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        HitPacket packet = new(transform.position, damage);
        if (TryHitOther(packet, collision))
        {
            hitAmount++;
            if (hitAmount > maxHits)
            {
                Destroy(gameObject);
            }
        }
    }
}
