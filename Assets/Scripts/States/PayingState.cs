using System;
using UnityEngine;

class PlayingState : IState
{
    public static event Action onShowPlayingUI;
    public static event Action onHidePlayingUI;
    public void OnEnter()
    {
        Cursor.lockState = CursorLockMode.Locked;
        onShowPlayingUI?.Invoke();
    }

    public void OnExit()
    {
        onHidePlayingUI?.Invoke();
    }

    public void OnFixedUpdate(float dt)
    {
    }

    public void OnUpdate(float dt)
    {
    }
}