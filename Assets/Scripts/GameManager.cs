using System;
using TMPro;
using UnityEngine;

[System.Serializable]
public class GamePhase
{
    public int latestWeaponUnlockedIndex; // 0 - knife; 1 - pistol; 2 - shotgun; 3 - ar;
    public float enemySpawnInterval;
    public int maxEnemiesPerSpawn; // will spawn from 1 to {this number} enemies
    public GameObject[] enemyArr;
    public int nextPhaseMilestone; // -1 a phase is infinite
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private int killCount;
    // infinite mode
    [SerializeField] int killsPerAddEnemy;
    private int lastMilestone;

    // scene objects
    [SerializeField] private TMP_Text killScoreText;
    [SerializeField] private CameraShake cameraHolder;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private Animator CenterNotification;
    //

    [SerializeField] private GamePhase[] gamePhaseArr;

    public GamePhase currentPhase;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ActivatePhase(0);
        lastMilestone = gamePhaseArr[gamePhaseArr.Length - 2].nextPhaseMilestone;
    }

    private void ActivatePhase(int phaseIndex)
    {
        currentPhase = gamePhaseArr[phaseIndex];

        enemySpawner.SetSpawnSettings(currentPhase.enemySpawnInterval, currentPhase.maxEnemiesPerSpawn, currentPhase.enemyArr);

        if (currentPhase.latestWeaponUnlockedIndex is > 0 and < 4)
        {
            CenterNotification.Play("Flash");
        }
    }

    public void ShakeCamera(float duration, float magnitude)
    {
        cameraHolder.Shake(duration, magnitude);
    }

    public void IncrementKillCount()
    {
        killCount++;

        UpdateKillScoreText();

        if (currentPhase.nextPhaseMilestone == -1)
        {
            enemySpawner.SetAdditionalEnemiesPerSpawn((killCount - lastMilestone) / killsPerAddEnemy);
            return;
        }

        if (killCount >= currentPhase.nextPhaseMilestone)
        {
            ActivatePhase(Array.IndexOf(gamePhaseArr, currentPhase) + 1);
            UpdateKillScoreText(); // ugh
        }

    }

    private void UpdateKillScoreText()
    {
        if (currentPhase.nextPhaseMilestone == -1)
        {
            killScoreText.text = $"{killCount}";
            return;
        }

        killScoreText.text = $"{killCount} / {currentPhase.nextPhaseMilestone}";
    }

}
