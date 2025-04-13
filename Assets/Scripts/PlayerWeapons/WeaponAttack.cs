using Core.Extensions;
using UnityEngine;

public class WeaponAttack : MonoBehaviour
{
    [SerializeField] BaseUnit Owner;
    [field: SerializeField] public string WeaponName { get; private set; } = "Headhunter, Leather Belt";
    public int level;
    public WeaponLevelData[] WeaponLevels;
    public float nextAttackTime;
    public Weapon attack;
    float attackRate;
    [Tooltip("Optional")]
    public Transform overrideSpawnPosition;
    public Vector2 SpawnPosition => overrideSpawnPosition == null ? transform.position : overrideSpawnPosition.position;
    private void Start()
    {
        attackRate = WeaponLevels[level.Clamp(0, WeaponLevels.Length)].attackRate;
    }
    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + (1f / attackRate.Clamp(0.1f, 10f));
            InstantiateAttack();
        }
    }
    void InstantiateAttack()
    {
        attackRate = WeaponLevels[level.Clamp(0, WeaponLevels.Length)].attackRate;
        float damageDealt = WeaponLevels[level.Clamp(0, WeaponLevels.Length)].damage;
        float range = WeaponLevels[level.Clamp(0, WeaponLevels.Length)].range;
        float attackCount = WeaponLevels[level.Clamp(0, WeaponLevels.Length)].attackCount;
        
        for (int i = 0; i < attackCount; i++)
        {
            Weapon spawnedAttack = Instantiate(attack, SpawnPosition, attack.transform.rotation); // this is stupid WOW THAT WORKED?!
            spawnedAttack.SetOwner(Owner);
            spawnedAttack.firedFrom = gameObject;
            spawnedAttack.damage = damageDealt;
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.nearClipPlane + 10;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            spawnedAttack.RotateToTarget(worldPos);
            spawnedAttack.maxRange = range;
        }
    }
    public void LevelUp()
    {
        if (level >= WeaponLevels.Length - 1) { return; } //just this line added to check if weapon level is above the cap
        level++;
    }
    public void SetOwner(BaseUnit owner)
    {
        Owner = owner;
    }
}
