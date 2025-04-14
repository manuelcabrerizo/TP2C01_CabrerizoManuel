using System.Collections.Generic;
using UnityEngine;

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

    private PoolAllocator<PoolEnemy>[] enemyPools;
    private Dictionary<GameObject, PoolEnemy> goToEnemy;

    private void Awake()
    {
        enemyPools = new PoolAllocator<PoolEnemy>[enemyPrefabs.Length];
        enemyPools[0] = new PoolAllocator<PoolEnemy>(
            OnCreateReconEnemy,
            OnDestroyPooledObject,
            OnGetFromPool,
            OnReleaseToPool);

        enemyPools[1] = new PoolAllocator<PoolEnemy>(
            OnCreateAssaultEnemy,
            OnDestroyPooledObject,
            OnGetFromPool,
            OnReleaseToPool);

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
