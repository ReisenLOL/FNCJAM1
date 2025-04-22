using UnityEngine;

public class Kusanagi : Weapon
{
    public float offset;
    private float dissipationTime;
    public float damageRate;
    public float damageTime;
    //hit twice then dissipate
    public LayerMask enemyLayer;
    public Transform pointA;
    public Transform pointB;
    public float knockbackForce;
    void Start()
    {
        transform.parent = firedFrom.transform;
        UpdatePosition();
        SetWeaponProperties();
    }
    void UpdatePosition()
    {
        transform.position = transform.parent.position + (transform.right * offset);
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
                        detectedEnemies[i].GetComponent<Rigidbody2D>().AddForce((detectedEnemies[i].transform.position - transform.position).normalized * knockbackForce);
                    }
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        HitPacket packet = new(transform.position, damage);
        if (TryHitOther(packet, collision))
        {

        }
    }
    private Collider2D[] DetectEnemies()
    {
        return Physics2D.OverlapAreaAll(pointA.position, pointB.position, enemyLayer);
    }
}
