using Core.Extensions;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelect : MonoBehaviour
{
    public List<Item> KnownItems = new();
    static HashSet<string> existingSelectionOptions;
    static int currentSelectionItemCount;
    public Button buttonPrefab;
    public ItemList itemList;
    [SerializeField] RectTransform selectionPanel;
    [SerializeField] GameObject descriptionPanel;
    public static bool IsSelecting { get; private set; }
    void Start()
    {
        RebuildWeaponList(4);
        HideWeaponSelect();
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
    private void SelectWeapon(Item w)
    {
        Item weapon = PlayerWeaponHandler.FindWeaponReference(w, out bool wasCreated);
        if (!wasCreated)
        {
            if (weapon.gameObject.TryGetComponent(out WeaponAttack isWeaponAttack))
            {
                isWeaponAttack.LevelUp();
            }
            else if (weapon.gameObject.TryGetComponent(out Passive isPassiveItem))
            {
                isPassiveItem.LevelUp();
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
                if (KnownItems.Contains(equippedWeapons[i]))
                {
                    KnownItems.Remove(equippedWeapons[i]);
                }
            }
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
