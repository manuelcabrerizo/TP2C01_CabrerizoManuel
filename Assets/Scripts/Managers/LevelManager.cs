using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] private LevelsData levelsData;

    private int currentLevel = 0;

        private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

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

        // TODO: remove this 
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha9))
        {
            currentLevel = Math.Max(0, currentLevel  - 1);
            GameManager.Instance.StartNewGame(levelsData.levels[currentLevel]);
        }
        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            currentLevel = Math.Min(levelsData.levels.Count - 1, currentLevel  + 1);
            GameManager.Instance.StartNewGame(levelsData.levels[currentLevel]);
        }
    }

    public LevelData GetCurrentLevelData()
    {
        return levelsData.levels[currentLevel];
    }
}
