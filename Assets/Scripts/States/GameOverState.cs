
using UnityEngine;

class GameOverState : IState
{
    public void OnEnter()
    {
        Cursor.lockState = CursorLockMode.None;
        EventManager.Instance.onShowGameOverUI.Invoke();
    }

    public void OnExit()
    {
        EventManager.Instance.onHideGameOverUI.Invoke();
        EventManager.Instance.onGameOver.Invoke();
    }

    public void OnFixedUpdate(float dt)
    {
    }

    public void OnUpdate(float dt)
    {
    }
}