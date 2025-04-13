using Core.Extensions;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelect : MonoBehaviour
{
    public List<WeaponAttack> KnownWeapons = new();
    static HashSet<string> existingSelectionOptions;
    static int currentSelectionItemCount;
    public Button buttonPrefab;
    [SerializeField] RectTransform selectionPanel;
    void Start()
    {
        RebuildWeaponList(3);
        HideWeaponSelect();
    }
    public void ShowWeaponSelect()
    {
        RebuildWeaponList(3);
        selectionPanel.gameObject.SetActive(true);
    }
    public void HideWeaponSelect()
    {
        selectionPanel.gameObject.SetActive(false);
    }
    private void SelectWeapon(WeaponAttack w)
    {
        WeaponAttack weapon = PlayerWeaponHandler.FindWeaponReference(w, out bool wasCreated);
        if (!wasCreated)
        {
            weapon.LevelUp();
        }
        HideWeaponSelect();
    }
    private void RebuildWeaponList(int choices)
    {
        for (int i = 0; i < selectionPanel.childCount; i++)
        {
            Destroy(selectionPanel.GetChild(i).gameObject);
        }
        ShowWeaponChoices(KnownWeapons, choices);
    }
    public void ShowWeaponChoices(List<WeaponAttack> weaponChoices, int choices)
    {
        currentSelectionItemCount = 0;
        existingSelectionOptions = new();
        int attempts = 50;
        while (attempts > 0 && currentSelectionItemCount < choices)
        {
            attempts--;
            int random = 0.RandomBetween(0, KnownWeapons.Count);
            Debug.Log(random);
            WeaponAttack choice = KnownWeapons[random];
            if (existingSelectionOptions.Contains(choice.WeaponName))
                continue;
            currentSelectionItemCount++;
            string weaponName = choice.WeaponName;
            Button newButton = Instantiate(buttonPrefab, selectionPanel);
            newButton.gameObject.SetActive(true);
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = weaponName;
            newButton.GetComponent<Button>().onClick.AddListener(() => SelectWeapon(choice));
            newButton.name = weaponName;
            existingSelectionOptions.Add(choice.WeaponName);
        }
    }
}
