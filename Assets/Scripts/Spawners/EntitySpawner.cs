using UnityEngine;

public class EntitySpawner : Spawner<EntitySpawner, Entity>
{
    // TODO: move this to a scriptable object
    [SerializeField] private Citizen citizenPrefab;
    [SerializeField] private AssaultEnemy assaultEnemyPrefab;
    [SerializeField] private ReconEnemy reconEnemyPrefab;
    [SerializeField] private int initialCitizenCount = 200;
    [SerializeField] private int initialAssaultEnemyCount = 5;
    [SerializeField] private int initialReconEnemyCount = 5;

    private void Start()
    {
        poolManager.InitPool(citizenPrefab, transform, initialCitizenCount);
        poolManager.InitPool(assaultEnemyPrefab, transform, initialAssaultEnemyCount);
        poolManager.InitPool(reconEnemyPrefab, transform, initialReconEnemyCount);
        EventManager.Instance.onEntityRelease.AddListener(OnEntityRelease);
    }

    // TODO: mabye this is no necesary 
    private void OnEntityRelease(Entity entity)
    {
        Entity test = null;
        if(test = entity as Citizen)
        {
            poolManager.Release<Citizen>((Citizen)entity);
        }
        else if(test = entity as AssaultEnemy)
        {
            poolManager.Release<AssaultEnemy>((AssaultEnemy)entity);
        }
        else if(test = entity as ReconEnemy)
        {
            poolManager.Release<ReconEnemy>((ReconEnemy)entity);
        }
    }
}