using System;
using UnityEngine;

public class LevelManager : MonoBehaviourSingleton<LevelManager>
{
    public static event Action<LevelData> onStartNewGame;

    [SerializeField] private LevelsData levelsData;
    private int currentLevel = 0;

    protected override void OnAwaken()
    {
        WinState.onWin += OnWin;
        GameOverState.onGameOver += OnGameOver;
    }
    protected override void OnDestroyed()
    {
        WinState.onWin -= OnWin;
        GameOverState.onGameOver -= OnGameOver;
    }

    private void Start()
    {
        onStartNewGame?.Invoke(levelsData.levels[currentLevel]);
    }

    private void OnWin()
    {
        currentLevel = Math.Min(levelsData.levels.Count - 1, currentLevel  + 1);
        onStartNewGame?.Invoke(levelsData.levels[currentLevel]);
    }

    private void OnGameOver()
    {
        onStartNewGame?.Invoke(levelsData.levels[currentLevel]);
    }

    public LevelData GetCurrentLevelData()
    {
        return levelsData.levels[currentLevel];
    }
}
