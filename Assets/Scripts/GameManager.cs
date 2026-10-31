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
    // infinite mode
    [SerializeField] int killsPerAddEnemy;
    [SerializeField] private int lastMilestone;

    // scene objects
    [SerializeField] private TMP_Text killScoreText;
    [SerializeField] private CameraShake cameraHolder;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private Animator centerNotification;
    [SerializeField] private HeartsUI heartsContainer;
    [SerializeField] private WeaponIconsHolder weaponIconsHolder;
    [SerializeField] private BossHealthBar bossHealthBar;
    [SerializeField] private GameObject canvas;
    [SerializeField] private AudioSource backgroundMusic;
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
        StartCoroutine(NewGame());
    }

    private void ActivatePhase(int phaseIndex)
    {
        audioSource.PlayOneShot(newPhaseReached);

        currentPhase = gamePhaseArr[phaseIndex];

        enemySpawner.SetSpawnSettings(currentPhase.enemySpawnInterval, currentPhase.maxEnemiesPerSpawn, currentPhase.enemyArr);

        // ui
        weaponIconsHolder.SetWeaponLocked(currentPhase.latestWeaponUnlockedIndex, false);

        if (currentPhase.isNewWeaponUnlocked)
        {
            centerNotification.Play("Flash");
        }

        // boss
        if (currentPhase.isBoss)
        {
            enemySpawner.SpawnBoss();

            killScoreText.enabled = false;
            bossHealthBar.GetComponent<Animator>().Play("BossHealthBarIn");
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
            bossHealthBar.GetComponent<Animator>().Play("BossHealthBarOut");
            ActivatePhase(Array.IndexOf(gamePhaseArr, currentPhase) + 1);
            killScoreText.enabled = true;
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

    public void IncrementKillCount()
    {
        if (currentPhase.isBoss) return;

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

    public IEnumerator NewGame()
    {
        // disable everything
        backgroundMusic.volume = 0.05f;

        enemySpawner.SetIsSpawning(false);
        canvas.SetActive(false);
        foreach (FollowPlayer obj in followPlayerObjects) //ugh
        {
            obj.SetIsFollowing(false);
        }
        // (reset phase)
        currentPhase = gamePhaseArr[0];

        // kill all enemies
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemy.GetComponent<Enemy>().DieByWeapon(WeaponType.AlmightyPush);
        }

        // respawn player
        yield return StartCoroutine(player.Respawn());

        // enable everything back and reset values;
        backgroundMusic.volume = 0.1f;

        canvas.SetActive(true);
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
