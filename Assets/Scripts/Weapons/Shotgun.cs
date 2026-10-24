using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Shotgun : Weapon
{
    [SerializeField] private float attackRange;
    [SerializeField] private float spreadAreaDeg;
    [SerializeField] private int bulletsPerShot;
    [SerializeField] private float angularRecoilForce;

    [SerializeField] private FollowMouse weaponHolder;

    protected override void SpecificAttackBehaviour()
    {
        for (int i = 0; i < bulletsPerShot; i++)
        {
            Vector2 direction = Quaternion.Euler(0, 0, Random.Range(-spreadAreaDeg / 2, spreadAreaDeg / 2)) * transform.up;
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, attackRange);

            foreach(RaycastHit2D hit in hits)
            {
                if (hit) KillIfEnemy(hit.collider);
            }

        }

        //recoil knockback
        player.ApplyLinearRecoil(-transform.up, recoilForce);

        player.ApplyAngularRecoil(angularRecoilForce);

        GameManager.Instance.ShakeCamera(0.3f, 0.3f);
    }

}
