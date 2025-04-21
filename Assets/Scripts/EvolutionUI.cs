using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EvolutionUI : MonoBehaviour
{
    [System.Serializable]
    public class WeaponEvolutions
    {
        public Item weapon;
        public Item passive;
        public Item result;
        public bool isKnown;
    }
    public List<WeaponEvolutions> evolutionList;
    public Transform evolutionUI;
    public GameObject evolutionTemplate;
    private void Start()
    {
        for (int i = 0; i < evolutionList.Count; i++)
        {
            NewEvolutionTemplate(evolutionList[i]);
        }
    }
    public void NewEvolutionTemplate(WeaponEvolutions evolution)
    {
        GameObject newTemplate = Instantiate(evolutionTemplate, evolutionUI);
        newTemplate.transform.Find("Weapon").GetComponent<Image>().sprite = evolution.weapon.itemImage;
        newTemplate.transform.Find("Passive").GetComponent<Image>().sprite = evolution.passive.itemImage;
        newTemplate.transform.Find("Result").GetComponent<Image>().sprite = evolution.result.itemImage;
        if (evolution.isKnown)
        {
            newTemplate.SetActive(true);
        }
    }
    public void RefreshList()
    {
        foreach(GameObject template in evolutionUI)
        {
            Destroy(template);
        }
        for (int i = 0; i < evolutionList.Count; i++)
        {
            NewEvolutionTemplate(evolutionList[i]);
        }
    }
}
