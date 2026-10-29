using UnityEngine;

public class BossHealthBar : MonoBehaviour
{
    [SerializeField] private RectTransform healthBar;
    [SerializeField] private RectTransform trailingBar;
    [SerializeField] private float catchUpSpeed;

    private Vector2 initialSize;
    private float targetWidth;

    private void Start()
    {
        initialSize = healthBar.rect.size;
        targetWidth = initialSize.x;
    }

    public void SetHealth(float curHealth, float maxHealth)
    {
        targetWidth = initialSize.x * curHealth / maxHealth;
        healthBar.sizeDelta = new Vector2(targetWidth, initialSize.y);
    }

    private void Update()
    {
        float curWidth = trailingBar.rect.width;

        if (targetWidth >= curWidth) return;

        float newWidth = Mathf.MoveTowards(curWidth, targetWidth, catchUpSpeed * Time.deltaTime);
        trailingBar.sizeDelta = new Vector2(newWidth, trailingBar.sizeDelta.y);
    }
}
