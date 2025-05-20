using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultEnemy : Enemy
{
    public static event Action<Vector3> onAssaultEnemyKill;

    [SerializeField] private AssaultEnemyData assaultEnemyData;
    [SerializeField] private SpawnPoints spawnPoints;
    [SerializeField] private Transform cannonLeft;
    [SerializeField] private Transform cannonRight;
    private Rigidbody body = null;
    private Rigidbody target = null;
    private SteeringBehavior arrive;
    private SteeringBehavior face;
    private float timeToShoot = 2.0f;
    private float shootTimer = 0;
    private List<SmallBullet> spawnedBullets;
    private bool isTargetSet = false;

    protected override void OnAwaken() 
    {
        DroneShoot.onEnemyDamage += OnDamage;

        body = GetComponent<Rigidbody>();
        arrive = new Arrive(body, null, 10, 20, 15, 40, 0.25f);
        face = new Face(body, null, 6.0f, 6.0f, Mathf.PI*0.1f, Mathf.PI*0.4f, 0.01f);
        spawnedBullets = new List<SmallBullet>();
        isTargetSet = false;
    }

    private void OnDestroy()
    {
        DroneShoot.onEnemyDamage -= OnDamage;
    }

    public override void OnGet()
    {
        base.OnGet();
        transform.position = spawnPoints.GetRandomPoint();
        shootTimer = timeToShoot;
    }

    public override void OnRelease()
    {
        foreach(SmallBullet bullet in spawnedBullets)
        {
            BulletSpawner.Instance.Release(bullet);
        }
        spawnedBullets.Clear();
        base.OnRelease();
    }

    private void Update()
    {        
        if(shootTimer <= 0.0f)
        {
            
            SmallBullet leftBullet = BulletSpawner.Instance.Spawn<SmallBullet>();
            SmallBullet rightBullet = BulletSpawner.Instance.Spawn<SmallBullet>();
            leftBullet.transform.position = cannonLeft.position;
            leftBullet.transform.rotation = cannonLeft.rotation;
            rightBullet.transform.position = cannonRight.position;
            rightBullet.transform.rotation = cannonRight.rotation;
            spawnedBullets.Add(leftBullet);
            spawnedBullets.Add(rightBullet);
            StartCoroutine(BulletUpdate(leftBullet));
            StartCoroutine(BulletUpdate(rightBullet));
            shootTimer = timeToShoot;
        }
        shootTimer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            if (!isTargetSet)
            {
                arrive.SetTarget(target);
                face.SetTarget(target);
                isTargetSet = true;
            }
            SteeringOutput arriveSteering = arrive.GetSteering();
            SteeringOutput faceSteering = face.GetSteering();
            body.AddForce(arriveSteering.linear, ForceMode.Acceleration);
            body.AddTorque(faceSteering.angular, ForceMode.Acceleration);
        }
    }



    protected override void OnDamage(GameObject gameObject)
    {
        if (gameObject != this.gameObject) return;
        base.OnDamage(gameObject);
        if (life == 0)
        {
            onAssaultEnemyKill?.Invoke(transform.position);
        }
    }

    private IEnumerator BulletUpdate(SmallBullet bullet)
    {
        Vector3 velocity  = bullet.transform.forward * 20.0f;
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

    public void SetTarget(Rigidbody target)
    {
        this.target = target;
    }
}
