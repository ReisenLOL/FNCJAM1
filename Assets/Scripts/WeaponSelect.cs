using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelect : MonoBehaviour
{
    public GameObject[] weapons;
    List<GameObject> listedWeapons = new();
    private GameObject playerWeaponsFolder;
    public int weaponListAmount;
    public GameObject templateButton;
    void Start()
    {
        for (int i = 0; i < weaponListAmount; i++)
        {
            playerWeaponsFolder = GameObject.Find("Weapons");
            int n = Random.Range(0, weapons.Length);
            List<GameObject> shuffledWeapons = new List<GameObject>(weapons);
            Shuffle(shuffledWeapons);

            listedWeapons.AddRange(shuffledWeapons.GetRange(0, n));
            GameObject newButton = Instantiate(templateButton, transform);
            newButton.SetActive(true);
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = weapons[n].name;
            newButton.GetComponent<Button>().onClick.AddListener(() => SelectWeapon(newButton.GetComponentInChildren<TextMeshProUGUI>().text));
        }
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
                    weaponFound.gameObject.GetComponent<WeaponAttack>().level++;
                }
                else
                {
                    Instantiate(weapons[i], playerWeaponsFolder.transform);
                }
                gameObject.SetActive(false);
                break;
            }
        }
    }
    private void Shuffle(List<GameObject> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            GameObject temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
}
