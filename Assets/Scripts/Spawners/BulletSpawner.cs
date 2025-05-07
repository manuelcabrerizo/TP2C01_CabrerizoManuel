using UnityEngine;

public class BulletSpawner :  Spawner<BulletSpawner, Bullet>
{
    // TODO: move this to a scriptable object
    [SerializeField] private DroneBullet droneBulletPrefab;
    [SerializeField] private AlienBullet alienBulletPrefab;
    [SerializeField] private SmallBullet smallBulletPrefab;
    [SerializeField] private int initialDroneBulletCount = 10;
    [SerializeField] private int initialAlienBulletCount = 50;
    [SerializeField] private int initialSmallBulletCount = 50;
    private Rigidbody prefabBody;

    protected override void OnAwaken () 
    {
        prefabBody = droneBulletPrefab.GetComponent<Rigidbody>();
    }

    private void Start()
    {
        poolManager.InitPool(droneBulletPrefab, transform, initialDroneBulletCount);
        poolManager.InitPool(alienBulletPrefab, transform, initialAlienBulletCount);
        poolManager.InitPool(smallBulletPrefab, transform, initialSmallBulletCount);
    }

    public float GetBulletMass()
    {
        return prefabBody.mass;
    }
}
