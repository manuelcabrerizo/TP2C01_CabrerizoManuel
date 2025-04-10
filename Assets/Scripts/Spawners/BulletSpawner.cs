using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SpawnedBullet
{
    public GameObject go;
    public ParticleSystem particleSystem;
    public bool active;
}

public class BulletSpawner : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;

    private IObjectPool<SpawnedBullet> bulletPool;
    private Dictionary<GameObject, SpawnedBullet> goToBullet;

    // throw an exception if we try to return an existing item,
    // already in the pool
    [SerializeField] private bool collectionCheck = true;
    // extra options to control the pool capacity and maximun size
    [SerializeField] private int defaultCapacity = 20;
    [SerializeField] private int maxSize = 100;


    private void Awake()
    {
        bulletPool = new ObjectPool<SpawnedBullet>(
            OnCreatePooledObject,
            OnGetFromPool,
            OnReleaseToPool,
            OnDestroyPooledObject,
            collectionCheck, defaultCapacity, maxSize);

        goToBullet = new Dictionary<GameObject, SpawnedBullet>();
    }


    public SpawnedBullet SpawnBullet()
    {
        SpawnedBullet bullet = bulletPool.Get();
        return bullet;
    }

    public void ReleaseBullet(SpawnedBullet bullet)
    {
        bulletPool.Release(bullet);
    }

    public SpawnedBullet GetSpawnBullet(GameObject go)
    {
        return goToBullet[go];
    }

    private SpawnedBullet OnCreatePooledObject()
    {
        SpawnedBullet bullet = new SpawnedBullet();
        bullet.go = Instantiate(bulletPrefab);
        bullet.go.SetActive(false);
        bullet.particleSystem = bullet.go.GetComponentInChildren<ParticleSystem>();
        bullet.active = false;
        goToBullet.Add(bullet.go, bullet);
        return bullet;
    }

    private void OnReleaseToPool(SpawnedBullet bullet)
    {
        bullet.particleSystem.Stop();
        bullet.active = false;
        bullet.go.SetActive(bullet.active);
    }

    private void OnGetFromPool(SpawnedBullet bullet)
    {
        bullet.particleSystem.Play();
        bullet.active = true;
        bullet.go.SetActive(bullet.active);
    }

    private void OnDestroyPooledObject(SpawnedBullet bullet)
    {
        Destroy(bullet.go);
    }
}
