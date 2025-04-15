using Bremsengine;
using Core.Extensions;
using UnityEngine;
using UnityEngine.UIElements;
using static Core.Extensions.TransformExtensions;

public class ExplosiveUnitController : MonoBehaviour
{
    [field: SerializeField] public BaseUnit Owner { get; private set; }
    public float damage;
    public float speed;
    public float attackRate;
    private float attackTime;
    public bool willSelfDestruct;
    public float selfDestructRadius;
    public float selfDestructRate;
    public float selfDestructTime;
    public float blastRadius;
    public float blastDamage;
    private Rigidbody2D rb;
    private Vector2 lookDirection;
    public LayerMask playerLayer;
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
        if (DetectPlayer(selfDestructRadius) != null && !willSelfDestruct)
        {
            willSelfDestruct = true;
        }
        if (willSelfDestruct)
        {
            selfDestructTime += Time.deltaTime;
            if (selfDestructTime >= selfDestructRate)
            {
                Collider2D isPlayerThere = DetectPlayer(blastRadius);
                if (isPlayerThere && isPlayerThere.gameObject.TryGetComponent(out IHitListener hitListener))
                {
                    hitListener.PerformHit(new(transform.position, Owner.DamageScale(damage)));
                }
                GeneralManager.FunnyExplosion(transform.position, blastRadius * 0.25f);
                Destroy(gameObject);
            }
        }
    }
    void WhenTick()
    {
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
    private Collider2D DetectPlayer(float radius)
    {
        return Physics2D.OverlapCircle(transform.position, radius, playerLayer);
    }
}
