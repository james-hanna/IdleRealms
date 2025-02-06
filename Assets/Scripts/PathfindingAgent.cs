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

    /// <summary>
    /// Sets a new destination for the agent.
    /// </summary>
    /// <param name="dest">The target position.</param>
    public void SetDestination(Vector2 dest)
    {
        destination = dest;
        hasDestination = true;
    }
    
    /// <summary>
    /// Call this method to cancel the current destination.
    /// </summary>
    public void ResetDestination()
    {
        hasDestination = false;
    }
    
    /// <summary>
    /// Checks whether the agent is close enough to the destination.
    /// </summary>
    /// <returns>True if the agent is within stoppingDistance of the destination.</returns>
    public bool HasReachedDestination()
    {
        return Vector2.Distance(rb.position, destination) < stoppingDistance;
    }
    
    /// <summary>
    /// Moves the agent towards the specified target using Rigidbody2D.MovePosition.
    /// </summary>
    /// <param name="target">The target position to move towards.</param>
    public void MoveTowards(Vector2 target)
    {
        if (!hasDestination)
            return;
        
        Vector2 newPosition = Vector2.MoveTowards(rb.position, target, pathfindingSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPosition);
        
        // If we are within stoppingDistance, cancel further movement.
        if (Vector2.Distance(newPosition, target) < stoppingDistance)
        {
            hasDestination = false;
        }
    }
}
