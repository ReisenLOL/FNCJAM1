using Bremsengine;
using Core.Extensions;
using UnityEngine;
using UnityEngine.UIElements;

public class ProjectileUnitController : MonoBehaviour
{
    [field: SerializeField] public BaseUnit Owner { get; private set; }
    public float damage;
    public float speed;
    public Weapon projectile;
    public WeaponLevelData weaponStats;
    public float attackRate;
    private float attackTime;
    public ACWrapper attackSound;
    public bool willFireAtPlayer;
    public float detectionRadius;
    public float projectileFireRate;
    public float projectileFireTime;
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
        if (DetectPlayer(detectionRadius) != null && !willFireAtPlayer)
        {
            willFireAtPlayer = true;
        }
        if (willFireAtPlayer)
        {
            projectileFireTime += Time.deltaTime;
            if (projectileFireTime >= projectileFireRate)
            {
                projectileFireTime = 0;
                Collider2D playerDetected = DetectPlayer(detectionRadius);
                if (playerDetected != null)
                {
                    Weapon spawnedAttack = Instantiate(projectile, transform.position, projectile.transform.rotation); // this is stupid WOW THAT WORKED?!
                    spawnedAttack.weaponLevelData = weaponStats;
                    spawnedAttack.SetOwner(Owner);
                    spawnedAttack.firedFrom = gameObject;
                    spawnedAttack.damage = weaponStats.damage;
                    spawnedAttack.damageModifier = 1;
                    spawnedAttack.speedModifier = 1;
                    spawnedAttack.RotateToTarget(playerDetected.transform.position);
                    spawnedAttack.weaponNumber = 0;
                    attackSound.Play(transform.position);
                }
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

