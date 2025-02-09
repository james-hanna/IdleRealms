using UnityEngine;
using UnityEngine.UI;

// This script handles the display for an individual inventory slot.
// It sets up the item image and quantity text to show what is in the slot.
public class InventoryItemButton : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image itemImage;      // The image component that displays the item sprite.
    [SerializeField] private Text quantityText;    // The text component that shows the item quantity.
    [SerializeField] private Sprite defaultSprite; // A default sprite for empty slots

    /// Sets the UI for this inventory slot.
    public void SetItem(ItemData item, int quantity)
    {
        // Check that we actually have an image to work with.
        if (itemImage == null)
        {
            Debug.LogError("InventoryItemButton: 'itemImage' is not assigned on " + gameObject.name);
            return;
        }

        // If there's a valid item and it has an icon, display it; otherwise, show the default sprite.
        if (item != null && item.icon != null)
        {
            itemImage.sprite = item.icon;
        }
        else
        {
            itemImage.sprite = defaultSprite;
        }

        // Update the quantity text
        if (quantityText != null)
        {
            quantityText.text = quantity > 0 ? quantity.ToString() : "";
        }
    }
}
