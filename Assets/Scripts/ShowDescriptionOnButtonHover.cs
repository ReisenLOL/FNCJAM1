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
        GameObject foundItem = GameObject.Find(w.gameObject.name);
        if (foundItem != null)
        {
            descriptionLevel.gameObject.SetActive(true);
            if (foundItem.gameObject.TryGetComponent(out WeaponAttack isWeaponAttack))
            {
                descriptionLevel.text = "Level: " + isWeaponAttack.level;
            }
            else if (foundItem.gameObject.TryGetComponent(out Passive isPassiveItem))
            {
                descriptionLevel.text = "Level: " + isPassiveItem.level;
            }
        }
        else
        {
            descriptionLevel.gameObject.SetActive(false);
        }
    }
}
