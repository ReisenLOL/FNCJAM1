using UnityEngine;

public class OrbitalSword : Weapon
{
    public float rotationSpeed;
    private float dissipationTime;
    public float offset;
    public float rotationOffset;
    public bool willDissipate = true;
    private void Start()
    {
        transform.parent = firedFrom.transform;
        UpdatePosition();
        transform.RotateAround(firedFrom.transform.position, Vector3.forward, rotationOffset * weaponNumber);
        SetWeaponProperties();
    }
    void UpdatePosition()
    {
        transform.position = transform.parent.position + (transform.right * offset);
    }
    void Update()
    {
        if (hasLeeching)
        {
            leechingItem.ApplyModifierToPlayer();
        }
        dissipationTime += Time.deltaTime;
        if (willDissipate && dissipationTime >= dissipationDelay)
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
