using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Inventory playerInventory;
    public GameObject slotPrefab;
    public Transform slotsParent;
    
    void Start()
    {
            Debug.Log("InventoryUI starting...");

        if(playerInventory != null && slotsParent.childCount == 0)
        {
            for (int i = 0; i < playerInventory.inventorySize; i++)
            {
                GameObject slotObj = Instantiate(slotPrefab, slotsParent);
                InventorySlot slot = slotObj.GetComponent<InventorySlot>();
                playerInventory.slots.Add(slot);
            }
        }
    }
    
}
