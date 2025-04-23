using UnityEngine;

class CountDownState : IState
{
    private float timer = 0;

    private int secondCount = 0;
    private int timeToWait = 3;

    public void OnEnter()
    {
        Cursor.lockState = CursorLockMode.Locked;
        EventManager.Instance.onShowCountDownUI.Invoke();
        EventManager.Instance.onCountDownChange.Invoke(timeToWait);
        timer = 0;
        secondCount = 0;
    }

    public void OnExit()
    {
        timer = 0;
        secondCount = 0;
        EventManager.Instance.onHideCountDownUI.Invoke();
    }

    public void OnFixedUpdate(float dt)
    {
    }

    public void OnUpdate(float dt)
    {
        if(timer >= 1.0f)
        {
            secondCount++;
            EventManager.Instance.onCountDownChange.Invoke(timeToWait - secondCount);
            timer -= 1.0f;
        }
        timer += Time.deltaTime;

        if(secondCount == timeToWait)
        {
            GameManager.Instance.SetPlayingState();
        }
    }
}