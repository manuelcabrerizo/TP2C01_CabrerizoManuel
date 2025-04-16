using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    static public EnemyManager Instance;

    [SerializeField] private EnemyManagerData enemyManagerData;

    private List<PoolEnemy> enemies;
    private EnemyPool pool;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void Init()
    {
        pool = GetComponent<EnemyPool>();
        enemies = new List<PoolEnemy>();
        EventManager.Instance.onEnemyHit.AddListener(OnEnemyHit);
    }
   
    public PoolEnemy GetReconEnemy()
    {
        PoolEnemy enemy = pool.GetReconEnemy();
        enemies.Add(enemy);
        return enemy;
    }

    public PoolEnemy GetAssaultEnemy()
    {
        PoolEnemy enemy = pool.GetAssaultEnemy();
        enemies.Add(enemy);
        return enemy;
    }

    public PoolEnemy GetRandomEnemy()
    {
        PoolEnemy enemy = pool.GetRandomEnemy();
        enemies.Add(enemy);
        return enemy;
    }

    private void OnEnemyHit(RaycastHit hit, Ray ray)
    {
        GameObject go = hit.collider.gameObject;
        PoolEnemy poolEnemy = pool.GetPoolEnemy(go);
        Enemy enemy = poolEnemy.enemy;

        enemy.State.TakeDamage(1);
        if (enemy.State.GetLife() <= 0)
        {
            poolEnemy.spawner.OnEnemyDestroy(poolEnemy);
            enemies.Remove(poolEnemy);
            pool.ReleaseEnemy(poolEnemy);
        }
        else 
        {
            enemy.Body.AddForce(ray.direction * 40.0f, ForceMode.Impulse);
        }
    }
}
