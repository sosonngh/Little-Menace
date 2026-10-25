using UnityEngine;

public class WeaponSprite : MonoBehaviour
{
    protected SpriteRenderer sr;
    protected Animator animator;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    public void SetFlip(bool state)
    {
        sr.flipY = state;
    }

    public virtual void Attack()
    {
        animator.Play("Attack");
    }
}
