using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadSceneManager : MonoBehaviour
{
    public static string nextScene;
    [SerializeField] private Slider loadingBar;

    private void Start()
    {
        StartCoroutine(LoadScene());
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("Loading");
    }

    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;
        float timer = 0.1f;

        while (!op.isDone)
        {
            yield return null;
            timer += Time.deltaTime;
            if (op.progress < 0.9f)
            {
                loadingBar.value = Mathf.Lerp(loadingBar.value, op.progress, timer);
                if (loadingBar.value >= op.progress)
                {
                    timer = 0.1f;
                }
            }
            else
            {
                //timer 0으로 하면, 여기서 fillAmount 가 안채워져서 최소 0.1 로 설정함    
                loadingBar.value = Mathf.Lerp(loadingBar.value, 1f, timer);

                if (loadingBar.value >= 0.99f)
                {
                    op.allowSceneActivation = true;
                    AsyncOperation op2 = SceneManager.LoadSceneAsync("UI", LoadSceneMode.Additive);

                    while (!op2.isDone)
                    {
                        yield return null;
                    }

                    yield break;
                }
            }
        }
    }
}
