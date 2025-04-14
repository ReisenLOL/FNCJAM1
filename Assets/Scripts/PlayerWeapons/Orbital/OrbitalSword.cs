using UnityEngine;

public class OrbitalSword : Weapon
{
    public float rotationSpeed;
    private float dissipationTime;
    public float offset;
    private void Start()
    {
        transform.parent = firedFrom.transform;
        UpdatePosition();
        transform.RotateAround(firedFrom.transform.position, Vector3.forward, 90 * weaponNumber);
    }
    void UpdatePosition()
    {
        transform.position = transform.parent.position + (transform.right * offset);
    }
    void Update()
    {
        dissipationTime += Time.deltaTime;
        if (dissipationTime >= dissipationDelay)
        {
            Destroy(gameObject);
        }
        transform.RotateAround(firedFrom.transform.position, Vector3.forward, rotationSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        HitPacket packet = new(transform.position, damage);
        if (TryHitOther(packet, collision))
        {
            
        }
    }
}
