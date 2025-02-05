using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillSlotUI : MonoBehaviour, IDropHandler
{
    public Skill assignedSkill;
    public Image iconImage;
    public Text cooldownText;
    
    public void SetSkill(Skill newSkill)
    {
        assignedSkill = newSkill;
        if(iconImage != null && assignedSkill != null)
        {
            iconImage.sprite = assignedSkill.icon;
            cooldownText.text = cooldownText.ToString();
        }
    }
    
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Skill slot drop event");
    }
    
}
