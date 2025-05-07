using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviourSingleton<UIManager>
{
    [SerializeField] GameObject countDownUI;
    [SerializeField] GameObject playingUI;

    [SerializeField] GameObject winUI;
    [SerializeField] GameObject gameOverUI;

    [SerializeField] private TextMeshProUGUI aliensAlive;
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI citizensKilled;
    [SerializeField] private TextMeshProUGUI aliensKilled;
    [SerializeField] private Image lifebar;

    [SerializeField] private Button nextButton;
    [SerializeField] private Button restartButton;

    [SerializeField] private Button winMenuButton;
    [SerializeField] private Button gameOverMenuButton;

    [SerializeField] private TextMeshProUGUI countDownText;
    
    protected override void OnAwaken()
    {
        EventManager.Instance.onShowCountDownUI.AddListener(OnShowCountDownUI);
        EventManager.Instance.onHideCountDownUI.AddListener(OnHideCountDownUI);
        EventManager.Instance.onShowPlayingUI.AddListener(OnShowPlayingUI);
        EventManager.Instance.onHidePlayingUI.AddListener(OnHidePlayingUI);
        EventManager.Instance.onShowWinUI.AddListener(OnShowWinUI);
        EventManager.Instance.onHideWinUI.AddListener(OnHideWinUI);
        EventManager.Instance.onShowGameOverUI.AddListener(OnShowGameOverUI);
        EventManager.Instance.onHideGameOverUI.AddListener(OnHideGameOverUI);
        EventManager.Instance.onCountDownChange.AddListener(OnCountDownChange);

        aliensAlive.text = "Aliens Alive: 0";
        score.text = "Score: 0";
        citizensKilled.text = "Citizens killed: 0";
        aliensKilled.text = "Aliens killed: 0";
        EventManager.Instance.onAlienAliveChange.AddListener(OnAliensAliveChange);
        EventManager.Instance.onScoreChange.AddListener(OnScoreChange);
        EventManager.Instance.onCitizensKilledChange.AddListener(OnCitizensKilledChange);
        EventManager.Instance.onAliensKilledChange.AddListener(OnAliensKilledChange);
        EventManager.Instance.onTakeDamage.AddListener(OnTakeDamage);

        nextButton.onClick.AddListener(OnNextAndRestartButtonClick);
        restartButton.onClick.AddListener(OnNextAndRestartButtonClick);
        winMenuButton.onClick.AddListener(OnMenuButtonClick);
        gameOverMenuButton.onClick.AddListener(OnMenuButtonClick);
    }

    protected override void OnDestroyed()
    {
        nextButton.onClick.RemoveAllListeners();
        restartButton.onClick.RemoveAllListeners();
        winMenuButton.onClick.RemoveAllListeners();
        gameOverMenuButton.onClick.RemoveAllListeners();
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

    private void OnNextAndRestartButtonClick()
    {
        GameManager.Instance.SetCountDownState();
    }

    private void OnMenuButtonClick()
    {
        SceneManager.LoadScene("MainMenu");
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

    private void OnTakeDamage(float fillAmoun)
    {
        lifebar.fillAmount = fillAmoun;
    }

    private void OnCountDownChange(int countDown)
    {   
        countDownText.text = countDown.ToString();
    }
}
