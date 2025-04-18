using Core.Extensions;
using System.Collections;
using UnityEngine;

public class WeaponAttack : Item
{
    public int level;
    public WeaponLevelData[] WeaponLevels;
    public float nextAttackTime;
    public float shootDuration = 0.25f;
    public bool willRegenerate = true;
    public bool canFire = true;
    public Weapon attack;
    [SerializeField] ACWrapper attackSound;
    float attackRate;
    public float attackRateModifier = 1;
    public float speedModifier = 1;
    public float damageModifier = 1;
    public GameObject requiredEvolutionItem;
    public WeaponAttack evolvedForm;
    public bool canEvolve;
    [Tooltip("Optional")]
    public Transform overrideSpawnPosition;
    public Vector2 SpawnPosition => overrideSpawnPosition == null ? transform.position : overrideSpawnPosition.position;
    private void Start()
    {
        attackRate = WeaponLevels[level.Clamp(0, WeaponLevels.Length)].attackRate;
    }
    void Update()
    {
        nextAttackTime += Time.deltaTime;
        if (!willRegenerate && canFire)
        {
            InstantiateAttack();
            canFire = false;
        }
        if (nextAttackTime >= attackRate && canFire)
        {
            //nATTime = 1f / attackRate.Clamp(0.1f, 10f);
            nextAttackTime = 0;
            InstantiateAttack();
        }
        if (!canEvolve && refreshWeaponList)
        {
            for (int i = 0; i < transform.parent.childCount; i++)
            {
                if (transform.parent.GetChild(i) == requiredEvolutionItem)
                {
                    canEvolve = true;
                    break;
                }
            }
            refreshWeaponList = false;
        }
    }
    void InstantiateAttack()
    {
        attackRate = WeaponLevels[level.Clamp(0, WeaponLevels.Length)].attackRate * attackRateModifier;
        float damageDealt = WeaponLevels[level.Clamp(0, WeaponLevels.Length)].damage * damageModifier;
        damageDealt = Owner.DamageScale(damageDealt);
        float attackCount = WeaponLevels[level.Clamp(0, WeaponLevels.Length)].attackCount;
        IEnumerator CO_Shoot()
        {
            WaitForSeconds repeatDelay = new WaitForSeconds(shootDuration / attackCount);
            for (int i = 0; i < attackCount; i++)
            {
                Weapon spawnedAttack = Instantiate(attack, SpawnPosition, attack.transform.rotation); // this is stupid WOW THAT WORKED?!
                spawnedAttack.weaponLevelData = WeaponLevels[level.Clamp(0, WeaponLevels.Length)];
                spawnedAttack.SetOwner(Owner);
                spawnedAttack.firedFrom = gameObject;
                spawnedAttack.damage = damageDealt;
                spawnedAttack.damageModifier = damageModifier;
                spawnedAttack.speedModifier = speedModifier;
                Vector3 mousePos = Input.mousePosition;
                mousePos.z = Camera.main.nearClipPlane + 10;
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
                spawnedAttack.RotateToTarget(worldPos);
                spawnedAttack.weaponNumber = i;
                attackSound.Play(transform.position);
                yield return repeatDelay;
            }
        }
        StartCoroutine(CO_Shoot());
    }
    public void LevelUp()
    {
        if (level >= WeaponLevels.Length - 1) { return; } //just this line added to check if weapon level is above the cap
        level++;
        if (!willRegenerate)
        {
            canFire = true;
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
    }
}
