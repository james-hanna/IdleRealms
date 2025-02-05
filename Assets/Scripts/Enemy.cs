using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public int xpReward = 50;
    
    void Awake()
    {
        currentHealth = maxHealth;
    }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. Remaining health: {currentHealth}");
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    void Die()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)
        {
            PlayerStats stats = player.GetComponent<PlayerStats>();
            if(stats != null)
            {
                stats.AddXP(xpReward);
            }
        }
        Destroy(gameObject);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            PlayerStats playerStats = collision.gameObject.GetComponent<PlayerStats>();
            if(playerStats != null)
            {
                int damageToPlayer = 10;
                playerStats.TakeDamage(damageToPlayer);
            }
        }
    }
}
