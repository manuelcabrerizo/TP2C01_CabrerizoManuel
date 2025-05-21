using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    public static event Action<int> onAlienAliveChange;
    public static event Action<int> onScoreChange;
    public static event Action<int> onCitizensKilledChange;
    public static event Action<int> onAliensKilledChange;

    private LevelData levelData;

    [SerializeField] private Drone drone;
    private Rigidbody droneBody;
    private DroneMovement droneMovement;
    private DroneShoot droneShoot;
    [SerializeField] private CameraMovement cameraMovement;

    private int score = 0;
    private int alienAlive = 0;
    private int citzensKill = 0;
    private int aliensKill = 0;

    [SerializeField] private float timeToSpawnEnemies = 20.0f;
    private float spawnAssaultEnemyTimer = 0;
    private float spawnReconEnemyTimer = 0;
    private int assaultEnemySpawnedCount = 0;
    private int reconEnemySpawnedCount = 0;

    private StateMachine fsm;
    private CountDownState countDownState;
    private PlayingState playingState;
    private GameOverState gameOverState;
    private WinState winState;

    protected override void OnAwaken()
    {
        LevelManager.onStartNewGame += OnStartNewGame;
        UIManager.onNextOrResetButtonClick += SetCountDownState;
        UIManager.onMenuButtonClick += OnGoToMenu;
        Drone.onPlayerKill += OnPlayerKill;
        CitizenOnHit.onAlienKill += OnAlienKill;
        CitizenOnHit.onCitizenKill += OnCitizenKill;
        CountDownState.onCountDownEnd += SetPlayingState;
        AssaultEnemy.onAssaultEnemyKill += OnAssaultEnemyKill;
        ReconEnemy.onReconEnemyKill += OnReconEnemyKill;
        Coin.onCoinPickup += OnCoinPickup;
    }

    protected override void OnDestroyed()
    {
        LevelManager.onStartNewGame -= OnStartNewGame;
        UIManager.onNextOrResetButtonClick -= SetCountDownState;
        UIManager.onMenuButtonClick -= OnGoToMenu;
        Drone.onPlayerKill -= OnPlayerKill;
        CitizenOnHit.onAlienKill -= OnAlienKill;
        CitizenOnHit.onCitizenKill -= OnCitizenKill;
        CountDownState.onCountDownEnd -= SetPlayingState;
        AssaultEnemy.onAssaultEnemyKill -= OnAssaultEnemyKill;
        ReconEnemy.onReconEnemyKill -= OnReconEnemyKill;
        Coin.onCoinPickup -= OnCoinPickup;
    }

    private void Start()
    {
        fsm = new StateMachine();
        countDownState = new CountDownState();
        playingState = new PlayingState();
        gameOverState = new GameOverState();
        winState = new WinState();

        droneBody = drone.GetComponent<Rigidbody>();
        droneMovement = drone.GetComponent<DroneMovement>();
        droneShoot = drone.GetComponent<DroneShoot>();

        assaultEnemySpawnedCount = 0;
        reconEnemySpawnedCount = 0;
        spawnAssaultEnemyTimer = timeToSpawnEnemies;
        spawnReconEnemyTimer = timeToSpawnEnemies;
    }

    private void Update()
    {
        fsm.Update(Time.deltaTime);
        // if there is space for a enemy, spawn one
        if (assaultEnemySpawnedCount < levelData.assaultEnemyCount)
        {
            if (spawnAssaultEnemyTimer <= 0.0f)
            {
                AssaultEnemy enemy = EntitySpawner.Instance.Spawn<AssaultEnemy>();
                enemy.SetTarget(droneBody);
                assaultEnemySpawnedCount++;
                spawnAssaultEnemyTimer = timeToSpawnEnemies;
            }
            
            spawnAssaultEnemyTimer -= Time.deltaTime;
        }
        // if there is space for a enemy, spawn one
        if (reconEnemySpawnedCount < levelData.reconEnemyCount)
        {
            if (spawnReconEnemyTimer <= 0.0f)
            {
                EntitySpawner.Instance.Spawn<ReconEnemy>();
                reconEnemySpawnedCount++;   
                spawnReconEnemyTimer = timeToSpawnEnemies;
            }
            spawnReconEnemyTimer -= Time.deltaTime;
        }
    }

    private void OnStartNewGame(LevelData data)
    {
        levelData = data;
        score = 0;
        onScoreChange?.Invoke(score);
        alienAlive = 0;
        onAlienAliveChange?.Invoke(alienAlive);
        citzensKill = 0;
        onCitizensKilledChange?.Invoke(citzensKill);
        aliensKill = 0;
        onAliensKilledChange?.Invoke(aliensKill);

        ResetGame();

        // Spawn all the citizen / aliens
        for(int i = 0; i < data.citizenCount; ++i)
        {
            Citizen citizen = EntitySpawner.Instance.Spawn<Citizen>();
            ImpostorState impostorState = citizen.GetComponent<ImpostorState>();
            impostorState.SetTarget(drone.transform);
            if(ShouldSpawnAlien())
            {
                citizen.MakeImpostor();
                AlienHasSpawn();
            }
        }

        // Spawn all the assault enemies
        for (int i = 0; i < data.assaultEnemyCount; ++i)
        {
            AssaultEnemy enemy = EntitySpawner.Instance.Spawn<AssaultEnemy>();
            enemy.SetTarget(droneBody);
            assaultEnemySpawnedCount++;
        }

        // Spawn all the recon enemies
        for (int i = 0; i < data.reconEnemyCount; ++i)
        {
            EntitySpawner.Instance.Spawn<ReconEnemy>();
            reconEnemySpawnedCount++;
        }

        SetCountDownState();
    }

    private void OnGoToMenu()
    {
        ResetGame();
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("MainMenu");
    }

    private void ResetGame()
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
        PickableSpawner.Instance.Clear<Coin>();
        PickableSpawner.Instance.Clear<Health>();
        assaultEnemySpawnedCount = 0;
        reconEnemySpawnedCount = 0;
        spawnAssaultEnemyTimer = timeToSpawnEnemies;
        spawnReconEnemyTimer = timeToSpawnEnemies;
    }

    private void SetCountDownState()
    {
        droneMovement.enabled = false;
        droneShoot.enabled = false;
        cameraMovement.enabled = true;
        fsm.ChangeState(countDownState);
    }

    private void SetPlayingState()
    {
        droneMovement.enabled = true;
        droneShoot.enabled = true;
        cameraMovement.enabled = true;
        fsm.ChangeState(playingState);
    }

    private void SetWinState()
    {
        droneMovement.enabled = false;
        droneShoot.enabled = false;
        cameraMovement.enabled = false;
        fsm.ChangeState(winState);
    }

    private void SetGameOverState()
    {
        droneMovement.enabled = false;
        droneShoot.enabled = false;
        cameraMovement.enabled = false;
        fsm.ChangeState(gameOverState);
    }

    private void AlienHasSpawn()
    {
        alienAlive++;
        onAlienAliveChange?.Invoke(alienAlive);
    }

    private bool ShouldSpawnAlien()
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

    private void OnPlayerKill()
    {
        SetGameOverState();
    }

    private void OnCitizenKill()
    {
        citzensKill++;
        AddToScore(-2);
        onCitizensKilledChange?.Invoke(citzensKill);
    }

    private void OnAlienKill(Vector3 position)
    {
        int cointCount = UnityEngine.Random.Range(6, 12);
        for (int i = 0; i < cointCount; ++i)
        {
            Coin coin = PickableSpawner.Instance.Spawn<Coin>();
            coin.StartAnimation(position);
        }

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

    private void OnAssaultEnemyKill(Vector3 position)
    {
        int healthCount = UnityEngine.Random.Range(3, 6);
        for (int i = 0; i < healthCount; ++i)
        {
            Health health = PickableSpawner.Instance.Spawn<Health>();
            health.StartAnimation(position);
        }

        assaultEnemySpawnedCount--;
    }

    private void OnReconEnemyKill(Vector3 position)
    {
        int healthCount = UnityEngine.Random.Range(3, 6);
        for (int i = 0; i < healthCount; ++i)
        {
            Health health = PickableSpawner.Instance.Spawn<Health>();
            health.StartAnimation(position);
        }

        reconEnemySpawnedCount--;
    }

    private void OnCoinPickup()
    {
        AddToScore(1);
    }
}
