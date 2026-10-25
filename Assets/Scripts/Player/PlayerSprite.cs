using System.Collections;
using UnityEngine;

public class PlayerSprite : MonoBehaviour
{
    private bool isMoving;

    private Animator animator;
    private SpriteRenderer sr;

    [SerializeField] Sprite idle;
    [SerializeField] Sprite cry;

    private void Start()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    //
    public void SetFlip(bool state)
    {
        sr.flipX = state;
    }

    public void SetIsMoving(bool state)
    {
        isMoving = state;
        animator.SetBool("IsMoving" , isMoving);
    }

    public void StandUp()
    {
        animator.Play("StandUp");
    }

    public void Cry(float duration)
    {
        StopAllCoroutines();
        StartCoroutine(CryCoroutine(duration));
    }

    private IEnumerator CryCoroutine(float duration)
    {
        sr.sprite = cry;

        yield return new WaitForSeconds(duration + 0.5f);

        sr.sprite = idle;
    }
}
