using UnityEngine;

// This script handles the player's movement using both keyboard input and click-to-move.
// When you click somewhere, it uses the PathfindingAgent to move the player.
// But if you start using the keyboard, click-to-move gets canceled.
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PathfindingAgent))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;  // How fast the player moves with keyboard input.

    private Rigidbody2D rb;               // Cached Rigidbody2D component.
    private PathfindingAgent pathfindingAgent;  // The agent that handles click-to-move.
    private Camera mainCamera;            // Main camera for converting screen to world coordinates.

    // Input
    private Vector2 moveInput;            // Keyboard input vector.

    // Click-to-move variables
    [HideInInspector]
    public bool isClickToMoveActive = false;  // Flag to know if we're in click-to-move mode.
    [HideInInspector]
    public Vector2 targetPosition;        // The target position from the mouse click.

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pathfindingAgent = GetComponent<PathfindingAgent>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        HandleInput();  // Check for keyboard and mouse inputs.
    }

    void FixedUpdate()
    {
        // If keyboard input is detected, cancel click-to-move mode.
        if (moveInput.sqrMagnitude > 0.01f)
        {
            if (isClickToMoveActive)
            {
                isClickToMoveActive = false;
                pathfindingAgent.ResetDestination();  // Stop the click-to-move routine.
            }
        }

        // If click-to-move is active, let the agent do its thing.
        if (isClickToMoveActive)
        {
            if (pathfindingAgent.HasReachedDestination())
            {
                isClickToMoveActive = false;  // We reached the target, so turn off click-to-move.
            }
            else
            {
                pathfindingAgent.MoveTowards(targetPosition); // Keep moving toward the target.
            }
        }
        else
        {
            // Regular keyboard movement.
            Vector2 newPosition = rb.position + moveInput * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(newPosition);
        }
    }

    // Handle both keyboard and mouse inputs.
    void HandleInput()
    {
        // Get normalized keyboard input.
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(horizontal, vertical).normalized;
        
        // Check for mouse click to set a new destination.
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            SetClickToMoveTarget(mouseWorldPos);
        }
    }
    
    // Set up click-to-move by providing a target position.
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
