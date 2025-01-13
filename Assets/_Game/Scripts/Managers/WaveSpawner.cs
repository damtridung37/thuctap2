using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaveSpawner : Singleton<WaveSpawner>
{
    [System.Serializable]
    public class Wave
    {
        public EnemyType[] enemies;
        public int count;
        public float timeBetweenSpawns;
    }

    public Wave[] waves;
    public Transform[] spawnPoints;
    public float timeBetweenWaves;
    
    [Header("EnemyPrefabDictionary")]
    [SerializeField] private EnemyPrefabDictionary enemyPrefabDictionary;
    private Dictionary<EnemyType,Transform> enemyContainerDictionary = new Dictionary<EnemyType, Transform>();
    private int currentActiveEnemies;

    public int CurrentActiveEnemies
    {
        get => currentActiveEnemies;
        set
        {
            currentActiveEnemies = value;
            SpawnWave();
        }
    }

    private Wave currentWave;
    private int currentWaveIndex;
    private Transform player;

    private bool finishedSpawning;

    public GameObject boss;
    public Transform bossSpawnPoint;

    public GameObject healthBar;
    
    private Dictionary<EnemyType,ObjectPool<Enemy>> enemyPools = new Dictionary<EnemyType, ObjectPool<Enemy>>();
    
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(StartNextWave(currentWaveIndex));
    }

    IEnumerator StartNextWave(int index)
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        StartCoroutine(SpawnWave(index));
    }
    IEnumerator SpawnWave(int index)
    {
        currentWave = waves[index];

        for (int i = 0; i < currentWave.count; i++)
        {
            yield return new WaitForSeconds(currentWave.timeBetweenSpawns);
            if (player == null)
            {
                yield break;
            }
            
            EnemyType randomEnemy = currentWave.enemies[Random.Range(0, currentWave.enemies.Length)];
            Transform randomSpot = spawnPoints[Random.Range(0, spawnPoints.Length)];
            
            Enemy enemy = GetEnemy(randomEnemy,randomSpot.position);

        }
        
        finishedSpawning = true;
    }

    private void SpawnWave()
    {
        Debug.Log("Current Active Enemies: " + currentActiveEnemies);
        if (finishedSpawning && currentActiveEnemies <= 0)
        { 
            Debug.Log("Wave Finished");
            finishedSpawning = false ;
            if (currentWaveIndex + 1 < waves.Length) 
            {
                currentWaveIndex++;
                StartCoroutine(StartNextWave(currentWaveIndex));
            }
            else
            {
                Instantiate(boss, bossSpawnPoint.position, bossSpawnPoint.rotation);
                healthBar.SetActive(true);  
            }
        }
    }

    public Enemy GetEnemy(EnemyType enemyType, Vector3 position)
    {
        if (enemyPools.ContainsKey(enemyType))
        {
            return enemyPools[enemyType].Pull(position, enemyContainerDictionary[enemyType]);
        }
        else
        {
            GameObject container = new GameObject(enemyType.ToString());
            enemyContainerDictionary.Add(enemyType, container.transform);
            Enemy prototype = enemyPrefabDictionary[enemyType];
            
            if(prototype == null)
            {
                Debug.LogError("No prototype found for " + enemyType);
                return null;
            }
            
            ObjectPool<Enemy> newPool = new ObjectPool<Enemy>(prototype,
                (Enemy) =>
                {
                    currentActiveEnemies++;
                },
                (Enemy) =>
                {
                    CurrentActiveEnemies = currentActiveEnemies - 1;
                },
                container.transform,
                1
                );
            
            enemyPools.Add(enemyType, newPool);
            return newPool.Pull(position, container.transform);
        }
    }

    public Enemy GetRandomEnemy()
    {
        List<EnemyType> keys = new List<EnemyType>(enemyPrefabDictionary.Keys);
        EnemyType randomEnemy = keys[Random.Range(0, keys.Count)];
        Transform randomSpot = spawnPoints[Random.Range(0, spawnPoints.Length)];
            
        return GetEnemy(randomEnemy,randomSpot.position);
    }
}
[Serializable]
public class EnemyPrefabDictionary : SerializableDictionary<EnemyType, Enemy>
{
}

[Serializable]
public class StatDictionary : SerializableDictionary<StatType, float>
{
}