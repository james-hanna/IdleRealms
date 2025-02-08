using UnityEngine;
using UnityEngine.UI;

// This script handles that cool floating damage text effect.
// It makes the text rise and fade out over time so you can see how hard you wrecked something.
public class FloatingDamageText : MonoBehaviour
{
    [Header("Animation Settings")]
    [Tooltip("How fast the text moves upward (in canvas units per second).")]
    public float moveSpeed = 150f;   // How quickly the text will shoot upward.
    
    [Tooltip("How long (in seconds) before the text completely fades out.")]
    public float fadeDuration = 1.5f; // Duration until the text vanishes into thin air.
    
    private Text damageText;        // The Text component that shows the damage number.
    private float elapsedTime = 0f; // Time since the text was spawned.
    private Color originalColor;    // To remember the starting color for fading.
    private RectTransform rectTransform; // Needed for moving the text in the UI.

    void Awake()
    {
        damageText = GetComponent<Text>();            // Grab the Text component.
        rectTransform = GetComponent<RectTransform>();  // Grab the RectTransform.
        
        if (damageText != null)
        {
            originalColor = damageText.color;           // Save the original color so we can fade from it.
        }
        else
        {
            Debug.LogError("FloatingDamageText: No Text component found!");
        }
    }

    // Call this to set up the damage value text.
    public void Setup(string damageValue)
    {
        if (damageText != null)
        {
            damageText.text = damageValue; // Set the text to the damage amount.
        }
    }

    void Update()
    {
        // Move the text upward.
        if (rectTransform != null)
        {
            rectTransform.anchoredPosition += Vector2.up * moveSpeed * Time.deltaTime;
        }

        // Increase our elapsed time counter.
        elapsedTime += Time.deltaTime;
        
        // Fade out the text over time.
        if (damageText != null)
        {
            float alpha = Mathf.Lerp(originalColor.a, 0, elapsedTime / fadeDuration);
            damageText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
        }

        // When fade duration is over, destroy the text.
        if (elapsedTime >= fadeDuration)
        {
            Destroy(gameObject);
        }
    }
}
