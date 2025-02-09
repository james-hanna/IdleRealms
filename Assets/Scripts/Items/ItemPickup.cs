// ItemPickup.cs
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [Tooltip("The item that this pickup represents.")]
    public ItemData itemData;
    
    [Tooltip("How many of this item to add.")]
    public int quantity = 1;
    
    private bool pickedUp = false;


    private void PickUp(Inventory playerInventory)
    {
        if (pickedUp)
            return;

        pickedUp = true;

        if (playerInventory != null)
        {
            playerInventory.AddItem(itemData, quantity);
        }
        Destroy(gameObject);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Inventory playerInventory = collision.GetComponent<Inventory>();
            PickUp(playerInventory);
        }
    }
    
    private void OnMouseDown()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Inventory playerInventory = player.GetComponent<Inventory>();
            PickUp(playerInventory);
        }
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButton(0) && !pickedUp)
        {
            OnMouseDown();
        }
    }
}
