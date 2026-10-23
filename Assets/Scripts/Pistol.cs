using UnityEngine;

public class Pistol : Weapon
{
    [SerializeField] private float attackRange;

    public override void Attack()
    {
        if (IsOnCooldown()) return;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, attackRange);

        if (hit) KillIfEnemy(hit.collider);

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.up * attackRange);
    }
}
