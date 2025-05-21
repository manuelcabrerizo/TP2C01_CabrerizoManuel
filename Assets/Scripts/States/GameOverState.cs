
using System;
using UnityEngine;

class GameOverState : State
{
    public static event Action onGameOver;
    public static event Action onShowGameOverUI;
    public static event Action onHideGameOverUI;

    public GameOverState(GameManager gameManager) 
        : base(gameManager) { }
    public override void OnEnter()
    {
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.Mute();
        Cursor.lockState = CursorLockMode.None;
        onShowGameOverUI?.Invoke();
    }

    public override void OnExit()
    {
        onHideGameOverUI?.Invoke();
        onGameOver?.Invoke();
    }
}