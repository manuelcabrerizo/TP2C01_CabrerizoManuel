using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    public static event Action<int> onAlienAliveChange;
    public static event Action<int> onScoreChange;
    public static event Action<int> onCitizensKilledChange;
    public static event Action<int> onAliensKilledChange;

    [SerializeField] private DroneState drone;
    private Rigidbody droneBody;
    private DroneMovement droneMovement;
    private DroneShoot droneShoot;
    [SerializeField] private CameraMovement cameraMovement;

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

    protected override void OnAwaken()
    {
        LevelManager.onStartNewGame += OnStartNewGame;
        UIManager.onNextOrResetButtonClick += SetCountDownState;
        UIManager.onMenuButtonClick += OnGoToMenu;
        DroneState.onPlayerKill += OnPlayerKill;
        CitizenOnHit.onAlienKill += OnAlienKill;
        CitizenOnHit.onCitizenKill += OnCitizenKill;
        CountDownState.onCountDownEnd += SetPlayingState;
    }

    protected override void OnDestroyed()
    {
        LevelManager.onStartNewGame -= OnStartNewGame;
        UIManager.onNextOrResetButtonClick -= SetCountDownState;
        UIManager.onMenuButtonClick -= OnGoToMenu;
        DroneState.onPlayerKill -= OnPlayerKill;
        CitizenOnHit.onAlienKill -= OnAlienKill;
        CitizenOnHit.onCitizenKill -= OnCitizenKill;
        CountDownState.onCountDownEnd -= SetPlayingState;

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

        assaultEnemySpawned = new List<AssaultEnemy>();
        reconEnemySpawned = new List<ReconEnemy>();
    }

    private void Update()
    {
        fsm.Update(Time.deltaTime);
    }

    private void OnStartNewGame(LevelData data)
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
            Citizen citizen = EntitySpawner.Instance.Spawn<Citizen>();
            ImpostorState impostorState = citizen.GetComponent<ImpostorState>();
            impostorState.SetTarget(drone.transform);
            if(ShouldSpawnAlien())
            {
                citizen.MakeImpostor();
                AlienHasSpawn();
            }
        }

        AssaultEnemy assaultEnemy0 = EntitySpawner.Instance.Spawn<AssaultEnemy>();
        AssaultEnemy assaultEnemy1 = EntitySpawner.Instance.Spawn<AssaultEnemy>();
        assaultEnemy0.SetTarget(droneBody);
        assaultEnemy1.SetTarget(droneBody);

        EntitySpawner.Instance.Spawn<ReconEnemy>();
        EntitySpawner.Instance.Spawn<ReconEnemy>();

        SetCountDownState();
    }

    private void OnGoToMenu()
    {
        ResetGm();
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("MainMenu");
    }

    private void ResetGm()
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

    private void OnAlienKill()
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
}
