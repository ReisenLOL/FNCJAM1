using Core.Extensions;
using System.Collections;
using UnityEngine;

public class WeaponAttack : Item
{
    public bool hasLeeching;
    [HideInInspector]
    public LeechingSteel leechingItem;
    public bool canDuplicateProjectiles = false;

    public int level;
    public WeaponLevelData[] WeaponLevels;
    public float nextAttackTime;
    public float shootDuration = 0.25f;
    public bool willRegenerate = true;
    public bool canFire = true;
    public Weapon attack;
    [SerializeField] ACWrapper attackSound;
    float attackRate;
    public float attackCountModifier = 1;
    public float attackRateModifier = 1;
    public float speedModifier = 1;
    public float damageModifier = 1;
    public Passive requiredEvolutionItem;
    public bool isEvolvedForm;
    public WeaponAttack evolvedForm;
    public WeaponAttack baseForm;
    public bool isOnWeaponList = true;
    public bool canEvolve;
    [Tooltip("Optional")]
    public Transform overrideSpawnPosition;
    public WeaponSelect weaponSelect;
    public Vector2 SpawnPosition => overrideSpawnPosition == null ? transform.position : overrideSpawnPosition.position;
    private void Start()
    {
        attackRate = WeaponLevels[level.Clamp(0, WeaponLevels.Length)].attackRate;
        if (isEvolvedForm)
        {
            EvolutionUI evolutionUI = FindFirstObjectByType<EvolutionUI>();
            evolutionUI.MarkKnownEvolution(baseForm, baseForm.requiredEvolutionItem);
        }
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
        if (!isEvolvedForm && !canEvolve && refreshWeaponList && requiredEvolutionItem != null)
        {
            for (int i = 0; i < transform.parent.childCount; i++)
            {
                if (transform.parent.GetChild(i).TryGetComponent(out Passive p))
                {
                    if (p.ItemName == requiredEvolutionItem.ItemName)
                    {
                        canEvolve = true;
                        break;
                    }
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
        float attackCount = WeaponLevels[level.Clamp(0, WeaponLevels.Length)].attackCount * attackCountModifier;
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
                if (hasLeeching)
                {
                    spawnedAttack.hasLeeching = hasLeeching;
                    spawnedAttack.leechingItem = leechingItem;
                }
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
