using System.Collections;
using UnityEngine;

public class MeteorsSpawner : MonoBehaviour
{
    public GameObject[] Meteors;
    public float SpawnInterval = 10f;
    public int MaxMeteorsSpawn = 3;
    public int MinMeteorsSpawn = 1;

    int spawnOffset = 100;

    void Start()
    {
        StartCoroutine(SpawnMeteors());
    }

    IEnumerator SpawnMeteors()
    {
        while (true)
        {
            Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
            Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

            int EnemiesCount = Random.Range(MinMeteorsSpawn, MaxMeteorsSpawn);

            for (int i = 0; i < EnemiesCount; ++i)
            {
                GameObject enemy = Instantiate(Meteors[Random.Range(0, 4)]);

                float randomPosX = spawnOffset + Random.Range(min.x, max.x);
                float randomPosY = spawnOffset + Random.Range(min.y, max.y);

                enemy.transform.position = new Vector2(randomPosX, randomPosY);
            }

            yield return new WaitForSeconds(SpawnInterval);
        }
    }
}