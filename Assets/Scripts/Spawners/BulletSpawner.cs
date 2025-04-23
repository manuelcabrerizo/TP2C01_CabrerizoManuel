using System.Collections.Generic;
using UnityEngine;

public class SpawnedBullet
{
    public GameObject go;
    public ParticleSystem particleSystem;
    public Rigidbody body;
    public bool active;
}

public class BulletSpawner : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;

    private PoolAllocator<SpawnedBullet> bulletPool;
    private Dictionary<GameObject, SpawnedBullet> goToBullet;

    private List<SpawnedBullet> spawnedBullets;

    Rigidbody prefabBody;

    private void Awake()
    {
        spawnedBullets = new List<SpawnedBullet>();
        bulletPool = new PoolAllocator<SpawnedBullet>(OnCreatePooledObject, OnDestroyPooledObject, OnGetFromPool, OnReleaseToPool);
        goToBullet = new Dictionary<GameObject, SpawnedBullet>();
        prefabBody = bulletPrefab.GetComponent<Rigidbody>();
    }

    public void Clear()
    {
        for(int i = 0; i < spawnedBullets.Count; ++i)
        {
            bulletPool.Release(spawnedBullets[i]);
        }
        spawnedBullets.Clear();
    }

    public SpawnedBullet SpawnBullet()
    {
        SpawnedBullet bullet = bulletPool.Get();
        spawnedBullets.Add(bullet);
        return bullet;
    }

    public void ReleaseBullet(SpawnedBullet bullet)
    {
        spawnedBullets.Remove(bullet);
        bulletPool.Release(bullet);
    }

    public SpawnedBullet GetSpawnBullet(GameObject go)
    {
        return goToBullet[go];
    }

    public float GetBulletMass()
    {   
        return prefabBody.mass;
    }

    private SpawnedBullet OnCreatePooledObject()
    {
        SpawnedBullet bullet = new SpawnedBullet();
        bullet.go = Instantiate(bulletPrefab);
        bullet.go.SetActive(false);
        bullet.particleSystem = bullet.go.GetComponentInChildren<ParticleSystem>();
        bullet.body = bullet.go.GetComponent<Rigidbody>();
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
