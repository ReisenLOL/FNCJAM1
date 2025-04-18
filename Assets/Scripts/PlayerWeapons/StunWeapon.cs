using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StunWeapon : Weapon
{
    private float dissipationTime;
    private void Start()
    {
        if (EnemyUnit.TryGetRandomAliveEnemy(out EnemyUnit a))
        {
            RotateToTarget(a.CurrentPosition);
        }
        SetWeaponProperties();
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
        if (collision.gameObject.TryGetComponent(out EnemyUnit w))
        {
            w.SetStallTime(weaponLevelData.specialPropertyA);
        }
    }
}
