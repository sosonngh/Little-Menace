using System.Collections;
using UnityEngine;

public class RiffleLike : Weapon
{
    [SerializeField] private float attackRange;
    [SerializeField] private bool hasAimWiggle;
    [SerializeField] private float aimWiggleAreaDeg;

    [SerializeField] private FollowMouse weaponHolder;

    private Coroutine aimWiggleCoroutine = null;

    public override void Attack()
    {
        if (IsOnCooldown()) return;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, attackRange);

        if (hit) KillIfEnemy(hit.collider);

        //recoil knockback
        player.ApplyLinearRecoil(-transform.up, recoilForce);

        if (hasAimWiggle)
        {
            if (aimWiggleCoroutine != null) StopCoroutine(nameof(EnableAimWiggle));

            aimWiggleCoroutine = StartCoroutine(EnableAimWiggle());
        }
    }

    private IEnumerator EnableAimWiggle()
    {
        weaponHolder.SetAimWiggleOffset(aimWiggleAreaDeg);

        yield return new WaitForSeconds(1f);

        weaponHolder.SetAimWiggleOffset(0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.up * attackRange);
    }
}
