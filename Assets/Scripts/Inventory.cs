using System.Collections.Generic;
using UnityEngine;
using System;

// This script manages the player's inventory – adding, removing, and combining items.
// It also fires off an update event so the UI can keep up with loot hoarding.
public class Inventory : MonoBehaviour
{
    [Serializable]
    public class InventoryItem
    {
        public ItemData itemData; // The item data
        public int quantity;      // How many of this crap the player has.
    }
    
    public List<InventoryItem> items = new List<InventoryItem>(); // The list of all items.
    public List<Recipe> recipes = new List<Recipe>();               // Recipes for combining items.

    public event Action OnInventoryUpdated; // Fire this event whenever your inventory changes.

    // Add an item to the inventory.
    public void AddItem(ItemData newItem, int quantity)
    {
        // See if the item already exists in the inventory.
        InventoryItem existingItem = items.Find(i => i.itemData == newItem);
        if (existingItem != null)
        {
            existingItem.quantity += quantity; // Just bump up the quantity.
        }
        else
        {
            // Create a new inventory item.
            InventoryItem newInventoryItem = new InventoryItem
            {
                itemData = newItem,
                quantity = quantity
            };
            items.Add(newInventoryItem);
        }
        Debug.Log("Added " + quantity + " " + newItem.itemName + " to inventory.");
        OnInventoryUpdated?.Invoke(); // Let everyone know the inventory just got updated.
    }
    
    // Remove an item from the inventory.
    public void RemoveItem(ItemData item, int quantity)
    {
        InventoryItem existingItem = items.Find(i => i.itemData == item);
        if (existingItem != null)
        {
            existingItem.quantity -= quantity;
            if (existingItem.quantity <= 0)
            {
                items.Remove(existingItem); // If there's none left, ditch the item.
            }
            OnInventoryUpdated?.Invoke(); // Update the UI, dammit.
        }
    }
    
    // Check if the inventory has at least a certain quantity of an item.
    public bool HasItem(ItemData item, int quantity)
    {
        InventoryItem existingItem = items.Find(i => i.itemData == item);
        return existingItem != null && existingItem.quantity >= quantity;
    }

    // Attempt to combine two items based on available recipes.
    public bool TryCombineItems(ItemData itemA, ItemData itemB)
    {
        foreach (Recipe recipe in recipes)
        {
            // Allow combining in either order.
            if ((recipe.ingredient1 == itemA && recipe.ingredient2 == itemB) ||
                (recipe.ingredient1 == itemB && recipe.ingredient2 == itemA))
            {
                // Check if the player has at least one of each ingredient.
                if (HasItem(itemA, 1) && HasItem(itemB, 1))
                {
                    RemoveItem(itemA, 1);
                    RemoveItem(itemB, 1);
                    AddItem(recipe.result, 1);
                    Debug.Log("Combined " + itemA.itemName + " and " + itemB.itemName + " to create " + recipe.result.itemName);
                    return true;
                }
            }
        }
        Debug.Log("No valid recipe found for " + itemA.itemName + " and " + itemB.itemName);
        return false;
    }
}
