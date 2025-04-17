using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShowDescriptionOnButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Item buttonItem;
    [SerializeField] RectTransform descriptionPanel;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] TextMeshProUGUI descriptionName;
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
    }
}
