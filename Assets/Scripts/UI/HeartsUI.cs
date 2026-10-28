using UnityEngine;

public class HeartsUI : MonoBehaviour
{
    public void UpdateHealthUI(int health)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(health > i);
        }
    }
}
