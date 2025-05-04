using System;
using UnityEngine;

public class LevelManager : MonoBehaviourSingleton<LevelManager>
{
    [SerializeField] private LevelsData levelsData;
    private int currentLevel = 0;

    private void Start()
    {
        EventManager.Instance.onWin.AddListener(OnWin);
        EventManager.Instance.onGameOver.AddListener(OnGameOver);
        GameManager.Instance.StartNewGame(levelsData.levels[currentLevel]);
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
