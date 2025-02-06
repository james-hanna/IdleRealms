using UnityEngine;

public class Coin : MonoBehaviour
{
    [Tooltip("The value of this coin when picked up.")]
    public int coinValue = 1;

    // This method is called when another collider enters this trigger collider.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object is tagged as "Player"
        if (collision.CompareTag("Player"))
        {
            // Try to get the PlayerStats component from the player.
            PlayerStats playerStats = collision.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                // Add the coin value to the player's money.
                playerStats.AddMoney(coinValue);
            }

            // Optionally, you could play a pickup sound or particle effect here.

            // Destroy the coin after pickup.
            Destroy(gameObject);
        }
    }
}
