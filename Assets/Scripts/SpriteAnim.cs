using UnityEngine;

public class SpriteAnim : MonoBehaviour
{
    protected SpriteRenderer sr;
    protected Animator animator;

    private bool isMoving;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    public void SetSrEnabled(bool state)
    {
        sr.enabled = state;
    }

    public void SetFlip(bool state)
    {
        sr.flipX = state;
    }

    public void SetIsMoving(bool state)
    {
        isMoving = state;
        animator.SetBool("IsMoving", isMoving);
    }
}
