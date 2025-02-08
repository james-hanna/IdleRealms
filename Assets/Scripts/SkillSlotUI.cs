using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// This script manages the UI for an individual skill slot in the action bar.
// It shows the skill's icon and the cooldown timer. Also handles drag-and-drop if needed.
public class SkillSlotUI : MonoBehaviour, IDropHandler
{
    public Skill assignedSkill;  // The skill assigned to this slot.
    public Image iconImage;      // The UI image to display the skill icon.
    public Text cooldownText;    // Text UI element to show the cooldown countdown.

    private SkillManager skillManager;  // Reference to the SkillManager.

    void Awake()
    {
        // Find the SkillManager in the scene – assumes there's only one.
        skillManager = FindFirstObjectByType<SkillManager>();
    }

    // Assign a new skill to this slot and update the icon.
    public void SetSkill(Skill newSkill)
    {
        assignedSkill = newSkill;
        if (iconImage != null && assignedSkill != null)
        {
            iconImage.sprite = assignedSkill.icon;
        }
 
        if (cooldownText != null)
        {
            cooldownText.text = "";  // Clear any cooldown text initially.
        }
    }

    void Update()
    {
        // Update the cooldown timer if a skill is assigned.
        if (assignedSkill != null && skillManager != null && cooldownText != null)
        {
            float lastUsed = skillManager.GetLastUsedTime(assignedSkill);
            float elapsed = Time.time - lastUsed;
            float remaining = assignedSkill.cooldown - elapsed;
            
            if (remaining > 0)
            {
                cooldownText.text = remaining.ToString("F1");  // Show remaining time with 1 decimal place.
            }
            else
            {
                cooldownText.text = "";  // No cooldown, so clear the text.
            }
        }
    }

    // Handle drop events (for drag-and-drop functionality).
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Skill slot drop event");
        // TODO: Implement drag-and-drop functionality
    }
}
