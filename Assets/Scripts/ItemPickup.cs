// ItemPickup.cs
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [Tooltip("The item that this pickup represents.")]
    public ItemData itemData;
    
    [Tooltip("How many of this item to add.")]
    public int quantity = 1;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Inventory playerInventory = collision.GetComponent<Inventory>();
            if (playerInventory != null)
            {
                playerInventory.AddItem(itemData, quantity);
            }
            Destroy(gameObject);
        }
    }
}
