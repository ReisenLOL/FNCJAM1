using Core.Extensions;
using System.Collections.Generic;
using UnityEngine;

public class CelestialMissile : Weapon
{
    private float dissipationTime;
    public float knockbackForce;
    public float explosionRadius;
    public LayerMask unitCollectionLayer;
    private void Start()
    {
        SetWeaponProperties();
        explosionRadius = (int)weaponLevelData.specialPropertyB;
        knockbackForce = (int)weaponLevelData.specialPropertyA;
        RotateToMovement();
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
            EnemyUnit originalHitUnit = collision.GetComponent<EnemyUnit>(); //this was for original unit check dunno why
            if (EnemyUnit.TryFindInCircleCast(transform.position, explosionRadius, unitCollectionLayer, out HashSet<EnemyUnit> explodedUnits))
            {
                foreach (EnemyUnit item in explodedUnits)
                {
                    Vector3 knockbackDirection = collision.transform.position - transform.position;
                    item.GetComponent<Rigidbody2D>().AddForce(knockbackDirection.normalized * knockbackForce, ForceMode2D.Impulse);
                    if (item == originalHitUnit) { continue; }
                    packet.HitPosition = item.CurrentPosition;
                    item.PerformHit(packet);
                }
            }
            Destroy(gameObject);
        }
    }
    public void RotateToMovement()
    {
        Vector2 movementDirection = Owner.gameObject.GetComponent<PlayerController>().moveInput;
        if (movementDirection != Vector2.zero)
        {
            transform.Lookat2D((Vector2)transform.position + movementDirection);
        }
        else
        {
            transform.Lookat2D(Owner.gameObject.GetComponent<PlayerController>().LastMovementDirection);
        }
    }
}
