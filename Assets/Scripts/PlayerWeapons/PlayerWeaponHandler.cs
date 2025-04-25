using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponHandler : MonoBehaviour
{
    static PlayerWeaponHandler instance;
    Dictionary<string, Item> spawnedWeapons;
    [SerializeField] Transform playerWeaponsFolder;
    [SerializeField] Item startingWeapon;
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

    public static Item FindWeaponReference(Item weapon, out bool wasCreated)
    {
        if (instance == null)
        {
            Debug.LogError("Missing PlayerWeaponHandler.cs instance");
        }
        if (instance.spawnedWeapons.TryGetValue(weapon.ItemName, out Item foundWeapon))
        {
            wasCreated = false;
            return foundWeapon;
        }
        else
        {
            Item spawned = Instantiate(weapon, instance.playerWeaponsFolder.transform);
            spawned.SetOwner(BaseUnit.Player);
            instance.spawnedWeapons[weapon.ItemName] = spawned;
            wasCreated = true;
            return spawned;
        }
    }
}
