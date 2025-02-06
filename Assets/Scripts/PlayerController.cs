using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PathfindingAgent))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    // Cached components
    private Rigidbody2D rb;
    private PathfindingAgent pathfindingAgent;
    private Camera mainCamera;

    // Input
    private Vector2 moveInput;

    // Click-to-move variables
    [HideInInspector]
    public bool isClickToMoveActive = false;
    [HideInInspector]
    public Vector2 targetPosition;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pathfindingAgent = GetComponent<PathfindingAgent>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        HandleInput();
    }

    void FixedUpdate()
    {
        // If keyboard input is detected, cancel click-to-move.
        if (moveInput.sqrMagnitude > 0.01f)
        {
            if (isClickToMoveActive)
            {
                isClickToMoveActive = false;
                pathfindingAgent.ResetDestination();
            }
        }

        if (isClickToMoveActive)
        {
            // Check if we have reached the destination.
            if (pathfindingAgent.HasReachedDestination())
            {
                isClickToMoveActive = false;
            }
            else
            {
                // Let the pathfinding agent handle movement using Rigidbody2D.MovePosition.
                pathfindingAgent.MoveTowards(targetPosition);
            }
        }
        else
        {
            // Manual movement using keyboard input.
            Vector2 newPosition = rb.position + moveInput * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(newPosition);
        }
    }

    void HandleInput()
    {
        // Get keyboard input (WASD or arrow keys)
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(horizontal, vertical).normalized;
        
        // Set a new click-to-move target if the left mouse button is pressed.
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            SetClickToMoveTarget(mouseWorldPos);
        }
    }
    
    public void SetClickToMoveTarget(Vector2 target)
    {
        isClickToMoveActive = true;
        targetPosition = target;
        if (pathfindingAgent != null)
        {
            pathfindingAgent.SetDestination(target);
        }
    }
}
