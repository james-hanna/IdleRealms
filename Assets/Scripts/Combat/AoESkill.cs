using UnityEngine;

[CreateAssetMenu(fileName = "NewAOESkill", menuName = "Skills/AOESkill")]
public class AOESkill : Skill
{
    [Tooltip("The radius of the area-of-effect damage.")]
    public float aoeRadius = 2f;

    public override void Execute(GameObject user, Enemy target)
    {
        // Log for debugging
        Debug.Log($"{user.name} uses {skillName} as an AOE attack!");

        // Use the target's position as the center of the AOE.
        // (Alternatively, you could use the user's position if that fits your design better.)
        Vector2 center = target != null ? target.transform.position : (Vector2)user.transform.position;

        // Optionally, use a layer mask so that only enemies are affected.
        // Make sure your enemy objects are on a layer named "Enemy".
        int enemyLayerMask = LayerMask.GetMask("Enemy");

        // Get all colliders within the specified radius that are on the "Enemy" layer.
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(center, aoeRadius, enemyLayerMask);
        foreach (Collider2D hitCollider in hitColliders)
        {
            Enemy enemy = hitCollider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}
