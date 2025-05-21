using System;
using UnityEngine;

class WinState : State
{
    public static event Action onWin;
    public static event Action onShowWinUI;
    public static event Action onHideWinUI;

    public WinState(GameManager gameManager) 
        : base(gameManager) { }


    public override void OnEnter()
    {
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.Mute();
        Cursor.lockState = CursorLockMode.None;
        onShowWinUI?.Invoke();
    }

    public override void OnExit()
    {
        onHideWinUI?.Invoke();
        onWin?.Invoke();
    }
}