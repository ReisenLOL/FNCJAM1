using System.Collections.Generic;
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
                    Debug.Log(weaponFound);
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
            Destroy(transform.GetChild(i).gameObject);
        }
        transform.parent.gameObject.SetActive(false);
    }
    public void GetWeaponList()
    {
        for (int i = 0; i < weaponListAmount; i++)
        {
            playerWeaponsFolder = GameObject.Find("Weapons");
            int randomIndex = Random.Range(0, weapons.Length);
            if (listedWeapons.Count != 0)
            {
                while (!listedWeapons.Contains(weapons[randomIndex].name))
                {
                    randomIndex = Random.Range(0, weapons.Length);
                }
            }
            GameObject newButton = Instantiate(templateButton, transform);
            newButton.SetActive(true);
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = weapons[randomIndex].name;
            newButton.GetComponent<Button>().onClick.AddListener(() => SelectWeapon(newButton.GetComponentInChildren<TextMeshProUGUI>().text));
            listedWeapons.Add(weapons[randomIndex].name);
        }
    }
}
