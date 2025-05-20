using System;
using System.Collections;
using UnityEngine;

public enum WeaponType
{
    RocketLauncher,
    BlasterCannon
}

public class DroneShoot : MonoBehaviour
{
    public static event Action<GameObject> onEnemyDamage;
    public static event Action<GameObject> onCitizenDamage;

    [SerializeField] private PlayerData playerData;
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private LayerMask citizenMask;

    [SerializeField] GameObject gun;
    // Line renderer
    private LineRenderer lineRenderer;
    private int linePoints = 64;
    private float timeBetweenPoints = 0.05f;

    WeaponType weaponType;
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
        weaponType = WeaponType.RocketLauncher;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private void Update()
    {
        SelectWeapon();
        switch(weaponType)
        {
            case WeaponType.RocketLauncher:
            {
                UpdateRocketLauncher();
            } break;
            case WeaponType.BlasterCannon:
            {
                UpdateBlasterCannon();
            } break;
        }

        if(Input.GetKeyDown(KeyCode.P))
        {
            isPredictionActive = !isPredictionActive;
            lineRenderer.enabled = isPredictionActive;
        }
    }

    public void SelectWeapon()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            weaponType = WeaponType.RocketLauncher;
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            weaponType = WeaponType.BlasterCannon;
        }
    }

    public void UpdateRocketLauncher()
    {
        if(isPredictionActive)
        {
            PredictRocketLauncher();
        }
        if (Input.GetMouseButtonDown(0))
        {
            DroneBullet bullet = BulletSpawner.Instance.Spawn<DroneBullet>();
            bullet.transform.position = gun.transform.position;
            bullet.transform.rotation = Quaternion.LookRotation(transform.forward, transform.up);
            StartCoroutine(BulletUpdate(bullet));
        }
    }

    public void UpdateBlasterCannon()
    {
        if(isPredictionActive)
        {
            PredictBlasterCannon();
        }
        if (Input.GetMouseButtonDown(0))
        {
            DroneSmallBullet bullet = BulletSpawner.Instance.Spawn<DroneSmallBullet>();
            bullet.transform.position = gun.transform.position;
            bullet.transform.rotation = Quaternion.LookRotation(transform.forward, transform.up);
            StartCoroutine(SmallBulletUpdate(bullet));

            Vector3 startPosition = bullet.transform.position;
            Vector3 finalPosition = cam.transform.position + cam.transform.forward * playerData.ShootDistance;
            finalPosition += Vector3.up * 1.0f;
            Vector3 direction = (finalPosition - startPosition).normalized;
            Ray cameraRay = new Ray(startPosition, direction);
            RaycastHit hit;
            if (Physics.Raycast(cameraRay, out hit, playerData.ShootDistance, enemyMask))
            {
                onEnemyDamage?.Invoke(hit.collider.gameObject);
            }
            if (Physics.Raycast(cameraRay, out hit, playerData.ShootDistance, citizenMask))
            {
                onCitizenDamage?.Invoke(hit.collider.gameObject);
            }
        }
    }

    public void PredictRocketLauncher()
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

    public void PredictBlasterCannon()
    {
        Vector3 position = gun.transform.position;
        Vector3 finalPosition = cam.transform.position + cam.transform.forward * playerData.ShootDistance;
        finalPosition += Vector3.up * 1.0f;
        Vector3 direction = (finalPosition - position).normalized;
        int i = 0;
        lineRenderer.SetPosition(i, position);
        float increment = playerData.ShootDistance / linePoints;
        for(i = 1; i < linePoints; i++)
        {
            position += direction * increment;
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

    private IEnumerator SmallBulletUpdate(DroneSmallBullet bullet)
    {
        Vector3 startPosition = bullet.transform.position;
        Vector3 finalPosition = cam.transform.position + cam.transform.forward * playerData.ShootDistance;
        finalPosition += Vector3.up * 1.0f;
        float t = 0.0f;
        while (t <= 1.0f && bullet.isActiveAndEnabled)
        {
            bullet.transform.position = Vector3.Lerp(startPosition, finalPosition, t);
            yield return new WaitForEndOfFrame();
            t += 200.0f*(Time.deltaTime/playerData.ShootDistance);
        }
        if (bullet.isActiveAndEnabled)
        {
            BulletSpawner.Instance.Release(bullet);
        }
    }
}
