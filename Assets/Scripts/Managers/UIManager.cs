using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviourSingleton<UIManager>
{
    public static event Action onNextOrResetButtonClick;
    public static event Action onMenuButtonClick;
    public static event Action onResumeButtonClick;

    [SerializeField] GameObject countDownUI;
    [SerializeField] GameObject playingUI;
    [SerializeField] GameObject winUI;
    [SerializeField] GameObject gameOverUI;
    [SerializeField] GameObject pauseUI;

    [SerializeField] private TextMeshProUGUI aliensAlive;
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI citizensKilled;
    [SerializeField] private TextMeshProUGUI aliensKilled;
    [SerializeField] private Image lifebar;

    [SerializeField] private Button nextButton;
    [SerializeField] private Button restartButton;

    [SerializeField] private Button pauseResumeButton;
    [SerializeField] private Button pauseResetButton;
    [SerializeField] private Button pauseMenuButton;

    [SerializeField] private Button winMenuButton;
    [SerializeField] private Button gameOverMenuButton;

    [SerializeField] private TextMeshProUGUI countDownText;

    [SerializeField] private GameObject loadingBar;
    [SerializeField] private Image lodingBarImage;

    protected override void OnAwaken()
    {
        CountDownState.onShowCountDownUI += OnShowCountDownUI;
        CountDownState.onHideCountDownUI += OnHideCountDownUI;
        CountDownState.onCountDownChange += OnCountDownChange;
        PlayingState.onShowPlayingUI += OnShowPlayingUI;
        PlayingState.onHidePlayingUI += OnHidePlayingUI;
        WinState.onShowWinUI += OnShowWinUI;
        WinState.onHideWinUI += OnHideWinUI;
        GameOverState.onShowGameOverUI += OnShowGameOverUI;
        GameOverState.onHideGameOverUI += OnHideGameOverUI;
        PauseState.onShowPauseUI += OnShowPauseUI;
        PauseState.onHidePuaseUI += OnHidePauseUI;
        GameSceneManager.onLoadingBarChange += OnLoadingBarChange;

        aliensAlive.text = "Aliens Alive: 0";
        score.text = "Score: 0";
        citizensKilled.text = "Citizens killed: 0";
        aliensKilled.text = "Aliens killed: 0";
        GameManager.onAlienAliveChange += OnAliensAliveChange;
        GameManager.onScoreChange += OnScoreChange;
        GameManager.onCitizensKilledChange += OnCitizensKilledChange;
        GameManager.onAliensKilledChange += OnAliensKilledChange;
        Drone.onLifeChange += OnLifeChange;

        nextButton.onClick.AddListener(OnNextAndRestartButtonClick);
        restartButton.onClick.AddListener(OnNextAndRestartButtonClick);
        winMenuButton.onClick.AddListener(OnMenuButtonClick);
        gameOverMenuButton.onClick.AddListener(OnMenuButtonClick);

        pauseResumeButton.onClick.AddListener(OnPauseResumeButtonClick);
        pauseResetButton.onClick.AddListener(OnNextAndRestartButtonClick);
        pauseMenuButton.onClick.AddListener(OnMenuButtonClick);

    }

    protected override void OnDestroyed()
    {
        CountDownState.onShowCountDownUI -= OnShowCountDownUI;
        CountDownState.onHideCountDownUI -= OnHideCountDownUI;
        CountDownState.onCountDownChange -= OnCountDownChange;
        PlayingState.onShowPlayingUI -= OnShowPlayingUI;
        PlayingState.onHidePlayingUI -= OnHidePlayingUI;
        WinState.onShowWinUI -= OnShowWinUI;
        WinState.onHideWinUI -= OnHideWinUI;
        GameOverState.onShowGameOverUI -= OnShowGameOverUI;
        GameOverState.onHideGameOverUI -= OnHideGameOverUI;
        PauseState.onShowPauseUI -= OnShowPauseUI;
        PauseState.onHidePuaseUI -= OnHidePauseUI;
        GameSceneManager.onLoadingBarChange -= OnLoadingBarChange;

        GameManager.onAlienAliveChange -= OnAliensAliveChange;
        GameManager.onScoreChange -= OnScoreChange;
        GameManager.onCitizensKilledChange -= OnCitizensKilledChange;
        GameManager.onAliensKilledChange -= OnAliensKilledChange;
        Drone.onLifeChange -= OnLifeChange;

        nextButton.onClick.RemoveAllListeners();
        restartButton.onClick.RemoveAllListeners();
        winMenuButton.onClick.RemoveAllListeners();
        gameOverMenuButton.onClick.RemoveAllListeners();

        pauseResumeButton.onClick.RemoveAllListeners();
        pauseResetButton.onClick.RemoveAllListeners();
        pauseMenuButton.onClick.RemoveAllListeners();
    }
    private void OnShowCountDownUI()
    {
        countDownUI.SetActive(true);
    }

    private void OnHideCountDownUI()
    {
        countDownUI.SetActive(false);
    }

    private void OnShowPlayingUI()
    {
        playingUI.SetActive(true);
    }

    private void OnHidePlayingUI()
    {
        playingUI.SetActive(false);
    }

    private void OnShowWinUI()
    {
        winUI.SetActive(true);
    }

    private void OnHideWinUI()
    {
        winUI.SetActive(false);
    }

    private void OnShowGameOverUI()
    {
        gameOverUI.SetActive(true);
    }

    private void OnHideGameOverUI()
    {
        gameOverUI.SetActive(false);
    }

    private void OnShowPauseUI()
    {
        pauseUI.SetActive(true);
    }

    private void OnHidePauseUI()
    {
        pauseUI.SetActive(false);
    }

    private void OnNextAndRestartButtonClick()
    {
        onNextOrResetButtonClick?.Invoke();
    }

    private void OnMenuButtonClick()
    {
        loadingBar.SetActive(true);
        onMenuButtonClick?.Invoke();
    }

    private void OnPauseResumeButtonClick()
    {
        onResumeButtonClick?.Invoke();
    }

    private void OnAliensAliveChange(int value)
    {
        aliensAlive.text = "Aliens Alive: " + value;
    }

    private void OnScoreChange(int value)
    {
        score.text = "Score: " + value;
    }

    private void OnCitizensKilledChange(int value)
    {
        citizensKilled.text = "Citizens killed: " + value;
    }

    private void OnAliensKilledChange(int value)
    {
        aliensKilled.text = "Aliens killed: " + value;
    }

    private void OnLifeChange(float fillAmoun)
    {
        lifebar.fillAmount = fillAmoun;
    }

    private void OnCountDownChange(int countDown)
    {   
        countDownText.text = countDown.ToString();
    }

    private void OnLoadingBarChange(float value)
    {
        lodingBarImage.fillAmount = value;
    }
}
