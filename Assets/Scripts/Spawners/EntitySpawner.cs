using UnityEngine;

public class EntitySpawner : Spawner<EntitySpawner, Entity>
{
    [SerializeField] private Citizen citizenPrefab;
    [SerializeField] private AssaultEnemy assaultEnemyPrefab;
    [SerializeField] private ReconEnemy reconEnemyPrefab;
    [SerializeField] private int initialCitizenCount = 200;
    [SerializeField] private int initialAssaultEnemyCount = 5;
    [SerializeField] private int initialReconEnemyCount = 5;

    protected override void OnAwaken () 
    {
        PoolManager.Instance.InitPool(citizenPrefab, transform, initialCitizenCount);
        PoolManager.Instance.InitPool(assaultEnemyPrefab, transform, initialAssaultEnemyCount);
        PoolManager.Instance.InitPool(reconEnemyPrefab, transform, initialReconEnemyCount);
        Entity.onEntityRelease += OnEntityRelease;
    }

    protected override void OnDestroyed()
    {
        Entity.onEntityRelease -= OnEntityRelease;
    }

    private void OnEntityRelease(Entity entity)
    {
        Entity test = null;
        if(test = entity as Citizen)
        {
            PoolManager.Instance.Release((Citizen)entity);
        }
        else if(test = entity as AssaultEnemy)
        {
            PoolManager.Instance.Release((AssaultEnemy)entity);
        }
        else if(test = entity as ReconEnemy)
        {
            PoolManager.Instance.Release((ReconEnemy)entity);
        }
    }
}