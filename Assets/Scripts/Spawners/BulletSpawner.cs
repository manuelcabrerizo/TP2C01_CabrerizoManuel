using UnityEngine;

public class BulletSpawner : MonoBehaviourSingleton<BulletSpawner>
{
    [SerializeField] private PoolManager poolManager;
    [SerializeField] private DroneBullet droneBulletPrefab;
    [SerializeField] private AlienBullet alienBulletPrefab;
    private Rigidbody prefabBody;

    protected override void OnAwaken () 
    {
        prefabBody = droneBulletPrefab.GetComponent<Rigidbody>();
    }

    private void Start()
    {
        poolManager.InitPool(droneBulletPrefab, transform, 10);
        poolManager.InitPool(alienBulletPrefab, transform, 100);
    }

    public T Spawn<T>() where T : Bullet
    {
        T bullet = poolManager.Get<T>(transform);
        return bullet;
    }

    public void Release<T>(T bullet) where T : Bullet
    {
        poolManager.Release(bullet);
    }

    public void Clear<T>() where T : Bullet
    {
        poolManager.Clear<T>();
    }

    public float GetBulletMass()
    {
        return prefabBody.mass;
    }
}
