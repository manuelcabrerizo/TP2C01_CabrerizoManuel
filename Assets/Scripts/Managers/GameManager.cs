using UnityEngine;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [SerializeField] private DroneState drone;
    DroneMovement droneMovement;
    DroneShoot droneShoot;
    [SerializeField] CameraMovement cameraMovement;

    public CitizenSpawner citizenSpawner;
    public BulletSpawner playerBulletSpawner;
    public BulletSpawner alienBulletSpawner;

    private int score = 0;
    private int alienAlive = 0;
    private int citzensKill = 0;
    private int aliensKill = 0;

    private StateMachine fsm;
    private CountDownState countDownState;
    private PlayingState playingState;
    private GameOverState gameOverState;
    private WinState winState;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        // The singletons needs to be initialized in this order
        EventManager.Instance.Init();
        UIManager.Instance.Init();
        EnemyManager.Instance.Init();
    }

    private void Start()
    {
        fsm = new StateMachine();
        countDownState = new CountDownState();
        playingState = new PlayingState();
        gameOverState = new GameOverState();
        winState = new WinState();

        droneMovement = drone.GetComponent<DroneMovement>();
        droneShoot = drone.GetComponent<DroneShoot>();
    }

    public void Update()
    {
        fsm.Update(Time.deltaTime);
    }

    public void StartNewGame(LevelData data)
    {
        score = 0;
        EventManager.Instance.onScoreChange.Invoke(score);
        alienAlive = 0;
        EventManager.Instance.onAlienAliveChange.Invoke(alienAlive);
        citzensKill = 0;
        EventManager.Instance.onCitizensKilledChange.Invoke(citzensKill);
        aliensKill = 0;
        EventManager.Instance.onAliensKilledChange.Invoke(aliensKill);

        drone.Reset();
        playerBulletSpawner.Clear();
        alienBulletSpawner.Clear();
        citizenSpawner.Clear();
        citizenSpawner.SpawnEnemies(data.citizenCount);

        SetCountDownState();
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
        EventManager.Instance.onAlienAliveChange.Invoke(alienAlive);
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
        EventManager.Instance.onScoreChange.Invoke(score);
    }

    public void PlayerKill()
    {
        SetGameOverState();
    }

    public void CitizenKill()
    {
        citzensKill++;
        AddToScore(-2);
        EventManager.Instance.onCitizensKilledChange.Invoke(citzensKill);
    }

    public void AlienKill()
    {
        alienAlive--;
        EventManager.Instance.onAlienAliveChange.Invoke(alienAlive);

        aliensKill++;
        AddToScore(5);
        EventManager.Instance.onAliensKilledChange.Invoke(aliensKill);

        if(alienAlive == 0)
        {
            SetWinState();
        }
    }

    public Vector3 GetPlayerPosition()
    {
        return drone.transform.position;
    }
}
