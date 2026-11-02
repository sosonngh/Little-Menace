using System.Collections;
using UnityEngine;

public class PlayerSprite : SpriteAnim
{

    [SerializeField] private Sprite idle = null;
    [SerializeField] private Sprite cry = null;

    [SerializeField] private float explodeShakeMagnitude;

    private Coroutine cryCoroutine;


    public void StandUp()
    {
        animator.Play("StandUp");
    }

    public void Cry(float duration)
    {
        if (cryCoroutine != null)
        {
            StopCoroutine(cryCoroutine);
            cryCoroutine = null;
        }

        cryCoroutine = StartCoroutine(CryCoroutine(duration));
    }

    private IEnumerator CryCoroutine(float duration)
    {
        sr.sprite = cry;

        yield return new WaitForSeconds(duration);

        sr.sprite = idle;

        cryCoroutine = null;
    }

    public IEnumerator ExplodeCoroutine(float duration)
    {
        if (cryCoroutine != null)
        {
            StopCoroutine(cryCoroutine);
            cryCoroutine = null;
        }

        sr.sprite = cry;
        animator.enabled = false;

        Vector3 initialPos = transform.position;
        float timeLeft = duration;
        while (timeLeft > 0)
        {
            float posX = initialPos.x + Random.Range(-explodeShakeMagnitude, explodeShakeMagnitude);
            float posY = initialPos.y + Random.Range(-explodeShakeMagnitude, explodeShakeMagnitude);

            transform.position = new Vector2(posX, posY);

            timeLeft -= Time.deltaTime;
            yield return null;

        }
        
        transform.position = initialPos;
        sr.sprite = idle;
        animator.enabled = true;
    }
}
