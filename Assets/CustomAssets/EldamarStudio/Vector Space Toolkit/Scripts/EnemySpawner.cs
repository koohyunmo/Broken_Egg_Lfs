using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] Enemies;
    public float SpawnInterval = 10f;
    public int MaxEnemySpawn = 5;
    public int MinEnemySpawn = 3;

    int spawnOffset = 100;

    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        while (true)
        {
            Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
            Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

            int EnemiesCount = Random.Range(MinEnemySpawn, MaxEnemySpawn);

            for (int i = 0; i < EnemiesCount; ++i)
            {
                GameObject enemy = Instantiate(Enemies[Random.Range(0, 4)]);

                float randomPosX = spawnOffset + Random.Range(min.x, max.x);
                float randomPosY = spawnOffset + Random.Range(min.y, max.y);

                enemy.transform.position = new Vector2(randomPosX, randomPosY);
            }

            yield return new WaitForSeconds(SpawnInterval);
        }
    }
}