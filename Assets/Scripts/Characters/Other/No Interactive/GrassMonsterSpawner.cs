using System.Collections;
using UnityEngine;

public class GrassMonsterSpawner : MonoBehaviour
{
    public GameObject grassMonsterPrefab;
    public Transform spawnPoint;
    public Transform endPoint;
    public float spawnInterval = 0.2f;

    private bool hasSpawnedFirstMonster = false;
    private bool stopSpawning = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasSpawnedFirstMonster)
        {
            StartCoroutine(SpawnGrassMonsters());
        }
    }

    IEnumerator SpawnGrassMonsters()
    {
        while (!stopSpawning)
        {
            GameObject monster = Instantiate(grassMonsterPrefab, spawnPoint.position, Quaternion.identity);
            GrassMonster monsterMove = monster.GetComponent<GrassMonster>();
            monsterMove.Initialize(spawnPoint, endPoint, this);

            if (!hasSpawnedFirstMonster)
            {
                hasSpawnedFirstMonster = true;
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void StopSpawning()
    {
        stopSpawning = true;
    }
}
