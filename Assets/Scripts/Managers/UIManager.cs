using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI aliensAlive;
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI citizensKilled;
    [SerializeField] private TextMeshProUGUI aliensKilled;

    private void Awake()
    {
        aliensAlive.text = "Aliens Alive: 0";
        score.text = "Score: 0";
        citizensKilled.text = "Citizens killed: 0";
        aliensKilled.text = "Aliens killed: 0";
    }

    private void Start()
    {
        EventManager.Instance.onAlienAliveChange.AddListener(OnAliensAliveChange);
        EventManager.Instance.onScoreChange.AddListener(OnScoreChange);
        EventManager.Instance.onCitizensKilledChange.AddListener(OnCitizensKilledChange);
        EventManager.Instance.onAliensKilledChange.AddListener(OnAliensKilledChange);
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
}
