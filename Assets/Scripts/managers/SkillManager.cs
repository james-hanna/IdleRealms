using UnityEngine;
using System.Collections.Generic;

// This script manages skills – checking cooldowns, executing skills on targets,
// and handling the priority order for the action bar skills.
public class SkillManager : MonoBehaviour
{
    [Header("Action Bar Skills (in priority order)")]
    public List<Skill> actionBarSkills;  // List of skills sorted by priority.

    // Dictionary to track the last used time for each skill.
    private Dictionary<Skill, float> lastUsedTime = new Dictionary<Skill, float>();

    void Awake()
    {
        // Initialize all skills with a "last used" time of negative infinity so they're all ready.
        foreach (var skill in actionBarSkills)
        {
            lastUsedTime[skill] = -Mathf.Infinity;
        }
    }
    
    // Try using the first available (and in-range) skill on the target.
    public void UsePrioritySkill(Enemy target)
    {
        foreach (var skill in actionBarSkills)
        {
            if (CanUseSkill(skill))
            {
                float distance = Vector2.Distance(transform.position, target.transform.position);
                if (distance <= skill.range)
                {
                    UseSkill(skill, target);
                    return;  // Stop after using the first valid skill.
                }
            }
        }
        //Debug.Log("No available or in-range skill for target: " + target.name);
    }
    
    // Check if a skill is off cooldown and ready to use.
    public bool CanUseSkill(Skill skill)
    {
        return Time.time - lastUsedTime[skill] >= skill.cooldown;
    }
    
    // Execute the given skill on the target.
    public void UseSkill(Skill skill, Enemy target)
    {
        if (target == null)
        {
            //Debug.Log("No target available for skill usage.");
            return;
        }
        
        if (CanUseSkill(skill))
        {
            float distance = Vector2.Distance(transform.position, target.transform.position);
            if (distance <= skill.range)
            {
                // Actually execute the skill's effect.
                skill.Execute(gameObject, target);
                lastUsedTime[skill] = Time.time;
            }
            else
            {
                //Debug.Log("Target out of range for " + skill.skillName);
            }
        }
        else
        {
            //Debug.Log($"{skill.skillName} is still on cooldown.");
        }
    }
    
    // Returns a candidate skill for the given target.
    // Prioritizes skills that are off cooldown and in range.
    public Skill GetCandidateSkillForTarget(Enemy target)
    {
        Skill candidate = null;
        foreach (Skill skill in actionBarSkills)
        {
            if (CanUseSkill(skill))
            {
                if (candidate == null)
                    candidate = skill;  // Pick the first available skill as a backup.
                
                float distance = Vector2.Distance(transform.position, target.transform.position);
                if (distance <= skill.range)
                {
                    return skill;  // Return immediately if a skill is in range.
                }
            }
        }
        return candidate;
    }
    
    // Utility to get the last used time for a skill.
    public float GetLastUsedTime(Skill skill)
    {
        if (lastUsedTime.ContainsKey(skill))
        {
            return lastUsedTime[skill];
        }
        return -Mathf.Infinity;
    }
}
