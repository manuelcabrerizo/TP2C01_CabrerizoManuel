using System.Collections;
using UnityEngine;

public class DroneShoot : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private ParticleSystem hitParticles;

    [SerializeField] GameObject gun;

    BulletSpawner spawner;


    private void Awake()
    {
	    if(cam == null)
	    {
	        cam = Camera.main;
	    }

        spawner = GetComponent<BulletSpawner>();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SpawnedBullet bullet = spawner.SpawnBullet();
            bullet.go.transform.position = transform.position;
            bullet.go.transform.rotation = Quaternion.LookRotation(transform.forward, transform.up);

            Vector3 shootPosition = cam.transform.position + cam.transform.forward * playerData.ShootDistance;
            StartCoroutine(BulletUpdate(bullet, gun.transform.position, shootPosition));

            Ray cameraRay = new Ray(cam.transform.position, cam.transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(cameraRay, out hit, playerData.ShootDistance, enemyMask))
            {
                hitParticles.transform.position = hit.point;
                hitParticles.Play();
                EventManager.Instance.onEnemyHit.Invoke(hit, cameraRay);
            }
        }
    }

    private IEnumerator BulletUpdate(SpawnedBullet bullet, Vector3 startPosition, Vector3 endPosition)
    {
        float t = 0.0f;
        while (t <= 1.0f && bullet.active)
        {
            bullet.go.transform.position = Vector3.Lerp(startPosition, endPosition, t);
            yield return new WaitForEndOfFrame();
            t += playerData.BulletSpeed * Time.deltaTime;
        }

        if (bullet.active)
        {
            spawner.ReleaseBullet(bullet);
        }
    }
}
