using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class EvolutionUI : MonoBehaviour
{
    #region Save data stuff
    private string savePath => Path.Combine(Application.persistentDataPath, "evolutionSave.json");
    [System.Serializable]
    public class EvolutionSaveData
    {
        public List<string> knownEvolutionWeapons = new();
    }
    public void SaveKnownEvolutions()
    {
        EvolutionSaveData saveData = new EvolutionSaveData();
        foreach (WeaponEvolutions evolution in evolutionList)
        {
            if (evolution.isKnown)
            {
                string knownCombo = $"{evolution.weapon.ItemName}-{evolution.passive.ItemName}";
                saveData.knownEvolutionWeapons.Add(knownCombo);
            }
        }
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(savePath, json);
    }
    public void LoadKnownEvolutions()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            EvolutionSaveData saveData = JsonUtility.FromJson<EvolutionSaveData>(json);
            foreach (WeaponEvolutions evolution in evolutionList)
            {
                string knownCombo = $"{evolution.weapon.ItemName}-{evolution.passive.ItemName}";
                evolution.isKnown = saveData.knownEvolutionWeapons.Contains(knownCombo);
            }
        }
        RefreshList();
    }
    #endregion
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
        LoadKnownEvolutions();
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
        foreach(Transform template in evolutionUI)
        {
            Destroy(template.gameObject);
        }
        for (int i = 0; i < evolutionList.Count; i++)
        {
            NewEvolutionTemplate(evolutionList[i]);
        }
    }
    public void MarkKnownEvolution(Item weapon, Item passive)
    {
        WeaponEvolutions evolution = evolutionList.Find(e => e.weapon == weapon && e.passive == passive);
        {
            if (evolution != null)
            {
                evolution.isKnown = true;
                SaveKnownEvolutions();
                RefreshList();
            }
        }
    }
}
