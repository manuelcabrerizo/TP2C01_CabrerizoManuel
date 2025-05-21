using UnityEngine;

public class BulletSpawner :  Spawner<BulletSpawner, Bullet>
{
    [SerializeField] private DroneBullet droneBulletPrefab;
    [SerializeField] private DroneSmallBullet droneSmallBullet;
    [SerializeField] private AlienBullet alienBulletPrefab;
    [SerializeField] private SmallBullet smallBulletPrefab;
    [SerializeField] private int initialDroneBulletCount = 15;
    [SerializeField] private int initialDroneSmallBulletCount = 15;
    [SerializeField] private int initialAlienBulletCount = 50;
    [SerializeField] private int initialSmallBulletCount = 50;
    private Rigidbody prefabBody;

    protected override void OnAwaken () 
    {
        prefabBody = droneBulletPrefab.GetComponent<Rigidbody>();
        PoolManager.Instance.InitPool(droneBulletPrefab, transform, initialDroneBulletCount);
        PoolManager.Instance.InitPool(droneSmallBullet, transform, initialDroneSmallBulletCount);
        PoolManager.Instance.InitPool(alienBulletPrefab, transform, initialAlienBulletCount);
        PoolManager.Instance.InitPool(smallBulletPrefab, transform, initialSmallBulletCount);
    }

    public float GetBulletMass()
    {
        return prefabBody.mass;
    }
}
