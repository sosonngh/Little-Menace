using System;
using System.Collections;
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
    public bool isBoss;
    public bool isNewWeaponUnlocked;
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private int killCount;
    private int highScore;
    // infinite mode
    [SerializeField] int killsPerAddEnemy;
    [SerializeField] private int lastMilestone;

    // scene objects
    [SerializeField] private TMP_Text killScoreText;
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private CameraShake cameraHolder;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private Animator centerNotification;
    [SerializeField] private HeartsUI heartsContainer;
    [SerializeField] private WeaponIconsHolder weaponIconsHolder;
    [SerializeField] private BossHealthBar bossHealthBar;
    [SerializeField] private Canvas canvas;
    [SerializeField] private AudioSource backgroundMusic;
    [SerializeField] private GameObject NoAmmoMessage;
    //

    [SerializeField] private GamePhase[] gamePhaseArr;
    public GamePhase currentPhase;

    // audio
    private AudioSource audioSource;
    [SerializeField] private AudioClip newPhaseReached;

    // game over sequence
    [SerializeField] Player player;
    [SerializeField] FollowPlayer[] followPlayerObjects;

    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        StartCoroutine(NewGame());
    }

    private void ActivatePhase(int phaseIndex)
    {
        audioSource.PlayOneShot(newPhaseReached);

        currentPhase = gamePhaseArr[phaseIndex];

        enemySpawner.SetSpawnSettings(currentPhase.enemySpawnInterval, currentPhase.maxEnemiesPerSpawn, currentPhase.enemyArr);

        // ui
        weaponIconsHolder.SetWeaponLocked(currentPhase.latestWeaponUnlockedIndex, false);
        killScoreText.enabled = true;

        if (currentPhase.isNewWeaponUnlocked)
        {
            centerNotification.Play("Flash");
        }

        // boss
        if (currentPhase.isBoss)
        {
            enemySpawner.SpawnBoss();

            killScoreText.enabled = false;
            bossHealthBar.PlayAnimationIn();
        }
    }

    public void ShakeCamera(float duration, float magnitude)
    {
        cameraHolder.Shake(duration, magnitude);
    }

    public void UpdateBossHealth(float curHealth, float maxHealth)
    {
        bossHealthBar.SetHealth(curHealth, maxHealth);

        if (curHealth <= 0)
        {
            bossHealthBar.PlayAnimationOut();
            ActivatePhase(Array.IndexOf(gamePhaseArr, currentPhase) + 1);
            UpdateKillScoreText(); // ugh
        }
    }

    public void UpdateHealthUI(int health)
    {
        heartsContainer.UpdateHealthUI(health);
    }

    public void SetSelectedWeaponUI(int weaponIndex)
    {
        weaponIconsHolder.SetSelectedWeapon(weaponIndex);
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

    private void UpdateHighScoreText()
    {
        highScoreText.text = $"{highScore}";
    }

    private void UpdateHighScore()
    {
        if (killCount <= highScore) return;

        highScore = killCount;
        PlayerPrefs.SetInt("HighScore", highScore);
        PlayerPrefs.Save();
    }

    public void SpawnNoAmmoMessage()
    {
        Instantiate(NoAmmoMessage, canvas.transform);
    }

    public void IncrementKillCount()
    {
        if (currentPhase.isBoss) return;

        killCount++;

        UpdateKillScoreText();
        UpdateHighScore();

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

    public void SetMusicVolume(float volume)
    {
        backgroundMusic.volume = Mathf.Clamp01(volume);
    }
    public IEnumerator NewGame()
    {
        UpdateHighScoreText();

        // disable everything

        // clear all enemies
        enemySpawner.SetIsSpawning(false);
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemy.GetComponent<Enemy>().DieByWeapon(WeaponType.AlmightyPush);
        }
        bossHealthBar.ResetToDefault();

        canvas.enabled = false;
        foreach (FollowPlayer obj in followPlayerObjects) //ugh
        {
            obj.SetIsFollowing(false);
        }
        // (reset phase)
        currentPhase = gamePhaseArr[0];

        // respawn player
        yield return StartCoroutine(player.Respawn());

        // enable everything back and reset values;
        canvas.enabled = true;
        ActivatePhase(0);

        killCount = 0;
        UpdateKillScoreText();

        weaponIconsHolder.SetWeaponLocked(1, true);
        weaponIconsHolder.SetWeaponLocked(2, true);
        weaponIconsHolder.SetWeaponLocked(3, true);

        foreach (FollowPlayer obj in followPlayerObjects) //ugh
        {
            obj.SetIsFollowing(true);
        }
        enemySpawner.SetIsSpawning(true);
        
    }

}
