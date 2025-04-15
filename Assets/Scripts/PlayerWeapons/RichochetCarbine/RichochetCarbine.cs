using Core.Extensions;
using UnityEngine;

public class RichochetCarbine : Weapon
{
    private float dissipationTime;
    Vector2 velocity;
    private Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SetWeaponProperties();
    }
    void Update()
    {
        dissipationTime += Time.deltaTime;
        if (dissipationTime >= dissipationDelay)
        {
            Destroy(gameObject);
        }
    }
    private void FixedUpdate()
    {
        rb.linearVelocity = transform.right * speed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        HitPacket packet = new(transform.position, damage);
        if (TryHitOther(packet, collision))
        {

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("MainCamera"))
        {
            ScreenBounce();
            RotateToTarget(velocity);
        }
    }
    private void ScreenBounce()
    {
        velocity = -velocity.QuantizeToStepSize(90f);
    }
}
