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
    public UnityEvent onWin;
    public UnityEvent onGameOver;
    public UnityEvent onShowCountDownUI;
    public UnityEvent onHideCountDownUI;
    public UnityEvent onShowPlayingUI;
    public UnityEvent onHidePlayingUI;
    public UnityEvent onShowWinUI;
    public UnityEvent onHideWinUI;
    public UnityEvent onShowGameOverUI;
    public UnityEvent onHideGameOverUI;

    public UnityEvent<int> onCountDownChange;

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

    public void Init()
    {
        onEnemyHit = new UnityEvent<RaycastHit, Ray>();
        onCitizenRelease = new UnityEvent<Citizen>();
        onAlienAliveChange = new UnityEvent<int>();
        onScoreChange = new UnityEvent<int>();
        onCitizensKilledChange = new UnityEvent<int>();
        onAliensKilledChange = new UnityEvent<int>();
        onTakeDamage = new UnityEvent<float>();
        onWin = new UnityEvent();
        onGameOver = new UnityEvent();
        onShowCountDownUI = new UnityEvent();
        onHideCountDownUI = new UnityEvent();
        onShowPlayingUI = new UnityEvent();
        onHidePlayingUI = new UnityEvent();
        onShowWinUI = new UnityEvent();
        onHideWinUI = new UnityEvent();
        onShowGameOverUI = new UnityEvent();
        onHideGameOverUI = new UnityEvent();
        onCountDownChange = new UnityEvent<int>();
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
        onWin.RemoveAllListeners();
        onGameOver.RemoveAllListeners();
        onShowCountDownUI.RemoveAllListeners();
        onHideCountDownUI.RemoveAllListeners();
        onShowPlayingUI.RemoveAllListeners();
        onHidePlayingUI.RemoveAllListeners();
        onShowWinUI.RemoveAllListeners();
        onHideWinUI.RemoveAllListeners();
        onShowGameOverUI.RemoveAllListeners();
        onHideGameOverUI.RemoveAllListeners();
        onCountDownChange.RemoveAllListeners();
    }
}
