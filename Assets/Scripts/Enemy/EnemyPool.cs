using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolEnemy
{
    public GameObject go;
    public Enemy enemy;
    public int poolIndex;
    public IEnemySpawner spawner;
}

public class EnemyPool : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;

    private IObjectPool<PoolEnemy>[] enemyPools;
    private Dictionary<GameObject, PoolEnemy> goToEnemy;

    // throw an exception if we try to return an existing item,
    // already in the pool
    [SerializeField] private bool collectionCheck = true;
    // extra options to control the pool capacity and maximun size
    [SerializeField] private int defaultCapacity = 20;
    [SerializeField] private int maxSize = 100;

    private void Awake()
    {
        enemyPools = new ObjectPool<PoolEnemy>[enemyPrefabs.Length];
        enemyPools[0] = new ObjectPool<PoolEnemy>(
            OnCreateReconEnemy,
            OnGetFromPool,
            OnReleaseToPool,
            OnDestroyPooledObject,
            collectionCheck, defaultCapacity, maxSize);

        enemyPools[1] = new ObjectPool<PoolEnemy>(
            OnCreateAssaultEnemy,
            OnGetFromPool,
            OnReleaseToPool,
            OnDestroyPooledObject,
            collectionCheck, defaultCapacity, maxSize);

        goToEnemy = new Dictionary<GameObject, PoolEnemy>();
    }

    public PoolEnemy GetReconEnemy()
    {
        PoolEnemy enemy = enemyPools[0].Get();
        return enemy;
    }

    public PoolEnemy GetAssaultEnemy()
    {
        PoolEnemy enemy = enemyPools[1].Get();
        return enemy;
    }

    public PoolEnemy GetRandomEnemy()
    {
        int index = Random.Range(0, enemyPrefabs.Length);
        PoolEnemy enemy = enemyPools[index].Get();
        return enemy;
    }

    public void ReleaseEnemy(PoolEnemy enemy)
    {
        enemyPools[enemy.poolIndex].Release(enemy);
    }

    public PoolEnemy GetPoolEnemy(GameObject go)
    {
        return goToEnemy[go];
    }

    private PoolEnemy OnCreateReconEnemy()
    {
        PoolEnemy enemy = new PoolEnemy();
        enemy.go = Instantiate(enemyPrefabs[0]);
        enemy.enemy = enemy.go.GetComponent<ReconEnemy>();
        enemy.go.SetActive(false);
        enemy.poolIndex = 0;
        goToEnemy.Add(enemy.go, enemy);
        return enemy;
    }

    private PoolEnemy OnCreateAssaultEnemy()
    {
        PoolEnemy enemy = new PoolEnemy();
        enemy.go = Instantiate(enemyPrefabs[1]);
        enemy.enemy = enemy.go.GetComponent<AssaultEnemy>();
        enemy.go.SetActive(false);
        enemy.poolIndex = 1;
        goToEnemy.Add(enemy.go, enemy);
        return enemy;
    }
    private void OnReleaseToPool(PoolEnemy enemy)
    {
        enemy.enemy.OnRelease();
        enemy.go.SetActive(false);
    }

    private void OnGetFromPool(PoolEnemy enemy)
    {
        enemy.go.SetActive(true);
        enemy.enemy.OnAquire();
    }

    private void OnDestroyPooledObject(PoolEnemy enemy)
    {
        Destroy(enemy.go);
    }
}
