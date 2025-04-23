using UnityEngine;
using UnityEngine.UIElements;

public class SpiritChain : Weapon
{
    public bool isAttachedtoEnemy;
    public Collider2D closestEnemy;
    public float damageRate;
    private float damageTimer;
    public LayerMask enemyLayer;

    private Transform playerTransform;
    private Vector3 originalScale;

    void Start()
    {
        playerTransform = firedFrom.transform;
        transform.parent = null;
        SetWeaponProperties();
        damageRate = weaponLevelData.specialPropertyA;
        originalScale = transform.localScale;
        transform.localScale = new Vector3(0, originalScale.y, 1);
    }

    void Update()
    {
        if (!isAttachedtoEnemy)
        {
            damageTimer += Time.deltaTime;
            if (damageTimer > damageRate)
            {
                damageTimer = 0f;
                closestEnemy = DetectEnemies();
                if (closestEnemy != null)
                {
                    isAttachedtoEnemy = true;
                }
            }
        }
        else
        {
            if (closestEnemy == null || !closestEnemy.gameObject.activeInHierarchy)
            {
                isAttachedtoEnemy = false;
                transform.localScale = Vector3.zero;
                return;
            }
            Vector3 start = playerTransform.position;
            Vector3 end = closestEnemy.transform.position;
            transform.position = (start + end) / 2f;
            Vector3 dir = end - start;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
            float length = dir.magnitude;
            transform.localScale = new Vector3(length, originalScale.y, 1);
            damageTimer += Time.deltaTime;
            if (damageTimer >= damageRate)
            {
                damageTimer = 0;
                HitPacket packet = new(transform.position, damage);
                if (TryHitOther(packet, closestEnemy))
                {
                    if (hasLeeching)
                    {
                        leechingItem.ApplyModifierToPlayer();
                    }
                }
            }
        }
    }

    private Collider2D DetectEnemies()
    {
        return Physics2D.OverlapCircle(playerTransform.position, maxRange, enemyLayer);
    }
}
