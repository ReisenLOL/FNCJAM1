using Core.Extensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float fireRate;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    public GameObject projectile;
    private float damageDealt;
    private float range;
    private float fireRateTime;
    public bool canShoot = true;
    private Camera playerCam;
    private bool isFacingRight = true;
    private SpriteRenderer playerSpriteRenderer;
    private GameObject playerCamera;
    public bool hasFired = false;
    void Start()
    {
        playerCamera = GameObject.Find("Camera");
        playerSpriteRenderer = base.GetComponent<SpriteRenderer>();
        playerCam = GameObject.Find("Camera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize();
        if (hasFired)
        {
            fireRateTime += Time.deltaTime;
        }
        else
        {
            fireRateTime += fireRate;
        }
        /*if (canShoot && !EventSystem.current.IsPointerOverGameObject())
        {
            if (fireRateTime >= fireRate)
            {
                fireRateTime = 0;
                GameObject newProjectile = Instantiate(projectile, transform.position, projectile.transform.rotation); // this is stupid WOW THAT WORKED?!
                Projectile projectileStats = newProjectile.GetComponent<Projectile>();
                projectileStats.firedFrom = gameObject;
                projectileStats.damage = damageDealt;
                Vector3 mousePos = Input.mousePosition;
                mousePos.z = Camera.main.nearClipPlane + 10;
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
                projectileStats.RotateToTarget(worldPos);
                projectileStats.isEnemyBullet = false;
                projectileStats.maxRange = range;
                hasFired = true;
            }
        }*/
        if (playerCam.orthographicSize > 1f || playerCam.orthographicSize > 0f && Input.mouseScrollDelta.y < 0)
        {
            playerCam.orthographicSize -= Input.mouseScrollDelta.y / 2;
        }
        else if (playerCam.orthographicSize < 0f)
        {
            playerCam.orthographicSize = 5f;
        }
        if (this.moveInput.x > 0f && this.isFacingRight)
        {
            playerSpriteRenderer.flipX = true;
            this.isFacingRight = !this.isFacingRight;
        }
        else if (this.moveInput.x < 0f && !this.isFacingRight)
        {
            playerSpriteRenderer.flipX = false;
            this.isFacingRight = !this.isFacingRight;
        }
    }
    void FixedUpdate()
    {
        rb.linearVelocity = moveInput * speed;
    }
}
