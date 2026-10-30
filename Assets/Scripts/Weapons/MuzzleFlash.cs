using System.Collections;
using UnityEngine;

public class MuzzleFlash : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(ShootOnce());
    }

    private IEnumerator ShootOnce()
    {
        animator.Play("Attack", 0, 0f);

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        Destroy(gameObject);
    }
}
