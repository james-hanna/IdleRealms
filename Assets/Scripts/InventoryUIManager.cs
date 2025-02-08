using UnityEngine;


public class InventoryUIManager : MonoBehaviour
{
    public GameObject inventoryPanel;
    
    
    public void Toggle()
    {
        if ( inventoryPanel != null)
        {
            bool isActive = inventoryPanel.activeSelf;
            inventoryPanel.SetActive(!isActive);
        }
        else{
            Debug.Log("Could not interact with inventory!");
        }
    }
}
