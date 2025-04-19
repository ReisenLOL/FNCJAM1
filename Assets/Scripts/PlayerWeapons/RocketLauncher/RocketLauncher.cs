using Bremsengine;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RocketLauncher : Weapon
{
    private float dissipationTime;
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
            GeneralManager.FunnyExplosion(transform.position, explosionRadius * 0.5f);
            EnemyUnit originalHitUnit = collision.GetComponent<EnemyUnit>(); //this was for original unit check dunno why
            if (EnemyUnit.TryFindInCircleCast(transform.position, explosionRadius, unitCollectionLayer, out HashSet<EnemyUnit> explodedUnits))
            {
                foreach (EnemyUnit item in explodedUnits)
                {
                    if(item == originalHitUnit) { continue; }
                    packet.HitPosition = item.CurrentPosition;
                    item.PerformHit(packet);
                }
            }
            Destroy(gameObject);
        }
    }
}
