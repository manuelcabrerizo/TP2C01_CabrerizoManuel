using System.Collections.Generic;
using UnityEngine;
public class ReconEnemySpawner : MonoBehaviour, IEnemySpawner
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float timeToSpawn = 10.0f;
    private float timer = 0.0f;

    [SerializeField] private Transform[] patrolPath;

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
            PoolEnemy poolEnemy = EnemyManager.Instance.GetReconEnemy();
            ReconEnemy enemy = (ReconEnemy)poolEnemy.enemy;
            enemy.SetPatrolPoints(patrolPath);
            poolEnemy.spawner = this;
            poolEnemy.go.transform.position = transform.position;
            spawnedEnemies.Add(poolEnemy);
        }
        timer -= Time.deltaTime;
    }

    public void OnEnemyDestroy(PoolEnemy enemy)
    {
        spawnedEnemies.Remove(enemy);
        timer = timeToSpawn;
    }
}