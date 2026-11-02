using TMPro;
using UnityEngine;

public class NoAmmoMessage : MonoBehaviour
{
    [SerializeField] private float animationLengthSeconds;
    [SerializeField] private float distanceUp;

    [SerializeField] private Vector2 minSpawnPos;
    [SerializeField] private Vector2 maxSpawnPos;

    private Vector2 initialPosition;
    private Color initialColor;

    private RectTransform rectTransform;
    private TMP_Text tmpText;

    private float timeElapsed;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        tmpText = GetComponent<TMP_Text>();

        initialColor = tmpText.color;
        initialPosition = Vector2.Lerp(minSpawnPos, maxSpawnPos, Random.value);
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;

        float progress = Mathf.Clamp01(timeElapsed / animationLengthSeconds);

        rectTransform.anchoredPosition = initialPosition + distanceUp * progress * Vector2.up;

        Color color = initialColor;
        color.a = Mathf.Lerp(1, 0, progress);
        tmpText.color = color;

        if (progress >= 1)
        {
            Destroy(gameObject);
        }
    }
}
