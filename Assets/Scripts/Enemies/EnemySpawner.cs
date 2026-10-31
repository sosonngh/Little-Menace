using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private float spawnInterval;
    private int maxEnemiesPerSpawn;
    private GameObject[] enemyArr;
    private int additionalEnemiesPerSpawn;

    private bool isSpawning;

    [SerializeField] private float spawnRadius;
    [SerializeField] private GameObject bossPrefab;


    private float timeElapsed;

    public void SetIsSpawning(bool state)
    {
        isSpawning = state;
    }

    private void Spawn()
    {
        int enemyCount = Random.Range(1, maxEnemiesPerSpawn + 1 + additionalEnemiesPerSpawn);

        for (int i = 0; i < enemyCount; i++)
        {
            Vector2 spawnPoint = (Vector2)transform.position + Random.insideUnitCircle.normalized * spawnRadius;
            GameObject enemy = enemyArr[Random.Range(0, enemyArr.Length)];
            Instantiate(enemy, spawnPoint, Quaternion.identity);
        }

    }

    public void SpawnBoss()
    {
        Vector2 spawnPoint = (Vector2)transform.position + Random.insideUnitCircle.normalized * spawnRadius;
        Instantiate(bossPrefab, spawnPoint, Quaternion.identity);
    }

    public void SetSpawnSettings(float spawnInterval, int maxEnemiesPerSpawn, GameObject[] enemyArr)
    {
        this.spawnInterval = spawnInterval;
        this.maxEnemiesPerSpawn = maxEnemiesPerSpawn;
        this.enemyArr = enemyArr;

        // reset just in case
        timeElapsed = 0;
    }

    public void SetAdditionalEnemiesPerSpawn(int value)
    {
        additionalEnemiesPerSpawn = value;
    }

    private void Update()
    {
        if (!isSpawning) return;

        if (timeElapsed >= spawnInterval)
        {
            Spawn();
            timeElapsed -= spawnInterval;
        }

        timeElapsed += Time.deltaTime;

    }
}
