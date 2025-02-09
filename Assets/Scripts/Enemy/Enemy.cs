using System.Collections.Generic;
using UnityEngine;

// This script defines an enemy – the poor bastard who gets smacked around by the player.
public class Enemy : MonoBehaviour
{
    [Header("Health & XP")]
    public int maxHealth = 100;       // Maximum health for this sucker.
    public int currentHealth;         // Tracks current health.
    public int xpReward = 50;         // XP the player gets for offing him

    public float forceMultiplier = 3f; // Adjust the multiplier to control how forcefully items are "exploded".

    [Header("Drop Table Settings")]
    public List<DropTableEntry> dropTable = new List<DropTableEntry>();
    // A list of potential drops.

    [Header("Respawn Settings")]
    [Tooltip("Reference to an EnemySpawner that will handle respawning this enemy.")]
    public EnemySpawner spawner;
    // The spawner that might bring this enemy back from the dead – or not.

    void Awake()
    {
        currentHealth = maxHealth;
        // On spawn, the enemy is as healthy as it gets.
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        //Debug.Log($"{gameObject.name} took {damage} damage. Remaining health: {currentHealth}");
        // Log the damage – because every hit is worth knowing about.

        if (DamageTextSpawner.Instance != null)
        {
            Vector3 offset = new Vector3(0, 1f, 0); // Raise the spawn position so text floats above the enemy.
            Vector3 spawnPos = transform.position + offset;
            DamageTextSpawner.Instance.ShowDamageText(spawnPos, damage);
            // Show floating damage text so you can see the pain.
        }

        if (currentHealth <= 0)
        {
            Die(); // Time to kick this bastard to the curb.
        }
    }

    void Die()
    {
        // Award XP to the player for doing the dirty work.
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerStats stats = player.GetComponent<PlayerStats>();
            if (stats != null)
            {
                stats.AddXP(xpReward);
            }
        }

        // Roll for drops from the enemy's drop table.
        foreach (DropTableEntry entry in dropTable)
            {
                if (entry.dropPrefab != null && Random.value <= entry.dropChance)
                {
                    // Instantiate the drop at the enemy's position.
                    GameObject drop = Instantiate(entry.dropPrefab, transform.position, Quaternion.identity);
                    
                    // Try to get a Rigidbody2D from the drop.
                    Rigidbody2D rb = drop.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        // Create a force vector with upward and horizontal components.
                        Vector2 force = new Vector2(Random.Range(-1f, 1f), Random.Range(1f, 2f)) * forceMultiplier;
                        
                        // Apply an impulse force to the Rigidbody2D.
                        rb.AddForce(force, ForceMode2D.Impulse);
                    }
                }
            }
        // BORING APPROACH TO LOOT DROPPING
        // foreach (DropTableEntry entry in dropTable)
        // {
        //     if (entry.dropPrefab != null && Random.value <= entry.dropChance)
        //     {
        //         // Create a small random offset for the spawn position.
        //         Vector2 randomOffset = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(0.5f, 1f));
        //         Vector2 spawnPosition = (Vector2)transform.position + randomOffset;
                
        //         Instantiate(entry.dropPrefab, spawnPosition, Quaternion.identity);
        //     }
        // }


        // Notify the spawner that this enemy is toast, so it can respawn one if needed.
        if (spawner != null)
        {
            spawner.OnEnemyDeath();
        }

        Destroy(gameObject); // Finally, destroy this enemy from the scene. RIP.
    }

    // private void OnCollisionEnter2D(Collision2D collision)
    // {
    //     // When colliding with the player, the enemy deals some damage.
    //     if (collision.gameObject.CompareTag("Player"))
    //     {
    //         PlayerStats playerStats = collision.gameObject.GetComponent<PlayerStats>();
    //         if (playerStats != null)
    //         {
    //             int damageToPlayer = 10; // The damage dealt to the player on collision.
    //             playerStats.TakeDamage(damageToPlayer);
    //             Debug.Log("Player has taken" + damageToPlayer + "damage!");
    //         }
    //     }
    // }
}
