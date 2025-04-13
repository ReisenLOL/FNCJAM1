using UnityEngine;

public class PowerItem : Collectable
{
    public PlayerLevelManager playerLevelManager;
    public int powerAmount;
    private Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerLevelManager = GameObject.Find("PlayerLevelManager").GetComponent<PlayerLevelManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerLevelManager.UpdatePower(powerAmount);
        Destroy(gameObject);
    }
    private void FixedUpdate()
    {
        if (isMovingToPlayer)
        {
            rb.linearVelocity = moveToPlayer * collectionSpeed;
        }
    }
}
