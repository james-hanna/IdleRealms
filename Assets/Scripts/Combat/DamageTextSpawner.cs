using UnityEngine;

// This script is here to spawn floating damage text whenever someone gets their ass kicked.
// It uses a singleton pattern because, frankly, we only need one of these bad boys.
public class DamageTextSpawner : MonoBehaviour
{
    public static DamageTextSpawner Instance;   // The instance of this spawner.
    
    [Tooltip("Floating Damage Text prefab reference.")]
    public GameObject damageTextPrefab;           // The prefab for the floating damage text. Don’t forget to assign this!
    public Canvas uiCanvas;                       // The UI canvas where this floating crap is going to be displayed.

    void Awake()
    {
        // Ensure we only have one instance of this spawner. If there’s another one, fuck it and destroy it.
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    // Call this function to show damage text at a specific world position.
    public void ShowDamageText(Vector3 worldPosition, int damage)
    {
        if (damageTextPrefab != null && uiCanvas != null)
        {
            // Convert the world position (where the enemy got smacked) to a screen position for the UI.
            Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPosition);
            
            // Instantiate the damage text prefab as a child of the UI canvas.
            GameObject dmgTextInstance = Instantiate(damageTextPrefab, screenPos, Quaternion.identity, uiCanvas.transform);
            
            // Get the FloatingDamageText component to set the displayed damage.
            FloatingDamageText floatingText = dmgTextInstance.GetComponent<FloatingDamageText>();
            if (floatingText != null)
            {
                // Pass in the damage value as a string.
                floatingText.Setup(damage.ToString());
            }
        }
        else
        {
            Debug.LogError("DamageTextSpawner: damageTextPrefab or uiCanvas is not assigned!");
        }
    }
}
