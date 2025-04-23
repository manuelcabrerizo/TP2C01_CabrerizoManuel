using UnityEngine;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private DroneState drone;

    public CitizenSpawner citizenSpawner;
    public BulletSpawner playerBulletSpawner;
    public BulletSpawner alienBulletSpawner;

    private int score = 0;
    private int alienAlive = 0;
    private int citzensKill = 0;
    private int aliensKill = 0;
    
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
    }

    public void AlienHasSpawn()
    {
        alienAlive++;
        EventManager.Instance.onAlienAliveChange.Invoke(alienAlive);
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
        EventManager.Instance.onGameOver.Invoke();
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
            EventManager.Instance.onWin.Invoke();
        }
    }

    public Vector3 GetPlayerPosition()
    {
        return drone.transform.position;
    }
}
