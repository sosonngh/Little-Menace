using System.Collections;
using UnityEngine;

public class RiffleLike : Weapon
{
    [SerializeField] private float attackRange;
    [SerializeField] private bool hasAimWiggle;
    [SerializeField] private float aimWiggleAreaDeg;

    [SerializeField] private FollowMouse weaponHolder;

    private Coroutine aimWiggleCoroutine = null;


    protected override void SpecificAttackBehaviour()
    {

        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, attackRange);

        if (hit) DealDamageIfEnemy(hit.collider);

        //recoil knockback
        player.ApplyLinearRecoil(-transform.right, recoilForce);

        if (hasAimWiggle)
        {
            if (aimWiggleCoroutine != null)
            {
                StopCoroutine(aimWiggleCoroutine);
                aimWiggleCoroutine = null;
            }

            aimWiggleCoroutine = StartCoroutine(EnableAimWiggle());
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.right * attackRange);
    }

    private IEnumerator EnableAimWiggle()
    {   
        weaponHolder.SetAimWiggleOffset(aimWiggleAreaDeg);

        yield return new WaitForSeconds(1f);

        weaponHolder.SetAimWiggleOffset(0);

        aimWiggleCoroutine = null;
    }

}
