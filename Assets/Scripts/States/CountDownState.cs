using System;
using UnityEngine;

class CountDownState : State
{
    public static event Action onShowCountDownUI;
    public static event Action onHideCountDownUI;
    public static event Action<int> onCountDownChange;
    public static event Action onCountDownEnd;

    private float timer = 0;
    private int secondCount = 0;
    private int timeToWait = 3;

    public CountDownState(GameManager gameManager)
        : base(gameManager) { }

    public override void OnEnter()
    {
        AudioManager.Instance.PlayMusic();
        AudioManager.Instance.Unmute();
        Cursor.lockState = CursorLockMode.Locked;
        onShowCountDownUI?.Invoke();
        onCountDownChange?.Invoke(timeToWait);
        timer = 0;
        secondCount = 0;
    }

    public override void OnExit()
    {
        timer = 0;
        secondCount = 0;
        onHideCountDownUI?.Invoke();
    }

    public override void OnUpdate()
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