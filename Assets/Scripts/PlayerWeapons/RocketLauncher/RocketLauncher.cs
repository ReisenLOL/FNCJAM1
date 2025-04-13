using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RocketLauncher : Weapon
{
    private float dissipationTime;
    [SerializeField] LayerMask unitCollectionLayer;
    [SerializeField] float explosionRadius = 5f;
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
            EnemyUnit originalHitUnit = collision.GetOrAddComponent<EnemyUnit>();
            if (EnemyUnit.TryFindInCircleCast(transform.position, explosionRadius, unitCollectionLayer, out HashSet<EnemyUnit> explodedUnits))
            {
                foreach (var item in explodedUnits)
                {
                    if (item == originalHitUnit)
                        continue;
                    item.PerformHit(packet);
                }
            }
            Destroy(gameObject);
        }
    }
}
