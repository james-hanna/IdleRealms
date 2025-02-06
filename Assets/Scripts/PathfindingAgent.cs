using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PathfindingAgent : MonoBehaviour
{
    [Header("Pathfinding Settings")]
    public float stoppingDistance = 0.1f;
    public float pathfindingSpeed = 5f;

    private Vector2 destination;
    private bool hasDestination = false;
    
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    public void SetDestination(Vector2 dest)
    {
        destination = dest;
        hasDestination = true;
    }
    

    public void ResetDestination()
    {
        hasDestination = false;
    }

    public bool HasReachedDestination()
    {
        return Vector2.Distance(rb.position, destination) < stoppingDistance;
    }
    
  
    public void MoveTowards(Vector2 target)
    {
        if (!hasDestination)
            return;
        
        Vector2 newPosition = Vector2.MoveTowards(rb.position, target, pathfindingSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPosition);
        

        if (Vector2.Distance(newPosition, target) < stoppingDistance)
        {
            hasDestination = false;
        }
    }
}
