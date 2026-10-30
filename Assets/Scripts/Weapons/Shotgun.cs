using UnityEngine;

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
            Vector2 direction = Quaternion.Euler(0, 0, Random.Range(-spreadAreaDeg / 2, spreadAreaDeg / 2)) * transform.right;
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, attackRange);

            foreach(RaycastHit2D hit in hits)
            {
                if (hit) DealDamageIfEnemy(hit.collider);
            }

        }

        //recoil knockback
        player.ApplyLinearRecoil(-transform.right, recoilForce);

        player.ApplyAngularRecoil(angularRecoilForce);

        GameManager.Instance.ShakeCamera(0.3f, 0.3f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.right * attackRange);
    }

}
