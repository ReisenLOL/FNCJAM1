using System.Collections.Generic;
using UnityEngine;

public class RifleShot : Weapon
{
    private float dissipationTime;
    public int pierceAmount;
    public int amountPierced;
    private void Start()
    {
        SetWeaponProperties();
        pierceAmount = (int)weaponLevelData.specialPropertyA;
    }
    void Update()
    {
        transform.Translate(Vector2.right * Time.deltaTime * speed);
        dissipationTime += Time.deltaTime;
        if (dissipationTime >= dissipationDelay)
        {
            Destroy(gameObject);
        }
        if (Vector3.Distance(transform.position, firedFrom.transform.position) > maxRange)
        {
            Destroy(gameObject);
        }
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
            amountPierced++;
            if (amountPierced > pierceAmount)
            {
                Destroy(gameObject);
            }
        }
    }
}
