using UnityEngine;

public class PathfindingAgent : MonoBehaviour
{
    public float stoppingDistance = 0.1f;
    public float pathfindingSpeed = 5f;
    private Vector2 destination;
    private bool hasDestination = false;
    
    public void SetDestination(Vector2 dest)
    {
        destination = dest;
        hasDestination = true;
    }
    
    public bool HasReachedDestination()
    {
        return Vector2.Distance(transform.position, destination) < stoppingDistance;
    }
    
    public void MoveTowards(Vector2 target)
    {
        if (!hasDestination) return;
        Vector2 currentPosition = transform.position;
        transform.position = Vector2.MoveTowards(currentPosition, target, pathfindingSpeed * Time.fixedDeltaTime);
        if (Vector2.Distance(transform.position, target) < stoppingDistance)
        {
            hasDestination = false;
        }
    }
}
