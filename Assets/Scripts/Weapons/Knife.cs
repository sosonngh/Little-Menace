using UnityEngine;

public class Knife : Weapon
{
    [SerializeField] private float attackRadius;

    protected override void SpecificAttackBehaviour()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position + 0.5f * transform.right, attackRadius);

        foreach (Collider2D hit in hits)
        {
            DealDamageIfEnemy(hit);
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + 0.5f * transform.right, attackRadius);
    }

}
