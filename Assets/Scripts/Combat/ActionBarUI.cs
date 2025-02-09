using UnityEngine;

// This script is responsible for setting up the damn action bar UI.
// It takes your SkillManager’s skills and creates a UI slot for each one.
// Not rocket science, but it does the job.
public class ActionBarUI : MonoBehaviour
{
    public SkillManager skillManager;         // The badass SkillManager that holds all your skills.
    public GameObject skillSlotPrefab;          // The prefab for each skill slot. Just a fancy UI element.
    public Transform skillSlotsParent;          // The parent transform where these slots are gonna hang out.

    void Start()
    {
        // Only set up the action bar if we actually have a SkillManager and
        // if there are no child slots already (i.e., we're not duplicating shit).
        if (skillManager != null && skillSlotsParent.childCount == 0)
        {
            // Loop through each skill in our action bar.
            foreach (var skill in skillManager.actionBarSkills)
            {
                // Instantiate a new skill slot from our prefab and parent it to our skillSlotsParent.
                GameObject slotObj = Instantiate(skillSlotPrefab, skillSlotsParent);
                
                // Grab the SkillSlotUI component so we can actually display some shit.
                SkillSlotUI slotUI = slotObj.GetComponent<SkillSlotUI>();
                if (slotUI != null)
                {
                    // Tell the slot what skill to show. Boom, magic!
                    slotUI.SetSkill(skill);
                }
            }
        }
    }
}
