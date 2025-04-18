using Core.Extensions;
using UnityEngine;

public class ArcanePrism : Weapon
{
    public float rotationSpeed;
    public float fireRate;
    private float fireTime;
    public float offset;
    public Weapon attack;
    [SerializeField] ACWrapper attackSound;
    public float attackSoundVolume;
    private void Start()
    {
        transform.parent = firedFrom.transform;
        UpdatePosition();
        transform.RotateAround(firedFrom.transform.position, Vector3.forward, 60 * weaponNumber);
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
        spawnedAttack.damageModifier = damageModifier;
        spawnedAttack.speedModifier = speedModifier;
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
        attackSound.Play(transform.position);
    }
}
