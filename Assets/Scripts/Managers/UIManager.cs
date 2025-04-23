using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

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

    public void Init()
    {
        EventManager.Instance.onShowCountDownUI.AddListener(OnShowCountDownUI);
        EventManager.Instance.onHideCountDownUI.AddListener(OnHideCountDownUI);
        EventManager.Instance.onShowPlayingUI.AddListener(OnShowPlayingUI);
        EventManager.Instance.onHidePlayingUI.AddListener(OnHidePlayingUI);
        EventManager.Instance.onShowWinUI.AddListener(OnShowWinUI);
        EventManager.Instance.onHideWinUI.AddListener(OnHideWinUI);
        EventManager.Instance.onShowGameOverUI.AddListener(OnShowGameOverUI);
        EventManager.Instance.onHideGameOverUI.AddListener(OnHideGameOverUI);

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
    }

    void Oestroy()
    {
        nextButton.onClick.RemoveAllListeners();
        restartButton.onClick.RemoveAllListeners();
    }
    void OnShowCountDownUI()
    {
        countDownUI.SetActive(true);
    }

    void OnHideCountDownUI()
    {
        countDownUI.SetActive(false);
    }

    void OnShowPlayingUI()
    {
        playingUI.SetActive(true);
    }

    void OnHidePlayingUI()
    {
        playingUI.SetActive(false);
    }

    void OnShowWinUI()
    {
        winUI.SetActive(true);
    }

    void OnHideWinUI()
    {
        winUI.SetActive(false);
    }

    void OnShowGameOverUI()
    {
        gameOverUI.SetActive(true);
    }

    void OnHideGameOverUI()
    {
        gameOverUI.SetActive(false);
    }

    void OnNextAndRestartButtonClick()
    {
        GameManager.Instance.SetCountDownState();
    }

    void OnAliensAliveChange(int value)
    {
        aliensAlive.text = "Aliens Alive: " + value;
    }

    void OnScoreChange(int value)
    {
        score.text = "Score: " + value;
    }

    void OnCitizensKilledChange(int value)
    {
        citizensKilled.text = "Citizens killed: " + value;
    }

    void OnAliensKilledChange(int value)
    {
        aliensKilled.text = "Aliens killed: " + value;
    }

    void OnTakeDamage(float fillAmoun)
    {
        lifebar.fillAmount = fillAmoun;
    }
}
