using UnityEngine;

public class MasterSpark : Weapon
{
    public float dissipationTime;
    public float damageRate;
    public float damageTime;
    public LayerMask enemyLayer;
    public Transform pointA;
    public Transform pointB;
    private void Start()
    {
        transform.parent = firedFrom.transform;
        transform.RotateAround(firedFrom.transform.position, Vector3.forward, Vector3.Angle(firedFrom.transform.position, targetPosition));
        transform.Translate(Vector3.right * transform.localScale.x * 0.5f);
        SetWeaponProperties();
    }
    private void Update()
    {
        damageTime += Time.deltaTime;
        dissipationTime += Time.deltaTime;
        if (dissipationTime >= dissipationDelay)
        {
            Destroy(gameObject);
        }
        if (damageTime > damageRate)
        {
            Collider2D[] detectedEnemies = DetectEnemies();
            if (detectedEnemies != null)
            {
                damageTime = 0;
                for (int i = 0; i < detectedEnemies.Length; i++)
                {
                    HitPacket packet = new(transform.position, damage);
                    if (TryHitOther(packet, detectedEnemies[i]))
                    {

                    }
                }
            }
        }
    }
    private Collider2D[] DetectEnemies()
    {
        return Physics2D.OverlapAreaAll(pointA.position, pointB.position, enemyLayer);
    }
}
