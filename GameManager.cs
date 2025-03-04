using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject lightSlice;

    [HideInInspector] public bool isGameOver = false;
    [HideInInspector] public bool isGameClear = false;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    public void GameStart()
    {
        Button startButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        startButton.interactable = false;

        SoundManager.instance.PlaySFX(SoundClip.StartButtonSFX, 0.5f);

        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.5f);
        LoadSceneManager.LoadScene("Map01");
    }

    void InitGameState()
    {
        isGameOver = false;
    }

    public void GameOver(bool isClear)
    {
        isGameOver = true;
        PlayerHUDManager.instance.ShowGameOverUI(isClear);
    }

    public void Restart()
    {
        SoundManager.instance.PlaySFX(SoundClip.ClearButtonSFX, 1f);
        InitGameState();

        //TODO: 이어하기 처리 
        LoadSceneManager.LoadScene("Map01");
    }

    public void GoBackFirstScene()
    {
        SoundManager.instance.PlaySFX(SoundClip.ClearButtonSFX, 0.4f);
        InitGameState();

        PlayerHUDManager.instance.EndGameOverUI();

        //TODO: GameManager 랑 Player 삭제 처리? 
        LoadSceneManager.LoadScene("Main");
    }

    public void RemoveFadeOut(GameObject obj)
    {
        StartCoroutine(FadeOut(obj));
    }


    IEnumerator FadeOut(GameObject obj)
    {
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        Color tempColor = sr.color;

        while (tempColor.a > 0f)
        {
            tempColor.a -= (Time.deltaTime / 1f);
            sr.color = tempColor;
            yield return null;
        }

        Destroy(obj);
        yield break;
    }
}
