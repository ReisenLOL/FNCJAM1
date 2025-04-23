using UnityEngine;

public class SwordSweep : Weapon
{
    public float offset;
    private float dissipationTime;
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
        dissipationTime += Time.deltaTime;
        if (dissipationTime >= dissipationDelay)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        HitPacket packet = new(transform.position, damage);
        if (TryHitOther(packet, collision))
        {
            Vector3 knockbackDirection = collision.transform.position - firedFrom.transform.position;
            collision.GetComponent<Rigidbody2D>().AddForce(knockbackDirection.normalized * knockbackForce, ForceMode2D.Impulse);
            if (hasLeeching)
            {
                leechingItem.ApplyModifierToPlayer();
            }
        }
    }
}
