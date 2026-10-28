using UnityEngine;
using UnityEngine.UI;

public class WeaponIconUI : MonoBehaviour
{
    [SerializeField] private Sprite inactive;
    [SerializeField] private Sprite active;

    private Image image;
    private Image iconLock;

    private void Awake()
    {
        image = GetComponent<Image>();
        iconLock = transform.GetChild(0).GetComponent<Image>();
    }

    public void SetSelected(bool state)
    {
        if (state) image.sprite = active;
        else image.sprite = inactive;
    }

    public void SetLocked(bool state)
    {
        if (state)
        {
            iconLock.enabled = true;
            image.color = new Color(128f, 128f, 128f);
        }
        else
        {
            iconLock.enabled = false;
            image.color = Color.white;
        }
    }
}
