using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    
    // Input for WASD movement
    private Vector2 moveInput;
    private Rigidbody2D rb;

    // Click-to-move variables
    [HideInInspector]
    public bool isClickToMoveActive = false;
    [HideInInspector]
    public Vector2 targetPosition;

    // Reference to the pathfinding agent
    private PathfindingAgent pathfindingAgent;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pathfindingAgent = GetComponent<PathfindingAgent>();
    }

    void Update()
    {
        HandleInput();
    }

    void FixedUpdate()
    {
        if (isClickToMoveActive)
        {
            if (pathfindingAgent != null)
            {
                if (pathfindingAgent.HasReachedDestination())
                {
                    isClickToMoveActive = false;
                }
                else
                {
                    pathfindingAgent.MoveTowards(targetPosition);
                }
            }
        }
        else
        {
            rb.linearVelocity = moveInput * moveSpeed;
        }
    }

    void HandleInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(horizontal, vertical).normalized;
        
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            SetClickToMoveTarget(mouseWorldPos);
        }
    }
    
    public void SetClickToMoveTarget(Vector2 target)
    {
        isClickToMoveActive = true;
        targetPosition = target;
        if(pathfindingAgent != null)
        {
            pathfindingAgent.SetDestination(target);
        }
    }
}
