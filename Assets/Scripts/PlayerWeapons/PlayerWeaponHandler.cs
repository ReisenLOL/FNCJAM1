using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponHandler : MonoBehaviour
{
    static PlayerWeaponHandler instance;
    Dictionary<string, WeaponAttack> spawnedWeapons;
    [SerializeField] Transform playerWeaponsFolder;
    [SerializeField] WeaponAttack startingWeapon;
    private void Awake()
    {
        spawnedWeapons = new();
        instance = this;
        for (int i = 0; i < playerWeaponsFolder.childCount; i++)
        {
            Destroy(playerWeaponsFolder.GetChild(i).gameObject);
        }
    }
    private void Start()
    {
        FindWeaponReference(startingWeapon, out bool wasCreated);
    }

    public static WeaponAttack FindWeaponReference(WeaponAttack weapon, out bool wasCreated)
    {
        if (instance == null)
        {
            Debug.LogError("Missing PlayerWeaponHandler.cs instance");
        }
        if (instance.spawnedWeapons.TryGetValue(weapon.WeaponName, out WeaponAttack foundWeapon))
        {
            wasCreated = false;
            return foundWeapon;
        }
        else
        {
            WeaponAttack spawned = Instantiate(weapon, instance.playerWeaponsFolder.transform);
            spawned.SetOwner(BaseUnit.Player);
            instance.spawnedWeapons[weapon.WeaponName] = spawned;
            wasCreated = true;
            return weapon;
        }
    }
}
