using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    [Header("Patrol Settings")]
    [Tooltip("Minimum speed at which the enemy patrols.")]
    public float minSpeed = 1f;
    [Tooltip("Maximum speed at which the enemy patrols.")]
    public float maxSpeed = 3f;
    private float speed;  // Current patrol speed

    [Tooltip("Minimum horizontal distance to patrol from the starting position.")]
    public float minPatrolDistance = 2f;
    [Tooltip("Maximum horizontal distance to patrol from the starting position.")]
    public float maxPatrolDistance = 5f;
    private float patrolDistance;  // Current patrol distance

    [Tooltip("Idle time at each patrol turn.")]
    public float idleTime = 1f;

    // The enemy’s starting position for the current patrol leg.
    private Vector2 startingPosition;
    // Direction of movement: 1 means right, -1 means left.
    private int direction;
    // Store the original scale for sprite flipping.
    private Vector3 originalScale;
    // Flag to prevent movement during idle.
    private bool isIdle = false;

    void Start()
    {
        startingPosition = transform.position;
        originalScale = transform.localScale;
        // Randomize initial parameters:
        patrolDistance = Random.Range(minPatrolDistance, maxPatrolDistance);
        speed = Random.Range(minSpeed, maxSpeed);
        direction = (Random.value > 0.5f) ? 1 : -1;
        // Set the sprite to face the correct initial direction.
        SetSpriteDirection();
    }

    void Update()
    {
        if (isIdle)
            return;

        // Move the enemy horizontally.
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);

        // Determine how far we've moved from the starting point.
        float distanceFromStart = Mathf.Abs(transform.position.x - startingPosition.x);

        // When the patrol distance is reached, start the idle/turn routine.
        if (distanceFromStart >= patrolDistance)
        {
            StartCoroutine(IdleAndTurn());
        }
    }

    // Coroutine to handle idle time and turning.
    IEnumerator IdleAndTurn()
    {
        isIdle = true;
        yield return new WaitForSeconds(idleTime);

        // Reverse the direction.
        direction *= -1;
        FlipSprite();

        // Reset the starting point and randomize the parameters for the next patrol leg.
        startingPosition = transform.position;
        patrolDistance = Random.Range(minPatrolDistance, maxPatrolDistance);
        speed = Random.Range(minSpeed, maxSpeed);

        isIdle = false;
    }

    // Flips the enemy's sprite along the X axis.
    private void FlipSprite()
    {
        Vector3 newScale = transform.localScale;
        newScale.x = Mathf.Abs(newScale.x) * direction;
        transform.localScale = newScale;
    }

    // Ensures the sprite faces the initial movement direction.
    private void SetSpriteDirection()
    {
        Vector3 newScale = transform.localScale;
        newScale.x = Mathf.Abs(newScale.x) * direction;
        transform.localScale = newScale;
    }
}
