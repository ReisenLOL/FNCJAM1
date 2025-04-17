using Bremsengine;
using Core.Extensions;
using UnityEngine;
using UnityEngine.UIElements;

public class UnitController : MonoBehaviour
{
    [field: SerializeField] public BaseUnit Owner { get; private set; }
    public float damage;
    public float speed;
    public float attackRate;
    private float attackTime;
    private Rigidbody2D rb;
    private Vector2 lookDirection;
    [SerializeField] float acceleration = 150f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        TickManager.MainTickLightweight += WhenTick;
    }
    private void OnDestroy()
    {
        TickManager.MainTickLightweight -= WhenTick;
    }
    private void Update()
    {
        lookDirection = Vector2.zero;
        if (BaseUnit.Player is not null and BaseUnit player && player.IsAlive)
        {
            lookDirection = (player.CurrentPosition - (Vector2)transform.position).normalized;
        }
    }
    void WhenTick()
    {
        if (Owner.IsStalled)
        {
            rb.VelocityTowards(Vector2.zero, acceleration * 1.5f);
            return;
        }
        rb.VelocityTowards(lookDirection.ScaleToMagnitude(speed), acceleration);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out IHitListener hitListener))
        {
            if (hitListener is not PlayerUnit)
                return;
            if (Time.time >= attackTime)
            {
                attackTime = Time.time + (1f / attackRate);
                hitListener.PerformHit(new(collision.contacts[0].point, Owner.DamageScale(damage)));
            }
        }
    }
}
