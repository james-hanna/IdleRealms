using UnityEngine;
using System.Collections;

public class CombatController : MonoBehaviour
{
    [Header("Auto Attack Settings")]
    public bool autoAttackEnabled = true;
    public float autoAttackCheckInterval = 0.5f;
    
    [Header("Enemy Detection Settings")]
    [Tooltip("The radius in which to search for enemies.")]
    public float enemyDetectionRadius = 10f;
    public LayerMask enemyLayer;
    
    [Header("Melee (Auto Attack) Settings")]
    [Tooltip("Range within which auto attacks can hit.")]
    public float attackRange = 2f;
    [Tooltip("Damage dealt by an auto attack.")]
    public float autoAttackDamage = 10f;
    [Tooltip("Cooldown between auto attacks.")]
    public float autoAttackCooldown = 1f;
    
    private float lastAutoAttackTime = -Mathf.Infinity;
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
                    Enemy enemyComponent = currentTarget.GetComponent<Enemy>();
                    if (enemyComponent != null)
                    {
                        float distance = Vector2.Distance(transform.position, currentTarget.position);
                        Skill candidateSkill = skillManager.GetCandidateSkillForTarget(enemyComponent);
                        
                        if (candidateSkill != null)
                        {
                            // At least one skill is off cooldown.
                            if (distance > candidateSkill.range)
                            {
                                // Move closer until the skill can hit the enemy.
                                playerController.SetClickToMoveTarget(currentTarget.position);
                            }
                            else
                            {
                                // Enemy is within skill’s range, so use it.
                                skillManager.UseSkill(candidateSkill, enemyComponent);
                            }
                        }
                        else
                        {
                            // No skills are off cooldown, so perform an auto attack.
                            if (distance > attackRange)
                            {
                                // Move closer until within auto attack range.
                                playerController.SetClickToMoveTarget(currentTarget.position);
                            }
                            else
                            {
                                AutoAttack(enemyComponent);
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
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, enemyDetectionRadius, enemyLayer);
        Transform nearest = null;
        float minDistance = Mathf.Infinity;
        foreach (Collider2D hit in hits)
        {
            float dist = Vector2.Distance(transform.position, hit.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                nearest = hit.transform;
            }
        }
        return nearest;
    }
    

    void AutoAttack(Enemy enemy)
    {
        if (Time.time - lastAutoAttackTime >= autoAttackCooldown)
        {
            enemy.TakeDamage((int)autoAttackDamage);
            lastAutoAttackTime = Time.time;
            Debug.Log($"{gameObject.name} auto attacked {enemy.name} for {autoAttackDamage} damage.");
        }
    }
    
    public void SetTarget(Transform enemy)
    {
        currentTarget = enemy;
    }
}
