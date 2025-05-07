using UnityEngine;

public abstract class Spawner<SpawnerType, SpawnObjectBaseType> : MonoBehaviourSingleton<SpawnerType> 
    where SpawnerType : MonoBehaviourSingleton<SpawnerType>
    where SpawnObjectBaseType : MonoBehaviour, IPooleable
{
    [SerializeField] protected PoolManager poolManager;
    public T Spawn<T>() where T : SpawnObjectBaseType
    {
        T go = poolManager.Get<T>(transform);
        return go;
    }
    public void Release<T>(T go) where T : SpawnObjectBaseType
    {
        poolManager.Release(go);
    }
    public void Clear<T>() where T : SpawnObjectBaseType
    {
        poolManager.Clear<T>();
    }
}