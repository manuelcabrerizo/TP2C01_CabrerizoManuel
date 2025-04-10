using System.Collections.Generic;
using UnityEngine;
public class AssaultEnemySpawner : MonoBehaviour, IEnemySpawner
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float timeToSpawn = 10.0f;
    private float timer = 0.0f;

    private List<PoolEnemy> spawnedEnemies;

    private void Start()
    {
        spawnedEnemies = new List<PoolEnemy>();
        timer = timeToSpawn;
    }

    private void Update()
    {
        if (timer <= 0 && spawnedEnemies.Count <= 0)
        {
            PoolEnemy enemy = EnemyManager.Instance.GetAssaultEnemy();
            enemy.spawner = this;
            enemy.go.transform.position = transform.position;
            spawnedEnemies.Add(enemy);
        }
        timer -= Time.deltaTime;
    }

    public void OnEnemyDestroy(PoolEnemy enemy)
    {
        spawnedEnemies.Remove(enemy);
        timer = timeToSpawn;
    }
}