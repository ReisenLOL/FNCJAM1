using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveDataFix : MonoBehaviour
{
    void Start()
    {
        InvokeRepeating("SendToFirstScene", 0.2f, 10f);
    }

    public void SendToFirstScene()
    {
        Item[] itemList = FindObjectsByType<Item>(FindObjectsSortMode.None);
        foreach (Item item in itemList)
        {
            if (item.TryGetComponent(out WeaponAttack isWeaponAttack))
            {
                PlayerItemData.instance.equippedItems.Add(new PlayerItemData.EquippedItemData(isWeaponAttack.ItemName, isWeaponAttack.level));
            }
            if (item.TryGetComponent(out Passive isPassive))
            {
                PlayerItemData.instance.equippedItems.Add(new PlayerItemData.EquippedItemData(isPassive.ItemName, isPassive.level));
            }
        }
        PlayerLevelManager levelManager = FindAnyObjectByType<PlayerLevelManager>();
        PlayerItemData.instance.playerLevel = levelManager.level;
        PlayerItemData.instance.powerAmount = levelManager.currentPower;
        PlayerItemData.instance.requiredPowerToNextLevel = levelManager.requiredPowerToNextLevel;
        PlayerItemData.instance.requiredPowerToNextLevelMin = levelManager.requiredPowerToNextLevelMin;
        SceneManager.LoadScene(1);
    }
}
