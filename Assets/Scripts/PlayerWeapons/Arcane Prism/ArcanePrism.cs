using UnityEngine;

public class ArcanePrism : Weapon
{
    public float rotationSpeed;
    public float fireRate;
    private float fireTime;
    public float offset;
    public Weapon attack;
    public AudioClip attackSound;
    public float attackSoundVolume;
    private void Start()
    {
        transform.parent = firedFrom.transform;
        UpdatePosition();
        transform.RotateAround(firedFrom.transform.position, Vector3.forward, 90 * weaponNumber);
        SetWeaponProperties();
    }
    void UpdatePosition()
    {
        transform.position = transform.parent.position + (transform.right * offset);
    }

    void Update()
    {
        transform.RotateAround(firedFrom.transform.position, Vector3.forward, rotationSpeed * Time.deltaTime);
        fireTime += Time.deltaTime;
        if (fireTime > fireRate)
        {
            FireProjectile();
            fireTime -= fireRate;
        }
    }
    private void FireProjectile()
    {
        Weapon spawnedAttack = Instantiate(attack, transform.position, attack.transform.rotation);
        spawnedAttack.weaponLevelData = weaponLevelData;
        spawnedAttack.SetOwner(Owner);
        spawnedAttack.firedFrom = gameObject;
        if (EnemyUnit.TryGetRandomAliveEnemy(out EnemyUnit a))
        {
            spawnedAttack.RotateToTarget(a.CurrentPosition);
        }
        else
        {
            spawnedAttack.RotateToTarget(Vector2.right);
        }
        if (dissipationDelay != 0)
        {
            spawnedAttack.dissipationDelay = dissipationDelay;
        }
        if (attackSound != null)
        {
            GetComponentInParent<AudioSource>().PlayOneShot(attackSound, attackSoundVolume);
        }
    }
}
