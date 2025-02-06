using UnityEngine;
using System.Collections.Generic;

public class SkillManager : MonoBehaviour
{
    [Header("Action Bar Skills (in priority order)")]
    public List<Skill> actionBarSkills;
    
    private Dictionary<Skill, float> lastUsedTime = new Dictionary<Skill, float>();
    
    void Awake()
    {

        foreach (var skill in actionBarSkills)
        {
            lastUsedTime[skill] = -Mathf.Infinity;
        }
    }
    

    public void UsePrioritySkill(Enemy target)
    {
        foreach(var skill in actionBarSkills)
        {
            if (CanUseSkill(skill))
            {
                float distance = Vector2.Distance(transform.position, target.transform.position);
                if (distance <= skill.range)
                {
                    UseSkill(skill, target);
                    return;
                }
            }
        }
        Debug.Log("No available or in-range skill for target: " + target.name);
    }
    
    public bool CanUseSkill(Skill skill)
    {
        return Time.time - lastUsedTime[skill] >= skill.cooldown;
    }
    

    public void UseSkill(Skill skill, Enemy target)
    {
        if (target == null)
        {
            Debug.Log("No target available for skill usage.");
            return;
        }
        
        if (CanUseSkill(skill))
        {
            float distance = Vector2.Distance(transform.position, target.transform.position);
            if (distance <= skill.range)
            {
                skill.Execute(gameObject, target);
                lastUsedTime[skill] = Time.time;
            }
            else
            {
                Debug.Log("Target out of range for " + skill.skillName);
            }
        }
        else
        {
            Debug.Log($"{skill.skillName} is still on cooldown.");
        }
    }
    

    public Skill GetCandidateSkillForTarget(Enemy target)
    {
        Skill candidate = null;
        foreach (Skill skill in actionBarSkills)
        {
            if (CanUseSkill(skill))
            {
                if (candidate == null)
                    candidate = skill;
                
                float distance = Vector2.Distance(transform.position, target.transform.position);
                if (distance <= skill.range)
                {

                    return skill;
                }
            }
        }
        return candidate;
    }
}
