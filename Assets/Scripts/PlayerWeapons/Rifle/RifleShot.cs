using System.Collections.Generic;
using UnityEngine;

public class RifleShot : Weapon
{
    private float dissipationTime;
    public int pierceAmount;
    public int amountPierced;
    private void Start()
    {
        Debug.Log(Owner.Faction);
        SetWeaponProperties();
        pierceAmount = (int)weaponLevelData.specialPropertyA;
    }
    void Update()
    {
        transform.Translate(Vector2.right * Time.deltaTime * speed);
        Vector3 firedfrompos = firedFrom.transform.position;
        dissipationTime += Time.deltaTime;
        if (dissipationTime >= dissipationDelay)
        {
            Destroy(gameObject);
        }
        if (Vector3.Distance(transform.position, firedfrompos) > maxRange)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        HitPacket packet = new(transform.position, damage);
        if (TryHitOther(packet, collision))
        {
            amountPierced++;
            if (amountPierced > pierceAmount)
            {
                Destroy(gameObject);
            }
        }
    }
}
