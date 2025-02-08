using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

// This bad boy handles the inventory grid – building pages and slots so the shit doesn't look like a mess.
public class DynamicGridPages : MonoBehaviour
{
    [Header("Prefabs & References")]
    [Tooltip("The prefab for the page container. It must include a GridLayoutGroup with proper settings.")]
    public GameObject pagePrefab;  
    // Our page container prefab. Make sure it's set up with a GridLayoutGroup or else you'll be staring at a jumbled pile.

    [Tooltip("The prefab for an inventory slot. It should have the InventoryItemButton script attached.")]
    public GameObject inventorySlotPrefab;
    // This prefab represents each inventory slot. It needs the InventoryItemButton script so it knows what to do.

    [Tooltip("The panel that will contain the page(s).")]
    public Transform panelParent;
    // The parent panel where all these pages will live. Think of it as the inventory's home.

    [Header("Settings")]
    [Tooltip("How many slots per page.")]
    public int slotsPerPage = 25;

    // Reference to the Inventory script (which holds the item data)
    public Inventory playerInventory;
    

    // Internal list to keep track of the created page containers.
    private List<GameObject> pages = new List<GameObject>();

    private void OnEnable()
    {
        if (playerInventory != null)
            playerInventory.OnInventoryUpdated += UpdatePages;
        // Subscribe to inventory updates – so when items change, our UI doesn't get left in the dust.
    }

    private void OnDisable()
    {
        if (playerInventory != null)
            playerInventory.OnInventoryUpdated -= UpdatePages;
        // Unsubscribe to avoid memory leaks – don't be a douche with dangling events.
    }

    private void Start()
    {
        CreatePages(); // Build the damn pages.
        UpdatePages(); // Populate them with items or empty slots.
    }

    /// Creates the page containers based on the number of slots required.
    /// Always creates at least one page with the full number of slots.
    private void CreatePages()
    {
        // Clear out any existing pages because we don't want any leftover crap.
        foreach (Transform child in panelParent)
        {
            Destroy(child.gameObject);
        }
        pages.Clear();

        // Calculate total slots: at minimum, use slotsPerPage so a full grid shows even if inventory is empty.
        int totalSlots = Mathf.Max(playerInventory.items.Count, slotsPerPage);
        int totalPages = Mathf.CeilToInt((float)totalSlots / slotsPerPage);

        // Spawn the damn pages.
        for (int p = 0; p < totalPages; p++)
        {
            GameObject page = Instantiate(pagePrefab, panelParent);
            page.name = "Page " + (p + 1);
            pages.Add(page);
        }
    }

    /// Updates the content of each page with the current inventory items.
    /// Always populates each page with exactly slotsPerPage slots.
    private void UpdatePages()
    {
        // If you need dynamic page count changes, you could call CreatePages() here – but for now we assume it's set.
        for (int p = 0; p < pages.Count; p++)
        {
            Transform pageTransform = pages[p].transform;

            // Clear any existing slots on this page so we're not doubling up like idiots.
            foreach (Transform child in pageTransform)
            {
                Destroy(child.gameObject);
            }

            // Fill this page with exactly slotsPerPage slots.
            for (int i = 0; i < slotsPerPage; i++)
            {
                GameObject slot = Instantiate(inventorySlotPrefab, pageTransform);
                slot.name = "Slot " + (p * slotsPerPage + i + 1);

                InventoryItemButton btn = slot.GetComponent<InventoryItemButton>();
                int index = p * slotsPerPage + i; // Calculate the overall index in the inventory list.

                if (btn != null)
                {
                    if (index < playerInventory.items.Count)
                    {
                        var invItem = playerInventory.items[index];
                        // Set the item data and quantity if we have one.
                        btn.SetItem(invItem.itemData, invItem.quantity);
                    }
                    else
                    {
                        // No item here – an empty slot.
                        btn.SetItem(null, 0);
                    }
                }
            }

            // Force the layout to rebuild so that the grid looks pretty.
            RectTransform pageRect = pageTransform.GetComponent<RectTransform>();
            if (pageRect != null)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(pageRect);
            }
        }
    }
}
