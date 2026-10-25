using System.Collections;
using UnityEngine;

public class PlayerSprite : SpriteAnim
{

    [SerializeField] Sprite idle = null;
    [SerializeField] Sprite cry = null;


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

        yield return new WaitForSeconds(duration);

        sr.sprite = idle;
    }
}
