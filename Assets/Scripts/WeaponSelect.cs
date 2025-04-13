using Core.Extensions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net.WebSockets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelect : MonoBehaviour
{
    public GameObject[] weapons;
    List<string> listedWeapons = new();
    private GameObject playerWeaponsFolder;
    public int weaponListAmount;
    public GameObject templateButton;
    void Start()
    {
        GetWeaponList();
    }
    public void SelectWeapon(string weaponName)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i].name == weaponName)
            {
                Transform weaponFound = playerWeaponsFolder.transform.Find(weaponName);
                if (weaponFound != null)
                {
                    Debug.Log(weaponFound + " weapon found ");
                    weaponFound.gameObject.GetComponent<WeaponAttack>().LevelUp();
                }
                else
                {
                    Instantiate(weapons[i], playerWeaponsFolder.transform);
                }
                break;
            }
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).gameObject == templateButton) { continue; } //why remove template button grr
            Destroy(transform.GetChild(i).gameObject);
        }
        transform.parent.gameObject.SetActive(false);
    }
    public void GetWeaponList()
    {
        for (int i = 0; i < weaponListAmount; i++)
        {
            if(listedWeapons.Count > weapons.Length - 1) { return; }
            playerWeaponsFolder = GameObject.Find("Weapons");
            int randomIndex = Random.Range(0, weapons.Length);
            if (listedWeapons.Count < weaponListAmount)
            {
                while (listedWeapons.Contains(weapons[randomIndex].name))
                {
                    randomIndex = Random.Range(0, weapons.Length);
                }
                ShowWeaponChoices(weapons, randomIndex);
            }
        }
    }

    public void ShowWeaponChoices(GameObject[] weaponChoices, int randomIndex)
    {
        Debug.Log("giving new choices...");
        GameObject newButton = Instantiate(templateButton, transform);
        if (newButton == null) { Debug.LogWarning("new button is null!"); return; }
        newButton.SetActive(true);
        newButton.GetComponentInChildren<TextMeshProUGUI>().text = weaponChoices[randomIndex].name;
        newButton.GetComponent<Button>().onClick.AddListener(() => SelectWeapon(newButton.GetComponentInChildren<TextMeshProUGUI>().text));
        newButton.GetComponent<Button>().onClick.AddListener(() => ClearWeaponChoices());
        newButton.name = weaponChoices[randomIndex].name;
        listedWeapons.Add(weapons[randomIndex].name);
    }

    public void ClearWeaponChoices()
    {
        listedWeapons.Clear();
    }
}
