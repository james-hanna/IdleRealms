using UnityEngine;

// This little dude handles simple pathfinding movement.
// It moves the object toward a destination until it's close enough,
// and then it stops. Not exactly A* but gets the job done.
[RequireComponent(typeof(Rigidbody2D))]
public class PathfindingAgent : MonoBehaviour
{
    [Header("Pathfinding Settings")]
    public float stoppingDistance = 0.1f;    // When we're this close, we consider ourselves "there".
    public float pathfindingSpeed = 5f;      // How fast the object should hustle toward its target.

    private Vector2 destination;             // The target point we want to move to.
    private bool hasDestination = false;     // Are we currently chasing a destination or just standing around?
    
    private Rigidbody2D rb;                  // Cached Rigidbody2D for smooth movement.

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();    // Grab the Rigidbody2D component – no rigid body, no movement.
    }

    // Set a new destination for the agent.
    public void SetDestination(Vector2 dest)
    {
        destination = dest;
        hasDestination = true;               // Mark that we have somewhere to go.
    }
    
    // Cancel our current destination.
    public void ResetDestination()
    {
        hasDestination = false;
    }

    // Check if we've reached our destination (within a tiny margin of error).
    public bool HasReachedDestination()
    {
        return Vector2.Distance(rb.position, destination) < stoppingDistance;
    }
    
    // Move the agent toward the given target.
    public void MoveTowards(Vector2 target)
    {
        if (!hasDestination)
            return; // Nothing to do if there's no destination.

        // Calculate the new position using MoveTowards.
        Vector2 newPosition = Vector2.MoveTowards(rb.position, target, pathfindingSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPosition);  // Move the Rigidbody2D to the new position.

        // If we're close enough to the target, stop chasing it.
        if (Vector2.Distance(newPosition, target) < stoppingDistance)
        {
            hasDestination = false;
        }
    }
}
