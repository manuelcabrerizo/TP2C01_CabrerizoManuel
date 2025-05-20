
using System;
using UnityEngine;

class GameOverState : IState
{
    public static event Action onGameOver;
    public static event Action onShowGameOverUI;
    public static event Action onHideGameOverUI;
    public void OnEnter()
    {
        Cursor.lockState = CursorLockMode.None;
        onShowGameOverUI?.Invoke();
    }

    public void OnExit()
    {
        onHideGameOverUI?.Invoke();
        onGameOver?.Invoke();
    }

    public void OnFixedUpdate(float dt)
    {
    }

    public void OnUpdate(float dt)
    {
    }
}