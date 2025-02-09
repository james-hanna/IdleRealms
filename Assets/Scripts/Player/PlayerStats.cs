using UnityEngine;

// This script tracks the player's stats like health, XP, money, and defense.
// It also handles leveling up and taking damage
public class PlayerStats : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    
    public int level = 1;
    public int currentXP = 0;
    public int xpToNextLevel = 100;
    public int money = 0;
    
    public int defense = 5;  // Reduces incoming damage. Because why take damage if you can mitigate it?

    void Awake()
    {
        currentHealth = maxHealth;  // Start with full health.
    }
    
    // Add XP and check for level up.
    public void AddXP(int xp)
    {
        currentXP += xp;
        Debug.Log("Gained XP: " + xp + ". Total XP: " + currentXP);
        CheckLevelUp();
    }
    
    // Keep leveling up until currentXP is less than required XP.
    void CheckLevelUp()
    {
        while(currentXP >= xpToNextLevel)
        {
            currentXP -= xpToNextLevel;
            LevelUp();
        }
    }

    // Add money (coins) to the player's stash.
    public void AddMoney(int amount)
    {
        money += amount;
        Debug.Log($"Player picked up {amount} coin(s). Total money: {money}");
    }
    
    // Handle leveling up the player.
    void LevelUp()
    {
        level++;
        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * 1.5f); // Increase XP requirement.
        Debug.Log("Level Up! New level: " + level);
    }
    
    // Damage handling – takes into account defense.
    public void TakeDamage(int damage)
    {
        int effectiveDamage = Mathf.Max(0, damage - defense);
        currentHealth -= effectiveDamage;
        Debug.Log("Player's defense reduces damage to " + effectiveDamage + ". Remaining health: " + currentHealth);
        if(currentHealth <= 0)
        {
            Die();
        }
    }
    
    // Called when the player's health drops to zero.
    void Die()
    {
        Debug.Log("Player has died.");
        // TODO: Add death handling logic
    }
}
