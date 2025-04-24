using Core.Extensions;
using UnityEngine;

public class ReactiveShell : Weapon
{
    public PlayerUnit player;
    public float lastHealth;
    public Weapon attack;
    public Transform[] firingPositions;
    [SerializeField] ACWrapper attackSound;
    public float cooldown;
    public float cooldownTimer;
    private float dissipationTime;
    private void Start()
    {
        player = FindFirstObjectByType<PlayerUnit>();
        if (player != null)
        {
            lastHealth = player.CurrentHealth;
        }
        transform.parent = firedFrom.transform;
        SetWeaponProperties();
    }
    void Update()
    {
        cooldownTimer += Time.deltaTime;
        if (player.CurrentHealth < lastHealth)
        {
            if (cooldownTimer > cooldown)
            {
                for (int i = 0; i < firingPositions.Length; i++)
                {
                    FireProjectile(firingPositions[i]);
                }
                cooldownTimer = 0;
            }
        }
        lastHealth = player.CurrentHealth;
    }

    private void FireProjectile(Transform spawnPosition)
    {
        Weapon spawnedAttack = Instantiate(attack, transform.position, attack.transform.rotation);
        spawnedAttack.weaponLevelData = weaponLevelData;
        spawnedAttack.damageModifier = damageModifier;
        spawnedAttack.speedModifier = speedModifier;
        spawnedAttack.SetOwner(Owner);
        spawnedAttack.firedFrom = gameObject;
        spawnedAttack.RotateToTarget(spawnPosition.position);
        if (dissipationDelay != 0)
        {
            spawnedAttack.dissipationDelay = dissipationDelay;
        }
        attackSound.Play(transform.position);
    }
}
