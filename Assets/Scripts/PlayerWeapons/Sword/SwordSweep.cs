using UnityEngine;

public class SwordSweep : Weapon
{
    public float offset;
    private float dissipationTime;
    void Start()
    {
        transform.parent = firedFrom.transform;
        UpdatePosition();
        SetWeaponProperties();
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
        HitPacket packet = new(transform.position, damage);
        if (TryHitOther(packet, collision))
        {

        }
    }
}
