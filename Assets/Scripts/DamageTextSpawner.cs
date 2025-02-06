using UnityEngine;

public class DamageTextSpawner : MonoBehaviour
{
    public static DamageTextSpawner Instance;
    
    [Tooltip("Floating Damage Text prefab reference.")]
    public GameObject damageTextPrefab;
    public Canvas uiCanvas; 

    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    

public void ShowDamageText(Vector3 worldPosition, int damage)
{
    if (damageTextPrefab != null && uiCanvas != null)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPosition);
        
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
