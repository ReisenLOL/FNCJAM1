using Core.Extensions;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelect : MonoBehaviour
{
    public List<Item> KnownItems = new();
    //knownItems appear in shop, all items is just a list of every item that can be equipped.
    public List<Item> AllItems = new();
    static HashSet<string> existingSelectionOptions;
    static int currentSelectionItemCount;
    public Button buttonPrefab;
    public ItemList itemList;
    public int itemCapacity;
    public bool weaponsAtMaxCapacity;
    public bool passivesAtAMaxCapacity;
    public int weaponCount;
    public int passiveCount;
    [SerializeField] RectTransform selectionPanel;
    [SerializeField] GameObject descriptionPanel;
    public static bool IsSelecting { get; private set; }
    void Start()
    {
        RebuildWeaponList(4);
        HideWeaponSelect();
        RestoreWeapons();
    }
    void RestoreWeapons()
    {
        foreach (PlayerItemData.EquippedItemData equippedItemData in PlayerItemData.instance.equippedItems)
        {
            Item itemToRestore = AllItems.Find(w => w.ItemName.Trim().ToLower() == equippedItemData.itemID.Trim().ToLower());
            if (itemToRestore != null)
            {
                SelectWeapon(itemToRestore, equippedItemData.level);
            }
            else
            {
                Debug.LogWarning("lmao: " + equippedItemData.itemID);
            }
        }
    }
    public void ShowWeaponSelect()
    {
        RebuildWeaponList(4);
        IsSelecting = true;
        selectionPanel.gameObject.SetActive(true);
    }
    public void HideWeaponSelect()
    {
        IsSelecting = false;
        selectionPanel.gameObject.SetActive(false);
    }
    private void SelectWeapon(Item w, int targetLevel = 1)
    {
        Item weapon = PlayerWeaponHandler.FindWeaponReference(w, out bool wasCreated);
        if (!wasCreated)
        {
            if (weapon.gameObject.TryGetComponent(out WeaponAttack isWeaponAttack))
            {
                for (int i = 1; i < targetLevel; i++)
                {
                    isWeaponAttack.LevelUp();
                }
            }
            else if (weapon.gameObject.TryGetComponent(out Passive isPassiveItem))
            {
                for (int i = 1; i < targetLevel; i++)
                {
                    isPassiveItem.LevelUp();
                }
            }
        }
        if (weapon.TryGetComponent(out WeaponAttack isWeaponAttackOfEvolvedForm) && isWeaponAttackOfEvolvedForm.isEvolvedForm)
        {
            WeaponAttack[] equippedWeapons = FindObjectsByType<WeaponAttack>(FindObjectsSortMode.None);
            for (int i = 0; i < equippedWeapons.Length; i++)
            {
                if (equippedWeapons[i].ItemName == isWeaponAttackOfEvolvedForm.baseForm.ItemName)
                {
                    Destroy(equippedWeapons[i]); //this code is so cooked
                    break;
                }
            }
        }
        Item[] equippedItems = FindObjectsByType<Item>(FindObjectsSortMode.None);
        if (equippedItems.Length > 0)
        {
            for (int i = 0; i < equippedItems.Length; i++)
            {
                equippedItems[i].refreshWeaponList = true;
            }
        }
        if (descriptionPanel.activeSelf)
        {
            descriptionPanel.SetActive(false);
        }
        itemList.RebuildItemList();
        HideWeaponSelect();
    }
    private void RebuildWeaponList(int choices)
    {
        for (int i = 0; i < selectionPanel.childCount; i++)
        {
            Destroy(selectionPanel.GetChild(i).gameObject);
        }
        ShowWeaponChoices(KnownItems, choices);
    }
    public void ShowWeaponChoices(List<Item> weaponChoices, int choices)
    {
        WeaponAttack[] equippedWeapons = FindObjectsByType<WeaponAttack>(FindObjectsSortMode.None);
        Passive[] equippedPassives = FindObjectsByType<Passive>(FindObjectsSortMode.None);
        weaponCount = equippedWeapons.Length;
        passiveCount = equippedPassives.Length;
        HashSet<string> equippedWeaponNames = new HashSet<string>(equippedWeapons.Select(w => w.ItemName));
        HashSet<string> equippedPassiveNames = new HashSet<string>(equippedPassives.Select(p => p.ItemName));
        for (int i = 0; i < equippedWeapons.Length; i++)
        {
            if (equippedWeapons[i].level == equippedWeapons[i].WeaponLevels.Length - 1)
            {
                Debug.Log("Found max level weapon: ", equippedWeapons[i]);
                if (equippedWeapons[i].canEvolve && !KnownItems.Contains(equippedWeapons[i].evolvedForm))
                {
                    Debug.Log("Evolvable weapon found");
                    KnownItems.Add(equippedWeapons[i].evolvedForm);
                }
                if (equippedWeapons[i].isOnWeaponList)
                {
                    for (int j = 0; j < KnownItems.Count; j++)
                    {
                        if (KnownItems[j].ItemName == equippedWeapons[i].ItemName)
                        {
                            KnownItems.Remove(KnownItems[j]);
                            equippedWeapons[i].isOnWeaponList = false;
                            break;
                        }
                    }
                }
            }
        }
        for (int i = 0; i < equippedPassives.Length; i++)
        {
            if (equippedPassives[i].level == equippedPassives[i].passiveLevels.Length - 1)
            {
                if (equippedPassives[i].isOnPassiveList)
                {
                    for (int j = 0; j < KnownItems.Count; j++)
                    {
                        if (KnownItems[j].ItemName == equippedPassives[i].ItemName)
                        {
                            KnownItems.Remove(KnownItems[j]);
                            equippedPassives[i].isOnPassiveList = false;
                            break;
                        }
                    }
                }
            }
        }
        if (weaponCount >= itemCapacity && !weaponsAtMaxCapacity)
        {
            KnownItems.RemoveAll(item => item.TryGetComponent<WeaponAttack>(out _) && !equippedWeaponNames.Contains(item.ItemName));
            weaponsAtMaxCapacity = true;
        }
        if (passiveCount >= itemCapacity && !passivesAtAMaxCapacity)
        {
            KnownItems.RemoveAll(item => item.TryGetComponent<Passive>(out _) && !equippedPassiveNames.Contains(item.ItemName));
            passivesAtAMaxCapacity = true;
        }
        currentSelectionItemCount = 0;
        existingSelectionOptions = new();
        int attempts = 50;
        while (attempts > 0 && currentSelectionItemCount < choices)
        {
            attempts--;
            int random = 0.RandomBetween(0, KnownItems.Count);
            Item choice = KnownItems[random];
            if (existingSelectionOptions.Contains(choice.ItemName))
                continue;
            currentSelectionItemCount++;
            string weaponName = choice.ItemName;
            Button newButton = Instantiate(buttonPrefab, selectionPanel);
            if (choice.itemImage != null)
            {
                Image buttonImage = newButton.transform.Find("Image").GetComponent<Image>();
                buttonImage.sprite = choice.itemImage;
            }
            newButton.gameObject.SetActive(true);
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = weaponName;
            newButton.GetComponent<Button>().onClick.AddListener(() => SelectWeapon(choice));
            newButton.GetComponent<ShowDescriptionOnButtonHover>().buttonItem = choice;
            newButton.name = weaponName;
            existingSelectionOptions.Add(choice.ItemName);
        }
    }
}
