using UnityEngine;
using System.Collections.Generic;

public class SkillManager : MonoBehaviour
{
    [Header("Action Bar Skills (in priority order)")]
    public List<Skill> actionBarSkills;
    
    // Dictionary to store when each skill was last used.
    private Dictionary<Skill, float> lastUsedTime = new Dictionary<Skill, float>();
    
    void Awake()
    {
        // Initialize the cooldown timer for each skill.
        foreach (var skill in actionBarSkills)
        {
            lastUsedTime[skill] = -Mathf.Infinity;
        }
    }
    
    /// <summary>
    /// Iterates through the skills (by priority) and uses the first that is off cooldown and for which the enemy is in range.
    /// (This method is kept for backwards compatibility but is not used in our revised CombatController.)
    /// </summary>
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
    
    /// <summary>
    /// Attempts to use the given skill on the target if it is within range.
    /// </summary>
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
    
    /// <summary>
    /// Returns a candidate skill for the target:
    /// - If a skill is off cooldown and the enemy is in range, returns that immediately.
    /// - If none are in range but at least one skill is off cooldown, returns the first one found.
    /// - If no skills are off cooldown, returns null.
    /// </summary>
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
                    // This ready skill can hit the target.
                    return skill;
                }
            }
        }
        return candidate;
    }
}
