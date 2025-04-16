using JetBrains.Annotations;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private GameObject player;
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
    }

    public void AlienHasSpawn()
    {
        alienAlive++;
        EventManager.Instance.onAlienAliveChange.Invoke(alienAlive);
    }

    public void AddToScore(int value)
    {
        score += value;
        EventManager.Instance.onScoreChange.Invoke(score);
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
    }

    public Vector3 GetPlayerPosition()
    {
        return player.transform.position;
    }
}
