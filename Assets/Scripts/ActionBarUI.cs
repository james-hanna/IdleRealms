using UnityEngine;

public class ActionBarUI : MonoBehaviour
{
    public SkillManager skillManager;
    public GameObject skillSlotPrefab;
    public Transform skillSlotsParent;
    
    void Start()
    {
        if(skillManager != null && skillSlotsParent.childCount == 0)
        {
            foreach(var skill in skillManager.actionBarSkills)
            {
                GameObject slotObj = Instantiate(skillSlotPrefab, skillSlotsParent);
                SkillSlotUI slotUI = slotObj.GetComponent<SkillSlotUI>();
                if(slotUI != null)
                {
                    slotUI.SetSkill(skill);
                }
            }
        }
    }
}
