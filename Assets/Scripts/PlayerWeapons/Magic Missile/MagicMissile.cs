using Core.Extensions;
using UnityEngine;

public class MagicMissile : Weapon
{
    private float dissipationTime;
    public float knockbackForce;
    private void Start()
    {
        SetWeaponProperties();
        knockbackForce = (int)weaponLevelData.specialPropertyA;
        RotateToMovement();
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
            if (hasLeeching)
            {
                leechingItem.ApplyModifierToPlayer();
            }
            Vector3 knockbackDirection = collision.transform.position - firedFrom.transform.position;
            collision.GetComponent<Rigidbody2D>().AddForce(knockbackDirection.normalized * knockbackForce, ForceMode2D.Impulse);
            Destroy(gameObject);
        }
    }
    public void RotateToMovement()
    {
        Vector2 movementDirection = Owner.gameObject.GetComponent<PlayerController>().moveInput;
        if (movementDirection != Vector2.zero)
        {
            transform.Lookat2D((Vector2)transform.position + movementDirection);
        }
        else
        {
            transform.Lookat2D(Owner.gameObject.GetComponent<PlayerController>().LastMovementDirection);
        }
    }
}
