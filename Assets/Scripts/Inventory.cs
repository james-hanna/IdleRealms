using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public int inventorySize = 20;
    public List<InventorySlot> slots = new List<InventorySlot>();
    
    public bool AddItem(InventoryItem newItem, int quantity = 1)
    {
        if(newItem.isStackable)
        {
            foreach (var slot in slots)
            {
                if(slot.item == newItem && slot.quantity < newItem.maxStack)
                {
                    int addable = newItem.maxStack - slot.quantity;
                    int toAdd = Mathf.Min(addable, quantity);
                    slot.SetItem(newItem, slot.quantity + toAdd);
                    quantity -= toAdd;
                    if(quantity <= 0)
                        return true;
                }
            }
        }
        
        foreach(var slot in slots)
        {
            if(slot.item == null)
            {
                int toAdd = newItem.isStackable ? Mathf.Min(quantity, newItem.maxStack) : 1;
                slot.SetItem(newItem, toAdd);
                quantity -= toAdd;
                if(quantity <= 0)
                    return true;
            }
        }
        
        Debug.Log("Inventory full or unable to add all items");
        return false;
    }
}
