using UnityEngine;

class PlayingState : IState
{
    public void OnEnter()
    {
        Cursor.lockState = CursorLockMode.Locked;
        EventManager.Instance.onShowPlayingUI.Invoke();
    }

    public void OnExit()
    {
        EventManager.Instance.onHidePlayingUI.Invoke();
    }

    public void OnFixedUpdate(float dt)
    {
    }

    public void OnUpdate(float dt)
    {
    }
}