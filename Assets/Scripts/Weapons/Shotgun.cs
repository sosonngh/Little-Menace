using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Shotgun : Weapon
{
    [SerializeField] private float attackRange;
    [SerializeField] private float spreadAreaDeg;
    [SerializeField] private int bulletsPerShot;
    [SerializeField] private float angularRecoilForce;

    [SerializeField] private FollowMouse weaponHolder;

    public override void Attack()
    {
        if (IsOnCooldown()) return;

        for (int i = 0; i < bulletsPerShot; i++)
        {
            Vector2 direction = Quaternion.Euler(0, 0, Random.Range(-spreadAreaDeg / 2, spreadAreaDeg / 2)) * transform.up;
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, attackRange);

            Debug.DrawRay(transform.position, direction * attackRange, Color.red, 0.5f);

            foreach(RaycastHit2D hit in hits)
            {
                if (hit) KillIfEnemy(hit.collider);
            }

        }

        //recoil knockback
        player.ApplyLinearRecoil(-transform.up, recoilForce);

        player.ApplyAngularRecoil(angularRecoilForce);

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.up * attackRange);
    }
}
