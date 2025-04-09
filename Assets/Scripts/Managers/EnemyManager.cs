using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private EnemyManagerData enemyManagerData;
    private List<SpawnedEnemy> enemies;
    
    [SerializeField] private EnemySpawner spawner;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private int[] spawnCount;
    private float[] timers;

    [SerializeField] private Transform[] patrolPath0;
    [SerializeField] private Transform[] patrolPath1;
    private Transform[][] patrolPaths;

    private void Awake()
    {
        enemies = new List<SpawnedEnemy>();
        spawnCount = new int[spawnPoints.Length];
        timers = new float[spawnPoints.Length];
        for (int i = 0; i < spawnPoints.Length; ++i)
        {
            spawnCount[i] = 0;
            timers[i] = enemyManagerData.TimeToSpawn;
        }

        patrolPaths = new Transform[2][];
        patrolPaths[0] = patrolPath0;
        patrolPaths[1] = patrolPath1;
    }

    private void Start()
    {
        EventManager.Instance.onEnemyHit.AddListener(OnEnemyHit);
    }

    private void Update()
    {
        for (int i = 0; i < spawnPoints.Length; ++i)
        {
            if (timers[i] <= 0.0f && spawnCount[i] <= 0)
            {
                SpawnedEnemy enemy = null;
                switch (i)
                {
                    case 0:
                    case 1:
                        {
                            enemy = spawner.SpawnReconEnemy();
                            enemy.reconBehavior.SetPatrolPoints(patrolPaths[i]);
                        } break;
                    case 2:
                    case 3:
                        {
                            enemy = spawner.SpawnAssaultEnemy();
                        } break;
                }

                if (enemy != null)
                {
                    enemy.spawnIndex = i;
                    enemy.go.transform.position = spawnPoints[i].position;
                    enemies.Add(enemy);
                    spawnCount[i]++;
                }
            }
            timers[i] -= Time.deltaTime;
        }
    }

    private void OnEnemyHit(RaycastHit hit, Ray ray)
    {
        GameObject go = hit.collider.gameObject;
        SpawnedEnemy enemy = spawner.GetSpawnEnemy(go);

        enemy.behavior.State.TakeDamage(1);
        if (enemy.behavior.State.GetLife() <= 0)
        {
            spawnCount[enemy.spawnIndex]--;
            timers[enemy.spawnIndex] = enemyManagerData.TimeToSpawn;

            enemies.Remove(enemy);
            spawner.ReleaseEnemy(enemy);
        }
        else 
        {
            enemy.behavior.Body.AddForce(ray.direction * 40.0f, ForceMode.Impulse);
        }
    }

}
