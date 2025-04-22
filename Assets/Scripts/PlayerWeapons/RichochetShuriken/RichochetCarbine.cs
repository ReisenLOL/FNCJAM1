using Core.Extensions;
using System.Transactions;
using UnityEngine;

public class RichochetCarbine : Weapon
{
    private float dissipationTime;
    public Transform sprite;
    Vector2 velocity;
    private Rigidbody2D rb;
    public float rotationSpeed;
    public int maxBounces;
    public int currentBounces;
    [SerializeField] ACWrapper bounceSound;
    private void Start()
    {
        maxBounces = (int)weaponLevelData.specialPropertyA;
        rb = GetComponent<Rigidbody2D>();
        SetWeaponProperties();
        SetVelocity(transform.right * speed);
    }
    void Update()
    {
        sprite.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        dissipationTime += Time.deltaTime;
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
            if (collision is BoxCollider2D box)
            {
                ScreenBounce(box);
            }
        }
    }
    private void SetVelocity(Vector2 direction)
    {
        velocity = direction;
        rb.linearVelocity = velocity;
        transform.Lookat2D((Vector2)transform.position + direction);
    }
    private void ScreenBounce(BoxCollider2D box)
    {
        if (TryGetExitNormal(box, transform.position, out Vector2 screenNormal))
        {
            bounceSound.Play(transform.position);
            SetVelocity(velocity.Bounce(screenNormal, 1f));
            currentBounces++;
            if (currentBounces > maxBounces)
            {
                Destroy(gameObject, 0.2f);
            }    
        }
    }
    public bool TryGetExitNormal(BoxCollider2D boxCollider, Vector2 pointInside, out Vector2 normal)
    {
        normal = Vector2.zero;
        if (boxCollider == null)
        {
            return false;
        }

        Bounds bounds = boxCollider.bounds;

        float left = bounds.min.x;
        float right = bounds.max.x;
        float bottom = bounds.min.y;
        float top = bounds.max.y;

        float distLeft = Mathf.Abs(pointInside.x - left);
        float distRight = Mathf.Abs(pointInside.x - right);
        float distBottom = Mathf.Abs(pointInside.y - bottom);
        float distTop = Mathf.Abs(pointInside.y - top);

        float minDist = Mathf.Min(distLeft, distRight, distBottom, distTop);

        if (minDist == distLeft)
            normal = Vector2.left;
        else if (minDist == distRight)
            normal = Vector2.right;
        else if (minDist == distBottom)
            normal = Vector2.down;
        else
            normal = Vector2.up;
        return normal != Vector2.zero;
    }
}
