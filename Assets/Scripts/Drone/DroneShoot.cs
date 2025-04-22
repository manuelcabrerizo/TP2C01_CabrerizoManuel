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
    [SerializeField] private BulletSpawner spawner;
    // Line renderer
    private LineRenderer lineRenderer;
    private int linePoints = 64;
    private float timeBetweenPoints = 0.05f;

    bool isPredictionActive = true;

    private Rigidbody body;
    private void Awake()
    {
	    if(cam == null)
	    {
	        cam = Camera.main;
	    }

        lineRenderer = GetComponent<LineRenderer>();
        body = GetComponent<Rigidbody>();

        lineRenderer.positionCount = linePoints;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            isPredictionActive = !isPredictionActive;
            lineRenderer.enabled = isPredictionActive;
        }

        if(isPredictionActive)
        {
            PredictProjectile();
        }

        if (Input.GetMouseButtonDown(0))
        {
            SpawnedBullet bullet = spawner.SpawnBullet();
            bullet.go.transform.SetParent(spawner.transform);
            bullet.go.transform.position = gun.transform.position;
            bullet.body.position = gun.transform.position;
            bullet.go.transform.rotation = Quaternion.LookRotation(transform.forward, transform.up);
            StartCoroutine(BulletUpdate(bullet));
        }
    }

    public void PredictProjectile()
    {
        Vector3 position = gun.transform.position;
        Vector3 velocity = playerData.BulletSpeed * transform.forward / spawner.GetBulletMass();
        Vector3 acceleration = Physics.gravity;

        int i = 0;
        lineRenderer.SetPosition(i, position);
        float time = timeBetweenPoints;
        for(i = 1; i < linePoints; i++)
        {
            position += velocity * time + (acceleration * time * time);
            velocity += acceleration * time;
            lineRenderer.SetPosition(i, position);
        }
    }

    private IEnumerator BulletUpdate(SpawnedBullet bullet)
    {
        Vector3 velocity = playerData.BulletSpeed * transform.forward / spawner.GetBulletMass();
        Vector3 acceleration = Physics.gravity;

        float timer = 5.0f;
        while (bullet.active && timer > 0.0f)
        {
            float time = Time.deltaTime;
            bullet.body.position += velocity * time + (acceleration * time * time); 
            velocity += acceleration * time;
            yield return new WaitForEndOfFrame();
            timer -= time;
        }

        if (bullet.active)
        {
            spawner.ReleaseBullet(bullet);
        }
    }
}
