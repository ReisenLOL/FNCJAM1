using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemList : MonoBehaviour
{
    public Image templateImage;
    public GameObject player;
    private void Start()
    {
        player = GameObject.Find("Player");
        RebuildItemList();
    }
    public void RebuildItemList()
    {
        Item[] items = player.GetComponentsInChildren<Item>();
        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
        for (int j = 0; j < items.Length; j++)
        {
            Image newImage = Instantiate(templateImage, transform);
            newImage.gameObject.SetActive(true);
            newImage.sprite = items[j].itemImage;
        }
    }
}
