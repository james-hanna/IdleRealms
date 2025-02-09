// DropTableEntry.cs
using UnityEngine;

[System.Serializable]
public class DropTableEntry
{
    [Tooltip("Reference to the item that will be dropped.")]
    public ItemData itemData;
    
    [Tooltip("The prefab to instantiate as a drop. It should have an ItemPickup script attached.")]
    public GameObject dropPrefab;
    
    [Tooltip("Chance to drop this item (0 to 1).")]
    [Range(0f, 1f)]
    public float dropChance;
}
