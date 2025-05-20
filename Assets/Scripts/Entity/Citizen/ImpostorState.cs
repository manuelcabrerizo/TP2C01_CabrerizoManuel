using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpostorState : MonoBehaviour, IState
{
    [SerializeField] private Transform shoot;
    private Transform target;

    private List<AlienBullet> spawnedBullets;

    private Citizen citizen;
    private float timeToShoot = 1.0f;
    private float shootTimer = 0;

    private void Awake()
    {
        citizen = GetComponent<Citizen>();
        spawnedBullets = new List<AlienBullet>();
    }

    public void OnEnter()
    {
        citizen.ConvertToAlien();
        shootTimer = timeToShoot;
    }

    public void OnExit()
    {
        foreach(AlienBullet bullet in spawnedBullets)
        {
            BulletSpawner.Instance.Release(bullet);
        }
        spawnedBullets.Clear();
    }

    public void OnUpdate(float dt)
    {
        if (target != null)
        {
            Vector3 toTarget = target.position - transform.position;
            toTarget.y = 0;
            toTarget.Normalize();
            transform.rotation = Quaternion.LookRotation(toTarget, transform.up);

            if (shootTimer <= 0.0f)
            {
                AlienBullet bullet = BulletSpawner.Instance.Spawn<AlienBullet>();
                bullet.transform.position = shoot.transform.position;
                spawnedBullets.Add(bullet);
                StartCoroutine(BulletUpdate(bullet));
                shootTimer = timeToShoot;
            }
            shootTimer -= dt;
        }
    }  

    public void OnFixedUpdate(float dt)
    {
    }  
    
    // TODO: move this to AlienBullet class
    private IEnumerator BulletUpdate(AlienBullet bullet)
    {
        Vector3 pos = shoot.transform.position;
        Vector3 shootPosition = target.position;
        Vector3 shootDirection = (shootPosition - pos).normalized;
        Vector3 velocity  = shootDirection * 10.0f;

        float timer = 5.0f;
        while (bullet.isActiveAndEnabled && timer > 0.0f)
        {
            bullet.transform.position += velocity * Time.deltaTime;   
            yield return new WaitForEndOfFrame();
            timer -= Time.deltaTime;
        }
        
        if (bullet.isActiveAndEnabled)
        {
            spawnedBullets.Remove(bullet);
            BulletSpawner.Instance.Release(bullet);
        }
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}
