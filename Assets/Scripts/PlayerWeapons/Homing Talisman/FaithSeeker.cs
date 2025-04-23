using UnityEngine;

public class FaithSeeker : Weapon
{
    private float dissipationTime;
    private float homingRange;
    private float timeUntilHoming;
    private Collider2D homingTarget;
    public LayerMask enemyLayer;
    private float _time;
    private bool isHoming;
    public int maxHits;
    public int currentHits;
    private void Start()
    {
        SetWeaponProperties();
        homingRange = weaponLevelData.specialPropertyA;
        timeUntilHoming = weaponLevelData.specialPropertyB;
    }
    void Update()
    {
        if (!isHoming)
        {
            _time += Time.deltaTime;
        }
        if (_time > timeUntilHoming && !isHoming)
        {
            _time = 0;
            isHoming = true;
            homingTarget = DetectEnemies(homingRange);
        }
        if (isHoming && homingTarget != null)
        {
            RotateToTarget(homingTarget.transform.position);
        }
        if (isHoming && homingTarget == null)
        {
            isHoming = false;
            RotateToTarget(targetPosition);
            if (DetectEnemies(homingRange))
            {
                homingTarget = DetectEnemies(homingRange);
            }
        }
        transform.Translate(Time.deltaTime * speed * Vector2.right);
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
            Destroy(gameObject);
        }
    }
    private Collider2D DetectEnemies(float radius)
    {
        return Physics2D.OverlapCircle(transform.position, radius, enemyLayer);
    }
}
