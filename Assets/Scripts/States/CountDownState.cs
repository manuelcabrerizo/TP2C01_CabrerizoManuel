using System;
using UnityEngine;

class CountDownState : IState
{
    public static event Action onShowCountDownUI;
    public static event Action onHideCountDownUI;
    public static event Action<int> onCountDownChange;
    public static event Action onCountDownEnd;

    private float timer = 0;
    private int secondCount = 0;
    private int timeToWait = 3;

    public void OnEnter()
    {
        Cursor.lockState = CursorLockMode.Locked;
        onShowCountDownUI?.Invoke();
        onCountDownChange?.Invoke(timeToWait);
        timer = 0;
        secondCount = 0;
    }

    public void OnExit()
    {
        timer = 0;
        secondCount = 0;
        onHideCountDownUI?.Invoke();
    }

    public void OnFixedUpdate(float dt)
    {
    }

    public void OnUpdate(float dt)
    {
        if(timer >= 1.0f)
        {
            secondCount++;
            onCountDownChange?.Invoke(timeToWait - secondCount);
            timer -= 1.0f;
        }
        timer += Time.deltaTime;

        if(secondCount == timeToWait)
        {
            onCountDownEnd?.Invoke();
        }
    }
}