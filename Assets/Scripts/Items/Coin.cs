using UnityEngine;

public class Coin : MonoBehaviour
{
    [Tooltip("The value of this coin when picked up.")]
    public int coinValue = 1;


    private void OnTriggerEnter2D(Collider2D collision)
    {
  
        if (collision.CompareTag("Player"))
        {

            PlayerStats playerStats = collision.GetComponent<PlayerStats>();
            if (playerStats != null)
            {

                playerStats.AddMoney(coinValue);
            }


            Destroy(gameObject);
        }
    }
}
