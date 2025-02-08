using UnityEngine;

// This script makes the enemy patrol back and forth like a dumb watch guard.
public class EnemyMovement : MonoBehaviour
{
    [Header("Patrol Settings")]
    [Tooltip("Speed at which the enemy patrols.")]
    public float speed = 2f;  // How fast the enemy shuffles around.

    [Tooltip("Maximum horizontal distance to patrol from the starting position.")]
    public float patrolDistance = 3f;  // The limit before the enemy turns around.

    // Starting position of the enemy.
    private Vector2 startingPosition;  

    // Direction of movement: 1 means right, -1 means left.
    private int direction = 1;

    // Store the original scale to flip the sprite when changing direction.
    private Vector3 originalScale;

    void Start()
    {
        startingPosition = transform.position;
        // Remember where you started, you clueless enemy.
        originalScale = transform.localScale;
        // Save the original scale for proper flipping later.
    }

    void Update()
    {
        // Move the enemy horizontally based on direction, speed, and frame time.
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);

        // Calculate the distance from the starting position.
        float distanceFromStart = Mathf.Abs(transform.position.x - startingPosition.x);

        // If we've hit the patrol limit, it's time to flip.
        if (distanceFromStart >= patrolDistance)
        {
            direction *= -1; // Reverse direction.
            FlipSprite();    // Flip the sprite so it faces the new direction.
        }
    }

    /// Flips the enemy's sprite along the X axis.
    private void FlipSprite()
    {
        Vector3 newScale = transform.localScale;
        newScale.x = -newScale.x; // Invert the X scale to mirror the sprite.
        transform.localScale = newScale;
    }
}
