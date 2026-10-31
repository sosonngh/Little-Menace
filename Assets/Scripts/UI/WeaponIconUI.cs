using UnityEngine;
using UnityEngine.UI;

public class WeaponIconUI : MonoBehaviour
{
    [SerializeField] private Sprite inactive;
    [SerializeField] private Sprite active;

    [SerializeField] private Image image;
    [SerializeField] private Image iconLock;

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
            image.color = Color.gray;
        }
        else
        {
            iconLock.enabled = false;
            image.color = Color.white;
        }
    }
}
