using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShowDescriptionOnButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Item buttonItem;
    public int itemLevel;
    [SerializeField] RectTransform descriptionPanel;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] TextMeshProUGUI descriptionName;
    [SerializeField] TextMeshProUGUI descriptionLevel;
    [SerializeField] Image descriptionImage;
    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowDecription(buttonItem);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        descriptionPanel.gameObject.SetActive(false);
    }
    private void ShowDecription(Item w)
    {
        descriptionPanel.gameObject.SetActive(true);
        descriptionText.text = w.itemDescription;
        descriptionName.text = w.ItemName;
        descriptionImage.sprite = w.itemImage;
        Item[] equippedItems = FindObjectsByType<Item>(FindObjectsSortMode.None);
        Item foundItem = null;
        for (int i = 0; i < equippedItems.Length; i++)
        {
            if (equippedItems[i].ItemName == w.ItemName)
            {
                foundItem = equippedItems[i]; //this code is so cooked;
                break;
            }
            
        }
        if (foundItem != null)
        {
            descriptionLevel.gameObject.SetActive(true);
            if (foundItem.gameObject.TryGetComponent(out WeaponAttack isWeaponAttack))
            {
                Debug.Log(isWeaponAttack.level);
                descriptionLevel.text = "Level: " + (isWeaponAttack.level + 1);
                descriptionText.text = isWeaponAttack.WeaponLevels[isWeaponAttack.level].upgradeDescription;
            }
            else if (foundItem.gameObject.TryGetComponent(out Passive isPassiveItem))
            {
                descriptionLevel.text = "Level: " + (isPassiveItem.level + 1);
                descriptionText.text = isPassiveItem.itemDescription;
                descriptionText.text = isPassiveItem.passiveLevels[isPassiveItem.level].upgradeDescription;
            }
        }
        else
        {
            descriptionLevel.gameObject.SetActive(false);
        }
    }
}
