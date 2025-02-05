using UnityEngine;

public enum SkillType
{
    Combat,
    Woodcutting
}

[CreateAssetMenu(fileName = "NewSkill", menuName = "Skills/Skill")]
public class Skill : ScriptableObject
{
    public string skillName;
    public Sprite icon;
    public SkillType skillType;
    public float cooldown;
    public float range;
    public int damage;
    
 
    public virtual void Execute(GameObject user, Enemy target)
    {
        if (target != null)
        {
            target.TakeDamage(damage);
            Debug.Log($"{user.name} used {skillName} on {target.name} for {damage} damage.");
        }
    }
}
