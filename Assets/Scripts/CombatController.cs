using UnityEngine;
using System.Collections;

public class CombatController : MonoBehaviour
{
    public bool autoAttackEnabled = true;
    public float autoAttackCheckInterval = 0.5f;
    public float attackRange = 2f;
    public LayerMask enemyLayer;
    
    private Transform currentTarget;
    private SkillManager skillManager;
    private PlayerController playerController;
    
    void Awake()
    {
        skillManager = GetComponent<SkillManager>();
        playerController = GetComponent<PlayerController>();
    }
    
    void Start()
    {
        StartCoroutine(AutoAttackRoutine());
    }

    void Update()
{
    // Toggle auto attack on/off with the T key
    if (Input.GetKeyDown(KeyCode.T))
    {
        autoAttackEnabled = !autoAttackEnabled;
        Debug.Log("Auto Attack toggled: " + autoAttackEnabled);
    }
}

    
IEnumerator AutoAttackRoutine()
{
    while (true)
    {
        if (autoAttackEnabled)
        {
            currentTarget = FindNearestEnemy();
            
            if (currentTarget != null)
            {
                float distance = Vector2.Distance(transform.position, currentTarget.position);
                if (distance > attackRange)
                {
                    if (playerController != null)
                    {
                        playerController.SetClickToMoveTarget(currentTarget.position);
                    }
                }
                else
                {
                    if (skillManager != null)
                    {
                        Enemy enemyComponent = currentTarget.GetComponent<Enemy>();
                        if (enemyComponent != null)
                        {
                            skillManager.UsePrioritySkill(enemyComponent);
                        }
                    }
                }
            }
        }
        yield return new WaitForSeconds(autoAttackCheckInterval);
    }
}

    Transform FindNearestEnemy()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 10f, enemyLayer);
        Transform nearest = null;
        float minDistance = Mathf.Infinity;
        foreach(Collider2D hit in hits)
        {
            float dist = Vector2.Distance(transform.position, hit.transform.position);
            if(dist < minDistance)
            {
                minDistance = dist;
                nearest = hit.transform;
            }
        }
        return nearest;
    }
    
    public void SetTarget(Transform enemy)
    {
        currentTarget = enemy;
    }
}
