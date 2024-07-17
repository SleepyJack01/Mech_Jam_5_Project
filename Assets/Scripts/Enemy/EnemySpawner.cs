using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy References")]
    [SerializeField] GameObject[] enemies;
    private Transform[] enemySpawnPoints;

    [Header("Spawn Settings")]
    [SerializeField] int enemiesPerWave = 10;
    public static int enemiesRemaining;
    [SerializeField] float spawnRate = 4f;
    private float spawnCountdown = 0f;

    void Start()
    {
        enemiesRemaining = enemiesPerWave;
        // get enemy spawn points from children
        enemySpawnPoints = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            enemySpawnPoints[i] = transform.GetChild(i);
        }
    }

    void Update()
    {

        if (spawnCountdown <= 0f && enemiesRemaining > 0)
        {
            SpawnEnemy();
            spawnCountdown = 1f / spawnRate;
        }

        spawnCountdown -= Time.deltaTime;
    }

    void SpawnEnemy()
    {
        if (enemies.Length == 0)
        {
            Debug.LogWarning("No enemies found for enemy spawner");
            return;
        }

        // get random enemy and spawn point
        GameObject enemy = enemies[Random.Range(0, enemies.Length)];
        Transform spawnPoint = enemySpawnPoints[Random.Range(0, enemySpawnPoints.Length)];

        Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
        enemiesRemaining--;
    }


}
