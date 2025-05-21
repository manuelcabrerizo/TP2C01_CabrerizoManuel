using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private GameObject loadingBar;
    [SerializeField] private Image lodingBarImage;

    private void Awake()
    {
        playButton.onClick.AddListener(OnPlayButtonClick);
        exitButton.onClick.AddListener(OnExitButtonClick);
        GameSceneManager.onLoadingBarChange += OnLoadingBarChange;
    }

    private void Start()
    {
        AudioManager.Instance.Mute();
    }

    private void OnDestroy()
    {
        GameSceneManager.onLoadingBarChange -= OnLoadingBarChange;
        playButton.onClick.RemoveAllListeners();
        exitButton.onClick.RemoveAllListeners();        
    }


    private void OnPlayButtonClick()
    {
        loadingBar.SetActive(true);
        GameSceneManager.Instance.ChangeSceneTo("Gameplay");
    }

    private void OnExitButtonClick()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void OnLoadingBarChange(float value)
    {
        lodingBarImage.fillAmount = value;
    }
}
