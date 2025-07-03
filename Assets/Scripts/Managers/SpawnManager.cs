using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    private LevelData levelData;
    private List<EnemyData> spawnQueue = new List<EnemyData>();
    private Vector3[] spawnPositions;
    private float cellSize;
    private bool isAllSpawned = false;
    private int aliveEnemyCount = 0;

    public void Initialize(LevelData data, Vector3[] spawnPositions, float cellSize)
    {
        this.levelData = data;
        this.spawnPositions = spawnPositions;
        this.cellSize = cellSize;
        PrepareQueue();
        StartCoroutine(SpawnEnemy());
    }

    private void PrepareQueue()
    {
        spawnQueue.Clear();
        isAllSpawned = false;

        foreach (var enemyWaveInfo in levelData.enemyWaves)
        {
            for (int i = 0; i < enemyWaveInfo.count; i++)
            {
                spawnQueue.Add(enemyWaveInfo.enemyType);
            }
        }

        aliveEnemyCount = spawnQueue.Count;
    }

    private IEnumerator SpawnEnemy()
    {
        foreach (EnemyData enemyToSpawn in spawnQueue)
        {
            yield return new WaitForSeconds(levelData.spawnDelay);

            int spawnIndex = Random.Range(0, spawnPositions.Length);
            Vector3 selectedSpawnPosition = new Vector3(spawnPositions[spawnIndex].x, spawnPositions[spawnIndex].y, spawnPositions[spawnIndex].z);

            // GameObject newEnemyObject = Instantiate(enemyToSpawn.prefab, selectedSpawnPosition, Quaternion.identity);
            GameObject newEnemyObject = ObjectPool.Instance.SpawnFromPool(enemyToSpawn.prefab.name, selectedSpawnPosition, Quaternion.identity);

            Enemy enemyScript = newEnemyObject.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.Initialize(enemyToSpawn, cellSize, onEnemyDie);
            }
        }

        isAllSpawned = true;
    }

    private void onEnemyDie()
    {
        aliveEnemyCount--;

        if (!isAllSpawned || aliveEnemyCount > 0) return;

        GameManager.Instance.GameOver(true);
    }
}
