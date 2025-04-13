using UnityEngine;

public class WeaponAttack : MonoBehaviour
{
    public int level;
    public WeaponLevelData[] WeaponLevels;
    public float attackRate;
    public float attackTime;
    public GameObject attack;
    public float damageDealt;
    public float range;
    // Update is called once per frame

    private void Start()
    {
        attackRate = WeaponLevels[level].attackRate;
        damageDealt = WeaponLevels[level].damage;
        range = WeaponLevels[level].range;
    }

    void Update()
    {
        attackTime += Time.deltaTime;
        if (attackTime >= attackRate)
        {
            InstantiateAttack();
        }
    }
    void InstantiateAttack()
    {
        for (int i = 0; i < WeaponLevels[level].attackCount; i++)
        {
            attackTime -= attackRate;
            GameObject newAttack = Instantiate(attack, transform.position, attack.transform.rotation); // this is stupid WOW THAT WORKED?!
            Weapon weaponStats = newAttack.GetComponent<Weapon>();
            weaponStats.firedFrom = gameObject;
            weaponStats.damage = damageDealt;
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.nearClipPlane + 10;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            weaponStats.RotateToTarget(worldPos);
            weaponStats.maxRange = range;
        }
    }
    public void LevelUp()
    {
        if(level >= WeaponLevels.Length - 1) { return; } //just this line added to check if weapon level is above the cap
        level++;
        attackRate = WeaponLevels[level].attackRate;
        damageDealt = WeaponLevels[level].damage;
        range = WeaponLevels[level].range;

    }
}
