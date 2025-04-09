using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SpawnedEnemy
{
    public GameObject go;
    public EnemyBehavior behavior;
    public ReconEnemyBehavior reconBehavior;
    public AssaultEnemyBehavior assaultBehavior;
    public int poolIndex;
    public int spawnIndex;
}

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] private GameObject[] enemyPrefabs;

    private IObjectPool<SpawnedEnemy>[] enemyPools;
    private Dictionary<GameObject, SpawnedEnemy> goToEnemy;

    // throw an exception if we try to return an existing item,
    // already in the pool
    [SerializeField] private bool collectionCheck = true;
    // extra options to control the pool capacity and maximun size
    [SerializeField] private int defaultCapacity = 20;
    [SerializeField] private int maxSize = 100;


    private void Awake()
    {
        enemyPools = new ObjectPool<SpawnedEnemy>[enemyPrefabs.Length];
        enemyPools[0] = new ObjectPool<SpawnedEnemy>(
            OnCreateReconEnemy,
            OnGetFromPool,
            OnReleaseToPool,
            OnDestroyPooledObject,
            collectionCheck, defaultCapacity, maxSize);

        enemyPools[1] = new ObjectPool<SpawnedEnemy>(
            OnCreateAssaultEnemy,
            OnGetFromPool,
            OnReleaseToPool,
            OnDestroyPooledObject,
            collectionCheck, defaultCapacity, maxSize);

        goToEnemy = new Dictionary<GameObject, SpawnedEnemy>();
    }

    public SpawnedEnemy SpawnReconEnemy()
    {
        SpawnedEnemy enemy = enemyPools[0].Get();
        return enemy;
    }

    public SpawnedEnemy SpawnAssaultEnemy()
    {
        SpawnedEnemy enemy = enemyPools[1].Get();
        return enemy;
    }

    public SpawnedEnemy SpawnRandomEnemy()
    {
        int index = Random.Range(0, enemyPrefabs.Length);
        SpawnedEnemy enemy = enemyPools[index].Get();
        return enemy;
    }

    public void ReleaseEnemy(SpawnedEnemy enemy)
    {
        enemyPools[enemy.poolIndex].Release(enemy);
    }

    public SpawnedEnemy GetSpawnEnemy(GameObject go)
    {
        return goToEnemy[go];
    }
   
    private SpawnedEnemy OnCreateReconEnemy()
    {
        SpawnedEnemy enemy = new SpawnedEnemy();
        enemy.go = Instantiate(enemyPrefabs[0]);
        enemy.behavior = enemy.go.GetComponent<EnemyBehavior>();
        enemy.reconBehavior = enemy.go.GetComponent<ReconEnemyBehavior>();
        enemy.go.SetActive(false);
        enemy.poolIndex = 0;
        goToEnemy.Add(enemy.go, enemy);
        return enemy;
    }

    private SpawnedEnemy OnCreateAssaultEnemy()
    {
        SpawnedEnemy enemy = new SpawnedEnemy();
        enemy.go = Instantiate(enemyPrefabs[1]);
        enemy.behavior = enemy.go.GetComponent<EnemyBehavior>();
        enemy.assaultBehavior = enemy.go.GetComponent<AssaultEnemyBehavior>();
        enemy.go.SetActive(false);
        enemy.poolIndex = 1;
        goToEnemy.Add(enemy.go, enemy);
        return enemy;
    }
    private void OnReleaseToPool(SpawnedEnemy enemy)
    {
        enemy.behavior.OnRelease();
        enemy.go.SetActive(false);
    }

    private void OnGetFromPool(SpawnedEnemy enemy)
    {
        enemy.go.SetActive(true);
        enemy.behavior.OnAquire();
    }

    private void OnDestroyPooledObject(SpawnedEnemy enemy)
    {
        Destroy(enemy.go);
    }
}


