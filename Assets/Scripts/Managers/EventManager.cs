using UnityEngine;
using UnityEngine.Events;


public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    public UnityEvent<RaycastHit, Ray> onEnemyHit;
    public UnityEvent<Citizen> onCitizenRelease;

    public UnityEvent<int> onAlienAliveChange;
    public UnityEvent<int> onScoreChange;
    public UnityEvent<int> onCitizensKilledChange;
    public UnityEvent<int> onAliensKilledChange;

    public UnityEvent<float> onTakeDamage;
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
        onEnemyHit = new UnityEvent<RaycastHit, Ray>();
        onCitizenRelease = new UnityEvent<Citizen>();
        onAlienAliveChange = new UnityEvent<int>();
        onScoreChange = new UnityEvent<int>();
        onCitizensKilledChange = new UnityEvent<int>();
        onAliensKilledChange = new UnityEvent<int>();
        onTakeDamage = new UnityEvent<float>();
    }

    private void OnDestroy()
    {
        onEnemyHit.RemoveAllListeners();
        onCitizenRelease.RemoveAllListeners();
        onAlienAliveChange.RemoveAllListeners();
        onScoreChange.RemoveAllListeners();
        onCitizensKilledChange.RemoveAllListeners();
        onAliensKilledChange.RemoveAllListeners();
        onTakeDamage.RemoveAllListeners();
    }
}
