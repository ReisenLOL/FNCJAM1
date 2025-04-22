using UnityEngine;

public class DoomSeal : Weapon
{
    public float damageRate;
    public float damageTime;
    public LayerMask enemyLayer;
    public float knockbackForce;
    void Start()
    {
        transform.parent = firedFrom.transform;
        SetWeaponProperties();
        transform.localScale = new Vector2(maxRange, maxRange);
    }
    private void Update()
    {
        damageTime += Time.deltaTime;
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
                        Vector3 knockbackDirection = detectedEnemies[i].transform.position - firedFrom.transform.position;
                        detectedEnemies[i].GetComponent<Rigidbody2D>().AddForce(knockbackDirection.normalized * knockbackForce, ForceMode2D.Impulse);
                    }
                }
            }
        }
    }
    private Collider2D[] DetectEnemies()
    {
        return Physics2D.OverlapCircleAll(transform.position, maxRange, enemyLayer);
    }
}
