using System;
using UnityEngine;

class WinState : IState
{
    public static event Action onWin;
    public static event Action onShowWinUI;
    public static event Action onHideWinUI;
    public void OnEnter()
    {
        Cursor.lockState = CursorLockMode.None;
        onShowWinUI?.Invoke();
    }

    public void OnExit()
    {
        onHideWinUI?.Invoke();
        onWin?.Invoke();
    }

    public void OnFixedUpdate(float dt)
    {
    }

    public void OnUpdate(float dt)
    {
    }
}