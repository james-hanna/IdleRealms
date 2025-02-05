using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public InventoryItem item;
    public int quantity = 0;
    
    public Image iconImage;
    public Text quantityText;
    
    public void SetItem(InventoryItem newItem, int newQuantity)
    {
        item = newItem;
        quantity = newQuantity;
        if(iconImage != null && item != null)
        {
            iconImage.sprite = item.icon;
            iconImage.enabled = true;
        }
        if(quantityText != null)
        {
            quantityText.text = quantity > 1 ? quantity.ToString() : "";
        }
    }
    
    public void ClearSlot()
    {
        item = null;
        quantity = 0;
        if(iconImage != null)
        {
            iconImage.sprite = null;
            iconImage.enabled = false;
        }
        if(quantityText != null)
        {
            quantityText.text = "";
        }
    }
    
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Item dropped on slot");
    }
}
