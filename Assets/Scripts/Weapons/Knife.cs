using UnityEngine;

public class Knife : Weapon
{
    [SerializeField] private float attackRadius;

    protected override void SpecificAttackBehaviour()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRadius);

        foreach (Collider2D hit in hits)
        {
            KillIfEnemy(hit);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
