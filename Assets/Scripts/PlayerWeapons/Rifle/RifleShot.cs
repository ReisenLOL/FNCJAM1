using UnityEngine;

public class RifleShot : Weapon
{
    public float dissipationDelay;
    private float dissipationTime;
    public int pierceAmount;
    public int amountPierced;
    void Update()
    {
        transform.Translate(Vector2.right * Time.deltaTime * speed);
        dissipationTime += Time.deltaTime;
        if (dissipationTime >= dissipationDelay)
        {
            Destroy(gameObject);
        }
        if (Vector3.Distance(transform.position, firedFrom.transform.position) > maxRange)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            amountPierced++;
            collision.gameObject.GetComponent<UnitStats>().TakeDamage(damage);
            if (amountPierced > pierceAmount)
            {
                Destroy(gameObject);
            }
        }
    }
}
