using UnityEngine;

class CountDownState : IState
{
    private float timer = 0;
    private float timeToWait = 3;

    public void OnEnter()
    {
        Cursor.lockState = CursorLockMode.Locked;
        EventManager.Instance.onShowCountDownUI.Invoke();
        timer = timeToWait;
    }

    public void OnExit()
    {
        timer = 0;
        EventManager.Instance.onHideCountDownUI.Invoke();
    }

    public void OnFixedUpdate(float dt)
    {
    }

    public void OnUpdate(float dt)
    {
        if(timer < 0.0f)
        {
            GameManager.Instance.SetPlayingState();
        }
        timer -= Time.deltaTime;
    }
}