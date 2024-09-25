using System.Collections;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public int EnemyCount;
    public float SpawnInterval;
    public GameObject EnemyPrefab;
    // Add more properties like enemy types, special enemies, etc.
}

public class WaveManager : MonoBehaviour
{
    public Wave[] Waves;
    private int currentWaveIndex = 0;
    private bool isSpawning = false;

    [Header("Spawn Points")]
    public Transform[] SpawnPoints;

    [Header("Game Over Conditions")]
    public int EnemiesRemaining = 0;

    private void Update()
    {
        if (isSpawning && EnemiesRemaining <= 0)
        {
            StartNextWave();
        }
    }

    public void StartWave()
    {
        if (currentWaveIndex < Waves.Length)
        {
            StartCoroutine(SpawnWave(Waves[currentWaveIndex]));
            isSpawning = true;
        }
        else
        {
            SiegeManager.Instance.Victory();
        }
    }

    private IEnumerator SpawnWave(Wave wave)
    {
        for (int i = 0; i < wave.EnemyCount; i++)
        {
            SpawnEnemy(wave.EnemyPrefab);
            EnemiesRemaining++;
            yield return new WaitForSeconds(wave.SpawnInterval);
        }
        isSpawning = false;
    }

    private void SpawnEnemy(GameObject enemyPrefab)
    {
        if (SpawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points assigned in WaveManager.");
            return;
        }

        Transform spawnPoint = SpawnPoints[Random.Range(0, SpawnPoints.Length)];
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }

    public void OnEnemyDestroyed()
    {
        EnemiesRemaining--;
        if (EnemiesRemaining <= 0 && currentWaveIndex >= Waves.Length - 1)
        {
            SiegeManager.Instance.Victory();
        }
    }

    private void StartNextWave()
    {
        currentWaveIndex++;
        if (currentWaveIndex < Waves.Length)
        {
            StartCoroutine(SpawnWave(Waves[currentWaveIndex]));
            isSpawning = true;
        }
        else
        {
            SiegeManager.Instance.Victory();
        }
    }
}
