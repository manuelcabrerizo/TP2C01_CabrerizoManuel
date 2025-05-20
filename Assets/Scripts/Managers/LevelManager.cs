using System;
using UnityEngine;

public class LevelManager : MonoBehaviourSingleton<LevelManager>
{
    [SerializeField] private LevelsData levelsData;
    private int currentLevel = 0;

    private void Start()
    {
        WinState.onWin += OnWin;
        GameOverState.onGameOver += OnGameOver;
        GameManager.Instance.StartNewGame(levelsData.levels[currentLevel]);
    }

    protected override void OnDestroyed()
    {
        WinState.onWin -= OnWin;
        GameOverState.onGameOver -= OnGameOver;
    }

    private void OnWin()
    {
        currentLevel = Math.Min(levelsData.levels.Count - 1, currentLevel  + 1);
        GameManager.Instance.StartNewGame(levelsData.levels[currentLevel]);
    }

    private void OnGameOver()
    {
        GameManager.Instance.StartNewGame(levelsData.levels[currentLevel]);
    }

    public LevelData GetCurrentLevelData()
    {
        return levelsData.levels[currentLevel];
    }
}
