using UnityEngine;

public class Landmine : Weapon
{
    private void Start()
    {
        SetWeaponProperties();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        HitPacket packet = new(transform.position, damage);
        if (TryHitOther(packet, collision))
        {
            Destroy(gameObject);
        }
    }
}
