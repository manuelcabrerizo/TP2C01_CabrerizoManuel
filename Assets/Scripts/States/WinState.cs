using UnityEngine;

class WinState : IState
{
    public void OnEnter()
    {
        Cursor.lockState = CursorLockMode.None;
        EventManager.Instance.onShowWinUI.Invoke();
    }

    public void OnExit()
    {
        EventManager.Instance.onHideWinUI.Invoke();
        EventManager.Instance.onWin.Invoke();
    }

    public void OnFixedUpdate(float dt)
    {
    }

    public void OnUpdate(float dt)
    {
    }
}