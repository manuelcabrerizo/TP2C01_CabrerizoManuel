using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviourSingleton<EventManager>
{
    public UnityEvent<Entity> onEntityRelease;
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
    public UnityEvent<float> onLoadingBarChange;
    public UnityEvent<GameObject> onEnemyDamage;
    public UnityEvent<GameObject> onCitizenDamage;

    protected override void OnAwaken()
    {
        onEntityRelease = new UnityEvent<Entity>();
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
        onLoadingBarChange = new UnityEvent<float>();
        onEnemyDamage = new UnityEvent<GameObject>();
        onCitizenDamage = new UnityEvent<GameObject>();
    }

    protected override void OnDestroyed()
    {
        onEntityRelease.RemoveAllListeners();
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
        onLoadingBarChange.RemoveAllListeners();
        onEnemyDamage.RemoveAllListeners();
        onCitizenDamage.RemoveAllListeners();
    }
}
