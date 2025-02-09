using UnityEngine;

public class EnemyDamageTrigger : MonoBehaviour
{
    [Tooltip("Damage dealt to the player when overlapping.")]
    public int damageToPlayer = 10;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Try to get the PlayerStats component from the player.
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(damageToPlayer);
                Debug.Log("Monster hits for " + damageToPlayer + "damage!");

            }
        }
    }
}
