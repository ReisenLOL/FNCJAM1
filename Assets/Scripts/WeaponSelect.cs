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
    [SerializeField] RectTransform selectionPanel;
    [SerializeField] GameObject descriptionPanel;
    void Start()
    {
        RebuildWeaponList(4);
        HideWeaponSelect();
    }
    public void ShowWeaponSelect()
    {
        RebuildWeaponList(4);
        selectionPanel.gameObject.SetActive(true);
    }
    public void HideWeaponSelect()
    {
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
        Passive[] passives = FindObjectsByType<Passive>(FindObjectsSortMode.None);
        if (passives.Length > 0)
        {
            for (int i = 0; i < passives.Length; i++)
            {
                passives[i].refreshWeaponList = true;
            }
        }
        if (descriptionPanel.activeSelf)
        {
            descriptionPanel.SetActive(false);
        }
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
