using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float spawnInterval;
    [SerializeField] private int enemiesPerSpawn;
    [SerializeField] private float spawnRadius;

    [SerializeField] private GameObject enemyPrefab;

    private float timeElapsed;

    private void Spawn()
    {
        for (int i = 0; i < enemiesPerSpawn; i++)
        {
            Vector2 spawnPoint = (Vector2)transform.position + Random.insideUnitCircle.normalized * spawnRadius;
            Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
        }
    }

    private void Update()
    {
        if (timeElapsed >= spawnInterval)
        {
            Spawn();
            timeElapsed -= spawnInterval;
        }

        timeElapsed += Time.deltaTime;
    }
}
