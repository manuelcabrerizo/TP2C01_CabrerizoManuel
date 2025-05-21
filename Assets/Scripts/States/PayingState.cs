using System;
using UnityEngine;

class PlayingState : State
{
    public static event Action onShowPlayingUI;
    public static event Action onHidePlayingUI;

    public PlayingState(GameManager gameManager) 
        : base(gameManager) { }

    public override void OnEnter()
    {
        Cursor.lockState = CursorLockMode.Locked;
        onShowPlayingUI?.Invoke();
    }

    public override void OnExit()
    {
        onHidePlayingUI?.Invoke();
    }

    public override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameManager.PauseGame();
        }
    }
}