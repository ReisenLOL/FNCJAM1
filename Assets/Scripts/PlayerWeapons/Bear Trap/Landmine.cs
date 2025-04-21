using Bremsengine;
using System.Collections.Generic;
using UnityEngine;

public class Landmine : Weapon
{
    [SerializeField] LayerMask unitCollectionLayer;
    [SerializeField] float explosionRadius;
    private void Start()
    {
        if (EnemyUnit.TryGetRandomAliveEnemy(out EnemyUnit a))
        {
            RotateToTarget(a.CurrentPosition);
        }
        SetWeaponProperties();
        explosionRadius = weaponLevelData.specialPropertyA;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        HitPacket packet = new(transform.position, damage);
        if (TryHitOther(packet, collision))
        {
            GeneralManager.FunnyExplosion(transform.position, explosionRadius * 0.5f);
            EnemyUnit originalHitUnit = collision.GetComponent<EnemyUnit>();
            if (EnemyUnit.TryFindInCircleCast(transform.position, explosionRadius, unitCollectionLayer, out HashSet<EnemyUnit> explodedUnits))
            {
                foreach (EnemyUnit item in explodedUnits)
                {
                    if (item == originalHitUnit) { continue; }
                    packet.HitPosition = item.CurrentPosition;
                    item.PerformHit(packet);
                }
            }
            Destroy(gameObject);
        }
    }
}
