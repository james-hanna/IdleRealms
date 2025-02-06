using UnityEngine;

public class DamageTextSpawner : MonoBehaviour
{
    public static DamageTextSpawner Instance;
    
    [Tooltip("Floating Damage Text prefab reference.")]
    public GameObject damageTextPrefab;
    public Canvas uiCanvas; // Assign this in the Inspector (the Canvas you want the damage text to appear on)

    
    void Awake()
    {
        // Simple singleton pattern so we can easily access this from anywhere.
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    /// <summary>
    /// Instantiates a floating damage text at the given world position.
    /// </summary>
public void ShowDamageText(Vector3 worldPosition, int damage)
{
    if (damageTextPrefab != null && uiCanvas != null)
    {
        // Convert world position to screen position if your canvas is in Screen Space
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPosition);
        
        // Instantiate as a child of the UI Canvas
        GameObject dmgTextInstance = Instantiate(damageTextPrefab, screenPos, Quaternion.identity, uiCanvas.transform);
        
        FloatingDamageText floatingText = dmgTextInstance.GetComponent<FloatingDamageText>();
        if (floatingText != null)
        {
            floatingText.Setup(damage.ToString());
        }
    }
    else
    {
        Debug.LogError("DamageTextSpawner: damageTextPrefab or uiCanvas is not assigned!");
    }
}

}
