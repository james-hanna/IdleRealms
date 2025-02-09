using UnityEngine;
using System.Collections;

// This script handles all the combat crap – auto attacks, enemy detection, and using skills.
public class CombatController : MonoBehaviour
{
    [Header("Auto Attack Settings")]
    public bool autoAttackEnabled = true;           // Toggle auto-attack. Sometimes you want the game to do the work for you.
    public float autoAttackCheckInterval = 0.5f;      // How often we check for enemies. Too damn often.

    [Header("Enemy Detection Settings")]
    [Tooltip("The radius in which to search for enemies.")]
    public float enemyDetectionRadius = 10f;          // The range in which we’ll sniff out enemies.
    public LayerMask enemyLayer;                      // Only look for enemies, not every damn thing.

    [Header("Melee (Auto Attack) Settings")]
    [Tooltip("Range within which auto attacks can hit.")]
    public float attackRange = 2f;                    // You need to be this close to whack an enemy.
    [Tooltip("Damage dealt by an auto attack.")]
    public float autoAttackDamage = 10f;              // How much pain you dish out with each auto attack.
    [Tooltip("Cooldown between auto attacks.")]
    public float autoAttackCooldown = 1f; 

    private float lastAutoAttackTime = -Mathf.Infinity;   // Track when we last auto attacked to enforce cooldown.
    private Transform currentTarget;                      // The enemy we’re currently focusing on.
    private SkillManager skillManager;                    // Reference to our SkillManager for those fancy skills.
    private PlayerController playerController;            // Reference to the PlayerController for movement commands.

    void Awake()
    {
        // Grab our needed components right off the bat.
        skillManager = GetComponent<SkillManager>();
        playerController = GetComponent<PlayerController>();
    }
    
    void Start()
    {
        // Kick off our never-ending auto attack routine.
        StartCoroutine(AutoAttackRoutine());
    }
    
    void Update()
    {
        // Toggle auto attack on/off when you press T.
        if (Input.GetKeyDown(KeyCode.T))
        {
            autoAttackEnabled = !autoAttackEnabled;
            Debug.Log("Auto Attack toggled: " + autoAttackEnabled);
        }
    }
    
    IEnumerator AutoAttackRoutine()
    {
        // This routine runs forever, checking for enemies and handling combat.
        while (true)
        {
            if (autoAttackEnabled)
            {
                // Find the nearest enemy in our detection radius.
                currentTarget = FindNearestEnemy();
                if (currentTarget != null)
                {
                    // Try to get the Enemy component from our target.
                    Enemy enemyComponent = currentTarget.GetComponent<Enemy>();
                    if (enemyComponent != null)
                    {
                        // Calculate how far away the enemy is.
                        float distance = Vector2.Distance(transform.position, currentTarget.position);
                        // Ask the SkillManager if we have any skills ready to rock this enemy.
                        Skill candidateSkill = skillManager.GetCandidateSkillForTarget(enemyComponent);
                        
                        if (candidateSkill != null)
                        {
                            // We got a usable skill. If the enemy's too far for this skill...
                            if (distance > candidateSkill.range)
                            {
                                // ...move closer. Chop-chop!
                                playerController.SetClickToMoveTarget(currentTarget.position);
                            }
                            else
                            {
                                // Bam! Enemy’s in range so use that skill.
                                skillManager.UseSkill(candidateSkill, enemyComponent);
                            }
                        }
                        else
                        {
                            // No skills are off cooldown – time to resort to the good old auto attack.
                            if (distance > attackRange)
                            {
                                // Not close enough to swing your fists? Get closer.
                                playerController.SetClickToMoveTarget(currentTarget.position);
                            }
                            else
                            {
                                // Time to auto attack and kick some ass.
                                AutoAttack(enemyComponent);
                            }
                        }
                    }
                }
            }
            // Chill for a bit before checking again so we don’t burn the CPU.
            yield return new WaitForSeconds(autoAttackCheckInterval);
        }
    }

    // Searches for the nearest enemy within a given radius.
    Transform FindNearestEnemy()
    {
        // Get all colliders within our enemy detection radius that belong to the enemy layer.
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, enemyDetectionRadius, enemyLayer);
        Transform nearest = null;
        float minDistance = Mathf.Infinity;
        foreach (Collider2D hit in hits)
        {
            // Calculate distance from us to this enemy.
            float dist = Vector2.Distance(transform.position, hit.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                nearest = hit.transform;
            }
        }
        return nearest;
    }
    
    // Executes an auto attack on the specified enemy.
    void AutoAttack(Enemy enemy)
    {
        // Only attack if enough time has passed since our last swing.
        if (Time.time - lastAutoAttackTime >= autoAttackCooldown)
        {
            // Deal the auto attack damage.
            enemy.TakeDamage((int)autoAttackDamage);
            lastAutoAttackTime = Time.time;
        }
    }
    
    // Allows other scripts to manually set the current target.
    public void SetTarget(Transform enemy)
    {
        currentTarget = enemy;
    }
}
