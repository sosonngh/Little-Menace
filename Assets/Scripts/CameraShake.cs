using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private float duration;
    private float magnitude;

    private Vector2 originalPos;

    private void Start()
    {
        originalPos = transform.localPosition;
    }

    public void Shake(float duration, float magnitude)
    {
        this.duration = duration;
        this.magnitude = magnitude;
        StopAllCoroutines();
        StartCoroutine(ShakeCoroutine());
    }

    private IEnumerator ShakeCoroutine()
    {
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            float subMagnitide = magnitude * (1f - (timeElapsed / duration));

            Vector2 offset = subMagnitide * Random.insideUnitCircle;

            transform.localPosition = originalPos + offset;

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
