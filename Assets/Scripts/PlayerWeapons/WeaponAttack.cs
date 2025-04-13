using UnityEngine;

public class WeaponAttack : MonoBehaviour
{
    public int level;
    public int maxLevel;
    public float attackRate;
    public float attackTime;
    public GameObject attack;
    public float damageDealt;
    public float range;
    // Update is called once per frame
    void Update()
    {
        attackTime += Time.deltaTime;
        if (attackTime >= attackRate)
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
}
