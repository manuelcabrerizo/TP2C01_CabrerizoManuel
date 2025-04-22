using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpostorState : MonoBehaviour, IState
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform shoot;
    private Citizen citizen;
    private float timeToShoot = 1.0f;
    private float shootTimer = 0;
    private BulletSpawner spawner;

    private List<SpawnedBullet> bullets;

    private void Awake()
    {
        citizen = GetComponent<Citizen>();
        bullets = new List<SpawnedBullet>();
    }

    private void Start()
    {
        spawner = GameManager.Instance.alienBulletSpawner;
    }

    public void OnEnter()
    {
        citizen.ConvertToAlien();
        shootTimer = timeToShoot;
    }

    public void OnExit()
    {
        for(int i = 0; i < bullets.Count; ++i)
        {
            spawner.ReleaseBullet(bullets[i]);
        }
        bullets.Clear();
    }

    public void OnUpdate(float dt)
    {
        
        Vector3 playerPosition = GameManager.Instance.GetPlayerPosition();
        Vector3 toPlayer = playerPosition - transform.position;
        toPlayer.y = 0;
        toPlayer.Normalize();
        transform.rotation = Quaternion.LookRotation(toPlayer, transform.up);
        
        target.position = GameManager.Instance.GetPlayerPosition();;
        if(shootTimer <= 0.0f)
        {
            SpawnedBullet bullet = spawner.SpawnBullet();
            bullet.go.transform.SetParent(spawner.transform);
            bullet.go.transform.position = shoot.transform.position;
            bullets.Add(bullet);
            StartCoroutine(BulletUpdate(bullet));
            shootTimer = timeToShoot;
        }
        shootTimer -= dt;
    }

    public void OnFixedUpdate(float dt)
    {
    }

    private IEnumerator BulletUpdate(SpawnedBullet bullet)
    {
        Vector3 pos = shoot.transform.position;
        Vector3 shootPosition = GameManager.Instance.GetPlayerPosition();;
        Vector3 shootDirection = (shootPosition - pos).normalized;
        Vector3 velocity  = shootDirection * 10.0f;

        float timer = 5.0f;
        while (bullet.active && timer > 0.0f)
        {
            bullet.go.transform.position += velocity * Time.deltaTime;   
            yield return new WaitForEndOfFrame();
            timer -= Time.deltaTime;
        }
        
        bullets.Remove(bullet);
        if (bullet.active)
        {
            spawner.ReleaseBullet(bullet);
        }
    }
}
