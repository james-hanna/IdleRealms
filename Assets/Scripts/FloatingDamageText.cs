using UnityEngine;
using UnityEngine.UI;

public class FloatingDamageText : MonoBehaviour
{
    [Header("Animation Settings")]
    [Tooltip("How fast the text moves upward (in canvas units per second).")]
    public float moveSpeed = 20f;
    [Tooltip("How long (in seconds) before the text completely fades out.")]
    public float fadeDuration = 1f;
    
    private Text damageText;
    private float elapsedTime = 0f;
    private Color originalColor;
    private RectTransform rectTransform;

    void Awake()
    {
        damageText = GetComponent<Text>();
        rectTransform = GetComponent<RectTransform>();
        if (damageText != null)
        {
            originalColor = damageText.color;
        }
        else
        {
            Debug.LogError("FloatingDamageText: No Text component found!");
        }
    }

    public void Setup(string damageValue)
    {
        if (damageText != null)
        {
            damageText.text = damageValue;
        }
    }

    void Update()
    {

        if (rectTransform != null)
        {
            rectTransform.anchoredPosition += Vector2.up * moveSpeed * Time.deltaTime;
        }


        elapsedTime += Time.deltaTime;
        if (damageText != null)
        {
            float alpha = Mathf.Lerp(originalColor.a, 0, elapsedTime / fadeDuration);
            damageText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
        }

        if (elapsedTime >= fadeDuration)
        {
            Destroy(gameObject);
        }
    }
}
