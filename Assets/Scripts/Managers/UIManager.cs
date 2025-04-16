using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] private TextMeshProUGUI aliensAlive;
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI citizensKilled;
    [SerializeField] private TextMeshProUGUI aliensKilled;
    [SerializeField] private Image lifebar;

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
        aliensAlive.text = "Aliens Alive: 0";
        score.text = "Score: 0";
        citizensKilled.text = "Citizens killed: 0";
        aliensKilled.text = "Aliens killed: 0";
        EventManager.Instance.onAlienAliveChange.AddListener(OnAliensAliveChange);
        EventManager.Instance.onScoreChange.AddListener(OnScoreChange);
        EventManager.Instance.onCitizensKilledChange.AddListener(OnCitizensKilledChange);
        EventManager.Instance.onAliensKilledChange.AddListener(OnAliensKilledChange);
        EventManager.Instance.onTakeDamage.AddListener(OnTakeDamage);
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
