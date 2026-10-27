using UnityEngine;

public class SpawnEnemiesOnDefeat : MonoBehaviour
{
    [SerializeField] GameObject enemyToSpawn;
    [SerializeField] int numOfEnemies;

    public void Spawn()
    {
        for (int i = 0; i < numOfEnemies; i++)
        {
            Vector2 offset = Random.insideUnitCircle * 0.5f;
            Instantiate(enemyToSpawn, (Vector2)transform.position + offset, Quaternion.identity);
        }
    }
}
