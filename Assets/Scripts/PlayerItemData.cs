using System.Collections.Generic;
using UnityEngine;

public class PlayerItemData : MonoBehaviour
{
    [System.Serializable]
    public class EquippedItemData
    {
        public string itemID;
        public int level;

        public EquippedItemData(string id, int lvl)
        {
            itemID = id;
            level = lvl;
        }
    }

    public static PlayerItemData instance;
    public List<EquippedItemData> equippedItems = new();
    public int playerLevel;
    public float powerAmount;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void ResetStats()
    {
        equippedItems.Clear();
        playerLevel = 0;
        powerAmount = 0;
    }
}
