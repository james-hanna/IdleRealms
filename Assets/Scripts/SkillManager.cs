using UnityEngine;
using System.Collections.Generic;

public class SkillManager : MonoBehaviour
{
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
                UseSkill(skill, target);
                break;
            }
        }
    }
    
    public bool CanUseSkill(Skill skill)
    {
        return Time.time - lastUsedTime[skill] >= skill.cooldown;
    }
    
    public void UseSkill(Skill skill, Enemy target)
    {
        if(target == null)
        {
            Debug.Log("No target available for skill usage.");
            return;
        }
        
        if (CanUseSkill(skill))
        {
            float distance = Vector2.Distance(transform.position, target.transform.position);
            if(distance <= skill.range)
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
}
