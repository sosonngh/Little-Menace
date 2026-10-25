using UnityEngine;

public class SpriteAnim : MonoBehaviour
{
    protected SpriteRenderer sr;
    protected Animator animator;

    private bool isMoving;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
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
