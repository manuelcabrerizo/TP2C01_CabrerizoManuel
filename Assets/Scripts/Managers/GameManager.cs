using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    public static event Action<int> onAlienAliveChange;
    public static event Action<int> onScoreChange;
    public static event Action<int> onCitizensKilledChange;
    public static event Action<int> onAliensKilledChange;

    [SerializeField] private DroneState drone;
    DroneMovement droneMovement;
    DroneShoot droneShoot;
    [SerializeField] CameraMovement cameraMovement;

    private int score = 0;
    private int alienAlive = 0;
    private int citzensKill = 0;
    private int aliensKill = 0;

    [SerializeField] private float timeToSpawnEnemies = 20.0f;
    private List<AssaultEnemy> assaultEnemySpawned;
    private List<ReconEnemy> reconEnemySpawned;

    private StateMachine fsm;
    private CountDownState countDownState;
    private PlayingState playingState;
    private GameOverState gameOverState;
    private WinState winState;
    
    private void Start()
    {
        fsm = new StateMachine();
        countDownState = new CountDownState();
        playingState = new PlayingState();
        gameOverState = new GameOverState();
        winState = new WinState();

        droneMovement = drone.GetComponent<DroneMovement>();
        droneShoot = drone.GetComponent<DroneShoot>();

        assaultEnemySpawned = new List<AssaultEnemy>();
        reconEnemySpawned = new List<ReconEnemy>();
    }

    public void Update()
    {
        fsm.Update(Time.deltaTime);
    }

    public void StartNewGame(LevelData data)
    {
        score = 0;
        onScoreChange?.Invoke(score);
        alienAlive = 0;
        onAlienAliveChange?.Invoke(alienAlive);
        citzensKill = 0;
        onCitizensKilledChange?.Invoke(citzensKill);
        aliensKill = 0;
        onAliensKilledChange?.Invoke(aliensKill);

        ResetGm();
        for(int i = 0; i < data.citizenCount; ++i)
        {
            EntitySpawner.Instance.Spawn<Citizen>();
        }

        EntitySpawner.Instance.Spawn<AssaultEnemy>();
        EntitySpawner.Instance.Spawn<AssaultEnemy>();
        EntitySpawner.Instance.Spawn<ReconEnemy>();
        EntitySpawner.Instance.Spawn<ReconEnemy>();

        SetCountDownState();
    }

    public void ResetGm()
    {
        fsm.Clear();
        drone.ResetDrone();
        BulletSpawner.Instance.Clear<DroneBullet>();
        BulletSpawner.Instance.Clear<AlienBullet>();
        BulletSpawner.Instance.Clear<SmallBullet>();
        BulletSpawner.Instance.Clear<DroneSmallBullet>();
        EntitySpawner.Instance.Clear<Citizen>();
        EntitySpawner.Instance.Clear<AssaultEnemy>();
        EntitySpawner.Instance.Clear<ReconEnemy>();
    }

    public void SetCountDownState()
    {
        droneMovement.enabled = false;
        droneShoot.enabled = false;
        cameraMovement.enabled = true;
        fsm.ChangeState(countDownState);
    }

    public void SetPlayingState()
    {
        droneMovement.enabled = true;
        droneShoot.enabled = true;
        cameraMovement.enabled = true;
        fsm.ChangeState(playingState);
    }

    public void SetWinState()
    {
        droneMovement.enabled = false;
        droneShoot.enabled = false;
        cameraMovement.enabled = false;
        fsm.ChangeState(winState);
    }

    public void SetGameOverState()
    {
        droneMovement.enabled = false;
        droneShoot.enabled = false;
        cameraMovement.enabled = false;
        fsm.ChangeState(gameOverState);
    }

    public void AlienHasSpawn()
    {
        alienAlive++;
        onAlienAliveChange?.Invoke(alienAlive);
    }

    public bool ShouldSpawnAlien()
    {
        LevelData levelData = LevelManager.Instance.GetCurrentLevelData();
        int alienCount = (int)(levelData.citizenCount * levelData.alienPercentage);
        return alienAlive  < alienCount;
    }

    private void AddToScore(int value)
    {
        score += value;
        if(score < 0)
        {
            score = 0;
        }
        onScoreChange?.Invoke(score);
    }

    public void PlayerKill()
    {
        SetGameOverState();
    }

    public void CitizenKill()
    {
        citzensKill++;
        AddToScore(-2);
        onCitizensKilledChange?.Invoke(citzensKill);
    }

    public void AlienKill()
    {
        alienAlive--;
        onAlienAliveChange?.Invoke(alienAlive);

        aliensKill++;
        AddToScore(5);
        onAliensKilledChange?.Invoke(aliensKill);

        if(alienAlive == 0)
        {
            SetWinState();
        }
    }

    public Vector3 GetPlayerPosition()
    {
        return drone.transform.position;
    }

    public Rigidbody GetPlayerBody()
    {
        return drone.GetComponent<Rigidbody>();
    }
}
