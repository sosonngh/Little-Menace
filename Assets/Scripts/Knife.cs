using UnityEngine;

public class Knife : Weapon
{
    [SerializeField] private float attackRadius;

    public override void Attack()
    {
        if (IsOnCooldown()) return;

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
