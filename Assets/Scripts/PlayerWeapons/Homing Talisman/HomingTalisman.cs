using UnityEngine;

public class HomingTalisman : Weapon
{
    private float dissipationTime;
    private float homingRange;
    private float timeUntilHoming;
    private Collider2D homingTarget;
    public LayerMask enemyLayer;
    private float _time;
    private bool isHoming;
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
            isHoming = true;
            homingTarget = DetectEnemies(homingRange);
        }
        if (isHoming && homingTarget != null)
        {
            RotateToTarget(homingTarget.transform.position);
        }
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
            Destroy(gameObject);
        }
    }
    private Collider2D DetectEnemies(float radius)
    {
        return Physics2D.OverlapCircle(transform.position, radius, enemyLayer);
    }
}
