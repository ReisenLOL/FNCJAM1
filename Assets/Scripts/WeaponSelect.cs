using Core.Extensions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net.WebSockets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelect : MonoBehaviour
{
    public List<WeaponAttack> KnownWeapons = new();
    public Button buttonPrefab;
    [SerializeField] RectTransform selectionPanel;
    void Start()
    {
        RebuildWeaponList();
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
    private void RebuildWeaponList(int choiceCount = 3)
    {
        for (int i = 0; i < selectionPanel.childCount; i++)
        {
            Destroy(selectionPanel.GetChild(i).gameObject);
        }
        for (int i = 0; i < choiceCount; i++)
        {
            int randomIndex = Random.Range(0, KnownWeapons.Count);
            ShowWeaponChoices(KnownWeapons, randomIndex);
        }
    }
    public void ShowWeaponChoices(List<WeaponAttack> weaponChoices, int randomIndex)
    {
        WeaponAttack choice = KnownWeapons[randomIndex];
        string weaponName = choice.WeaponName;
        Button newButton = Instantiate(buttonPrefab, selectionPanel);
        newButton.gameObject.SetActive(true);
        newButton.GetComponentInChildren<TextMeshProUGUI>().text = weaponName;
        newButton.GetComponent<Button>().onClick.AddListener(() => SelectWeapon(choice));
        newButton.name = weaponName;
    }
}
