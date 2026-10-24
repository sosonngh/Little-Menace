using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private int killCount;
    [SerializeField] private int[] killMilestones;

    [SerializeField] private TMP_Text killScoreText;

    [SerializeField] private CameraShake cameraHolder;

    private void Awake()
    {
        Instance = this;
    }

    public void ShakeCamera(float duration, float magnitude)
    {
        cameraHolder.Shake(duration, magnitude);
    }

    public void IncrementKillCount()
    {
        killCount++;
        killScoreText.text = killCount.ToString();
    }

}
