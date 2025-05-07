using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviourSingleton<GameSceneManager>
{
    [SerializeField] private float maxTime;
    private IEnumerator loadingScene = null;

    public void ChangeSceneTo(string sceneName)
    {
        if (loadingScene != null)
        {
            StopCoroutine(loadingScene);
        }
        loadingScene = LoadingScene(sceneName);
        StartCoroutine(loadingScene);
    }

    private IEnumerator LoadingScene(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        float onTime = 0.0f;
        while(onTime < maxTime * 0.5f)
        {
            onTime += Time.deltaTime;
            EventManager.Instance.onLoadingBarChange.Invoke(onTime/maxTime);
            yield return null;
        }

        while(operation.progress < 0.89f)
        {
            yield return null;
        }

        while(onTime < maxTime)
        {
            onTime += Time.deltaTime;
            EventManager.Instance.onLoadingBarChange.Invoke(onTime/maxTime);
            yield return null;
        }

        operation.allowSceneActivation = true;
        loadingScene = null;
        gameObject.SetActive(false);
    }
}
