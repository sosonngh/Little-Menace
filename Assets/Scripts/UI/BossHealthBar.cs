using UnityEngine;

public class BossHealthBar : MonoBehaviour
{
    [SerializeField] private RectTransform healthBar;
    [SerializeField] private RectTransform trailingBar;
    [SerializeField] private float catchUpSpeed;

    private Vector2 initialPos;
    private RectTransform rectTransform;

    private Vector2 initialSize;
    private float targetWidth;

    private Animator animator;

    private void Awake()
    {
        initialSize = healthBar.rect.size;
        targetWidth = initialSize.x;

        rectTransform = GetComponent<RectTransform>();
        animator = GetComponent<Animator>();

        initialPos = rectTransform.anchoredPosition;
    }

    public void PlayAnimationIn()
    {
        animator.Play("BossHealthBarIn");
    }

    public void PlayAnimationOut()
    {
        animator.Play("BossHealthBarOut");
    }

    public void ResetToDefault()
    {
        animator.Play("Idle");
        SetHealth(1, 1);
        trailingBar.sizeDelta = healthBar.sizeDelta;
        rectTransform.anchoredPosition = initialPos;
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
