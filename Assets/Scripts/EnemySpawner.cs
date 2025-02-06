using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [Tooltip("Enemy prefab to spawn.")]
    public GameObject enemyPrefab;
    [Tooltip("Time (in seconds) before respawning an enemy.")]
    public float respawnDelay = 5f;
    
    [Tooltip("Optional spawn point. If not assigned, the spawner's position is used.")]
    public Transform spawnPoint;

    public void OnEnemyDeath()
    {
        StartCoroutine(Respawn());
    }
    

IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnDelay);
        
        if (enemyPrefab != null)
        {
            Vector3 spawnPosition = (spawnPoint != null) ? spawnPoint.position : transform.position;
            Quaternion spawnRotation = (spawnPoint != null) ? spawnPoint.rotation : transform.rotation;
            GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, spawnRotation);

            // IMPORTANT: Assign this spawner reference to the newly spawned enemy.
            Enemy enemyScript = newEnemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.spawner = this;
            }
            
            Debug.Log("Spawned enemy with spawner assigned: " + enemyScript.spawner);
        }
    }
}
