using UnityEngine;

public class UnitStats : MonoBehaviour
{
    public float health;
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
