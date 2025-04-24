using UnityEngine;

public class PowerItem : Collectable
{
    public PlayerLevelManager playerLevelManager;
    public float powerAmount;
    private Rigidbody2D rb;
    public Vector3 lookDirection;
    public AudioClip collectionSound;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerLevelManager = GameObject.Find("PlayerLevelManager").GetComponent<PlayerLevelManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerLevelManager.UpdatePower(powerAmount);
        collision.GetComponent<AudioSource>().PlayOneShot(collectionSound, 0.2f);
        Destroy(gameObject);
    }
    private void FixedUpdate()
    {
        if (isMovingToPlayer)
        {
            lookDirection = (moveToPlayer.position - transform.position).normalized;
            rb.linearVelocity = lookDirection * collectionSpeed;
        }
    }
}
