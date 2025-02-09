using System.Collections;
using UnityEngine;

// This little bastard spawns enemies around the spawn point.
// It makes sure there's always a set number of enemies in the scene,
// and when one dies, it waits a bit before bringing in a replacement.
public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [Tooltip("Enemy prefab to spawn.")]
    public GameObject enemyPrefab;  // The enemy prefab we’re gonna be slapping into the scene.
    
    [Tooltip("Time (in seconds) before respawning an enemy.")]
    public float respawnDelay = 5f;   // How long to wait after an enemy dies before respawning another.
    
    [Tooltip("Total number of enemies to maintain in the scene.")]
    public int spawnCount = 6;        // Total enemies you want floating around.
    
    [Tooltip("Radius within which to spawn enemies around the spawn point.")]
    public float spawnRadius = 5f;    // Enemies will pop up randomly within this horizontal radius.

    [Tooltip("Optional spawn point. If not assigned, the spawner's position is used.")]
    public Transform spawnPoint;      // If you want to be fancy and set a custom spawn location.

    // Track the current number of enemies spawned.
    private int currentEnemyCount = 0;

    void Start()
    {
        // Spawn the initial batch of enemies. Let's get the party started!
        for (int i = 0; i < spawnCount; i++)
        {
            SpawnEnemy();
        }
    }

    /// Call this method when one of the spawned enemies dies.
    /// It will decrement the current count and start a timer to spawn a replacement.
    public void OnEnemyDeath()
    {
        currentEnemyCount--; // One less enemy
        StartCoroutine(Respawn()); // Kick off the respawn timer.
    }

    IEnumerator Respawn()
    {
        // Chill for a bit before respawning.
        yield return new WaitForSeconds(respawnDelay);
        SpawnEnemy();
    }

    /// Spawns one enemy at a random position within the spawn radius.
    void SpawnEnemy()
    {
        // Determine the base position (either the spawn point or the spawner's own position).
        Vector3 basePosition = spawnPoint != null ? spawnPoint.position : transform.position;
        
        // Generate a random offset within the specified radius.
        float randomX = Random.Range(-spawnRadius, spawnRadius);
        Vector3 spawnPosition = basePosition + new Vector3(randomX, 0, 0);

        // Use the spawn point's rotation if available, otherwise use the spawner's rotation.
        Quaternion spawnRotation = spawnPoint != null ? spawnPoint.rotation : transform.rotation;
        
        // Instantiate the enemy prefab at the calculated position.
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, spawnRotation);
        
        // Hook the enemy up with its spawner so it can notify us when it dies.
        Enemy enemyScript = newEnemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.spawner = this;
        }
        
        currentEnemyCount++; // Increase the enemy count because we just spawned one.
        Debug.Log("Spawned enemy. Current enemy count: " + currentEnemyCount);
    }
}
