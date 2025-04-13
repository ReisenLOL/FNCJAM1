using UnityEngine;

public class SwordSweep : Weapon
{
    public float offset;
    public float dissipationDelay;
    private float dissipationTime;
    void Start()
    {
        transform.parent = firedFrom.transform;
        UpdatePosition();
    }
    void UpdatePosition()
    {
        transform.position = transform.parent.position + (transform.right * offset);
    }
    private void Update()
    {
        dissipationTime += Time.deltaTime;
        if (dissipationTime >= dissipationDelay)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<UnitStats>().TakeDamage(damage); //oh it had no collider thats why it didnt take damage lol also ignore this i just like typing to myself
        }
    }
}
