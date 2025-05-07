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
            DroneBullet bullet = BulletSpawner.Instance.Spawn<DroneBullet>();
            bullet.transform.position = gun.transform.position;
            bullet.transform.rotation = Quaternion.LookRotation(transform.forward, transform.up);
            StartCoroutine(BulletUpdate(bullet));
        }
    }

    public void PredictProjectile()
    {
        Vector3 position = gun.transform.position;
        Vector3 velocity = playerData.BulletSpeed * transform.forward / BulletSpawner.Instance.GetBulletMass();
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

    private IEnumerator BulletUpdate(DroneBullet bullet)
    {
        Vector3 velocity = playerData.BulletSpeed * transform.forward / bullet.GetMass();
        Vector3 acceleration = Physics.gravity;
        float timer = 5.0f;
        while (bullet.isActiveAndEnabled && timer > 0.0f)
        {
            float time = Time.deltaTime;
            bullet.Move(velocity * time + (acceleration * (time * time)));
            velocity += acceleration * time;
            yield return new WaitForEndOfFrame();
            timer -= time;
        }
        if (bullet.isActiveAndEnabled)
        {
            BulletSpawner.Instance.Release(bullet);
        }
    }
}
