using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemList : MonoBehaviour
{
    public Image templateImage;
    public GameObject player;
    public Transform WeaponListUI;
    public Transform PassiveListUI;
    private void Start()
    {
        player = GameObject.Find("Player");
        RebuildItemList();
    }
    public void RebuildItemList()
    {
        Item[] items = player.GetComponentsInChildren<Item>();
        if (WeaponListUI.childCount > 0)
        {
            for (int i = 0; i < WeaponListUI.childCount; i++)
            {
                Destroy(WeaponListUI.GetChild(i).gameObject);
            }
        }
        if (PassiveListUI.childCount > 0)
        {
            for (int i = 0; i < PassiveListUI.childCount; i++)
            {
                Destroy(PassiveListUI.GetChild(i).gameObject);
            }
        }
        for (int j = 0; j < items.Length; j++)
        {
            Image newImage = Instantiate(templateImage);
            TextMeshProUGUI levelNumber = newImage.GetComponentInChildren<TextMeshProUGUI>();
            if (items[j].TryGetComponent(out WeaponAttack isWeapon))
            {
                newImage.transform.SetParent(WeaponListUI);
                levelNumber.text = (isWeapon.level + 1).ToString();
            }
            if (items[j].TryGetComponent(out Passive isPassive))
            {
                newImage.transform.SetParent(PassiveListUI);
                levelNumber.text = (isPassive.level + 1).ToString();
            }
            newImage.gameObject.SetActive(true);
            newImage.sprite = items[j].itemImage;
        }
    }
}
