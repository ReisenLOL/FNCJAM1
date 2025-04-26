using Core.Extensions;
using System.Collections;
using UnityEngine;

public class ArcanePrism : Weapon
{
    public float shootDuration = 0.25f;
    public float attackCount = 1;
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
        transform.RotateAround(firedFrom.transform.position, Vector3.forward, (360f / firedFrom.GetComponent<WeaponAttack>().WeaponLevels[firedFrom.GetComponent<WeaponAttack>().level].attackCount) * weaponNumber);
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
        IEnumerator CO_Shoot()
        {
            WaitForSeconds repeatDelay = new WaitForSeconds(shootDuration / attackCount);
            for (int i = 0; i < attackCount; i++)
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
                    spawnedAttack.RotateToTarget(transform.right);
                }
                if (dissipationDelay != 0)
                {
                    spawnedAttack.dissipationDelay = dissipationDelay;
                }
                attackSound.Play(transform.position);
            }
            yield return repeatDelay;
        }
        StartCoroutine(CO_Shoot());
    }
}
