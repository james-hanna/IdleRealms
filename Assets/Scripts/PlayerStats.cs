using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    
    public int level = 1;
    public int currentXP = 0;
    public int xpToNextLevel = 100;
    public int money = 0;
    
    public int defense = 5;
    
    void Awake()
    {
        currentHealth = maxHealth;
    }
    
    public void AddXP(int xp)
    {
        currentXP += xp;
        Debug.Log("Gained XP: " + xp + ". Total XP: " + currentXP);
        CheckLevelUp();
    }
    
    void CheckLevelUp()
    {
        while(currentXP >= xpToNextLevel)
        {
            currentXP -= xpToNextLevel;
            LevelUp();
        }
    }

        public void AddMoney(int amount)
    {
        money += amount;
        Debug.Log($"Player picked up {amount} coin(s). Total money: {money}");
    }
    
    void LevelUp()
    {
        level++;
        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * 1.5f);
        Debug.Log("Level Up! New level: " + level);
    }
    
    public void TakeDamage(int damage)
    {
        int effectiveDamage = Mathf.Max(0, damage - defense);
        currentHealth -= effectiveDamage;
        Debug.Log("Player took " + effectiveDamage + " damage. Remaining health: " + currentHealth);
        if(currentHealth <= 0)
        {
            Die();
        }
    }
    
    void Die()
    {
        Debug.Log("Player has died.");
    }
}
