using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillSlotUI : MonoBehaviour, IDropHandler
{
    public Skill assignedSkill;
    public Image iconImage;
    public Text cooldownText;

    private SkillManager skillManager;

    void Awake()
    {
        skillManager = FindFirstObjectByType<SkillManager>();
    }


    public void SetSkill(Skill newSkill)
    {
        assignedSkill = newSkill;
        if (iconImage != null && assignedSkill != null)
        {
            iconImage.sprite = assignedSkill.icon;
        }
 
        if (cooldownText != null)
        {
            cooldownText.text = "";
        }
    }

    void Update()
    {
        if (assignedSkill != null && skillManager != null && cooldownText != null)
        {
            float lastUsed = skillManager.GetLastUsedTime(assignedSkill);
            float elapsed = Time.time - lastUsed;
            float remaining = assignedSkill.cooldown - elapsed;
            
            if (remaining > 0)
            {
                cooldownText.text = remaining.ToString("F1"); 
            }
            else
            {
                cooldownText.text = "";
            }
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Skill slot drop event");
    }
}
