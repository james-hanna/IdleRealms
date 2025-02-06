using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Health & XP")]
    public int maxHealth = 100;
    public int currentHealth;
    public int xpReward = 50;
    
    [Header("Coin Drop Settings")]
    [Tooltip("Chance to drop a coin on death (0 to 1).")]
    [Range(0f, 1f)]
    public float coinDropChance = 0.5f; // 50% chance
    [Tooltip("The coin prefab to drop on death.")]
    public GameObject coinPrefab;
    
    [Header("Respawn Settings")]
    [Tooltip("Reference to an EnemySpawner that will handle respawning this enemy.")]
    public EnemySpawner spawner;
    
    void Awake()
    {
        currentHealth = maxHealth;
    }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. Remaining health: {currentHealth}");
        
        // Show the floating damage text above the enemy.
        if (DamageTextSpawner.Instance != null)
        {
            // Add an offset so that the damage text appears above the enemy.
            Vector3 offset = new Vector3(0, 1f, 0); // Adjust the Y offset as needed.
            Vector3 spawnPos = transform.position + offset;
            DamageTextSpawner.Instance.ShowDamageText(spawnPos, damage);
        }
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    void Die()
    {
        // Award XP to the player.
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerStats stats = player.GetComponent<PlayerStats>();
            if (stats != null)
            {
                stats.AddXP(xpReward);
            }
        }
        
        // Attempt to drop a coin.
        if (coinPrefab != null && Random.value <= coinDropChance)
        {
            Instantiate(coinPrefab, transform.position, Quaternion.identity);
        }
        
        // Notify the spawner to respawn a new enemy if one is assigned.
        if (spawner != null)
        {
            spawner.OnEnemyDeath();
        }
        
        // Destroy this enemy GameObject.
        Destroy(gameObject);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStats playerStats = collision.gameObject.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                int damageToPlayer = 10;
                playerStats.TakeDamage(damageToPlayer);
            }
        }
    }
}
