using System;
using UnityEngine;

public class PauseState : State
{
    public static event Action onShowPauseUI;
    public static event Action onHidePuaseUI;

    public PauseState(GameManager gameManager)
        : base(gameManager) { }

    public override void OnEnter()
    {
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0.0f;
        onShowPauseUI?.Invoke();
    }

    public override void OnExit()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1.0f;
        onHidePuaseUI?.Invoke();
    }

    public override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameManager.ResumeGame();
        }
    }
}
