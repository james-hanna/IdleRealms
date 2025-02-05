using UnityEngine;

public class WoodcuttingNode : MonoBehaviour
{
    public int woodXP = 20;
    private bool isChopped = false;
    
    public void Chop(GameObject player)
    {
        if (!isChopped)
        {
            isChopped = true;
            PlayerStats stats = player.GetComponent<PlayerStats>();
            if(stats != null)
            {
                stats.AddXP(woodXP); 
            }
            Debug.Log("Tree chopped, gained woodcutting XP.");
            Destroy(gameObject);
        }
    }
}
