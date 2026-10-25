using UnityEngine;

public class KnifeSprite : WeaponSprite
{
    public override void Attack()
    {
        // cuz it flips and moves itself during animation and breaks everything if left unnoticed
        if (sr.flipY)
        {
            animator.Play("AttackFlip");
            return;
        }

        animator.Play("Attack");
    }
}
