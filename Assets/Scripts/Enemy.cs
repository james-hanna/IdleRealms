using System.Collections.Generic;
using UnityEngine;

// This script defines an enemy – the poor bastard who gets smacked around by the player.
public class Enemy : MonoBehaviour
{
    [Header("Health & XP")]
    public int maxHealth = 100;       // Maximum health for this sucker.
    public int currentHealth;         // Tracks current health.
    public int xpReward = 50;         // XP the player gets for offing him

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
        Debug.Log($"{gameObject.name} took {damage} damage. Remaining health: {currentHealth}");
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
                Instantiate(entry.dropPrefab, transform.position, Quaternion.identity);
                // Spawn the drop if luck is on your side.
            }
        }

        // Notify the spawner that this enemy is toast, so it can respawn one if needed.
        if (spawner != null)
        {
            spawner.OnEnemyDeath();
        }

        Destroy(gameObject); // Finally, destroy this enemy from the scene. RIP.
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // When colliding with the player, the enemy deals some damage.
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStats playerStats = collision.gameObject.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                int damageToPlayer = 10; // The damage dealt to the player on collision.
                playerStats.TakeDamage(damageToPlayer);
            }
        }
    }
}
