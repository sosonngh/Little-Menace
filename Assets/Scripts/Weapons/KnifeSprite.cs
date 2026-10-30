using UnityEngine;

public class KnifeSprite : WeaponSprite
{
    [SerializeField] Animator animator;

    public override void AttackAnimation()
    {
        // cuz it flips and moves itself during animation and breaks everything if left unnoticed
        if (sr.flipY)
        {
            animator.Play("AttackFlip", 0, 0f);
            return;
        }

        animator.Play("Attack", 0, 0f);
    }

    // idk i doenst do i want it to do
    private void OnEnable()
    {
            animator.Rebind();
            animator.Play("Idle", 0, 0f);

    }

}
