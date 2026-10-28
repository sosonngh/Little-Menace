using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [SerializeField] private Animator centerNotification;
    [SerializeField] private HeartsUI heartsContainer;
    [SerializeField] private WeaponIconsHolder weaponIconsHolder;
    //

    [SerializeField] private GamePhase[] gamePhaseArr;

    public GamePhase currentPhase;

    // audio
    private AudioSource audioSource;
    [SerializeField] private AudioClip gameOverClip;
    [SerializeField] private AudioClip newPhaseReached;

    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        ActivatePhase(0);
        lastMilestone = gamePhaseArr[gamePhaseArr.Length - 2].nextPhaseMilestone;

    }

    private void ActivatePhase(int phaseIndex)
    {
        audioSource.PlayOneShot(newPhaseReached);

        currentPhase = gamePhaseArr[phaseIndex];

        enemySpawner.SetSpawnSettings(currentPhase.enemySpawnInterval, currentPhase.maxEnemiesPerSpawn, currentPhase.enemyArr);

        // ui
        weaponIconsHolder.SetWeaponLocked(currentPhase.latestWeaponUnlockedIndex, false);

        if (currentPhase.latestWeaponUnlockedIndex is > 0 and < 4)
        {
            centerNotification.Play("Flash");
        }
    }

    public void ShakeCamera(float duration, float magnitude)
    {
        cameraHolder.Shake(duration, magnitude);
    }

    public void UpdateHealthUI(int health)
    {
        heartsContainer.UpdateHealthUI(health);
    }

    public void SetSelectedWeaponUI(int weaponIndex)
    {
        weaponIconsHolder.SetSelectedWeapon(weaponIndex);
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


    public void GameOver()
    {
        StartCoroutine(GameOverCoroutine());
    }

    private IEnumerator GameOverCoroutine()
    {
        audioSource.PlayOneShot(gameOverClip);

        cameraHolder.Shake(3f, 2f);

        yield return new WaitForSeconds(5f);

        SceneManager.LoadScene(0);
    }

}
