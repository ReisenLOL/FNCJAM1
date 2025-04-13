using UnityEngine;

public class UnitController : MonoBehaviour
{
    private GameObject player;
    public float damage;
    public float speed;
    public float attackRate;
    private float attackTime;
    private Rigidbody2D rb;
    private Vector3 lookDirection;
    void Start()
    {
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        attackTime += Time.deltaTime;
        lookDirection = (player.transform.position - transform.position).normalized;
    }
    private void FixedUpdate()
    {
        rb.linearVelocity = lookDirection * speed;
    }// i dunno man
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (attackTime >= attackRate)
            {
                attackTime = 0;
                collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
            }
        }
    }
}
