using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using UnityEditor;

/// <summary>
/// SceneManager 순서와 동일
/// </summary>
public enum GameMap
{
    Map01 = 1,
    Map02 = 2,
    Map03 = 3,
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject lightSlice;

    [HideInInspector] public bool isGameOver = false;
    [HideInInspector] public bool isGameClear = false;

    [SerializeField] private GameObject cameraSettingPrefab;
    [SerializeField] private Player playerPrefab;

    GameObject cameraSetting;
    Player player;

    public bool isReadyCamSet = false;

    Vector3 playerInitPos = new Vector3(-3, 1, 0);

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

        StartPlayer();
    }

    /// <summary>
    /// 이어서 시작 
    /// </summary>
    public void StartContinue()
    {
        int mapID = PlayerPrefs.GetInt("RecentMapNumber");

        if (mapID <= 0)
            mapID = 1;

        GameMap mapEnum = (GameMap)mapID;
        string mapName = Enum.GetName(typeof(GameMap), mapEnum);

        Player.instnace.transform.position = playerInitPos;
        //cameraSetting.SetActive(true);

        LoadSceneManager.LoadScene(mapName);
    }

    IEnumerator Wait()
    {
        while (player == null || !isReadyCamSet)
            yield return null;

        StartContinue();
    }

    // 카메라의 부자연스러운 이동을 없애기 위해 추가
    public void TogglePlayer(bool state)
    {
        player.GetComponent<SpriteRenderer>().enabled = state;
        player.GetComponent<Rigidbody2D>().isKinematic = !state;
    }

    void StartPlayer()
    {
        player = Instantiate(playerPrefab, playerInitPos, Quaternion.identity);
        TogglePlayer(false);

        cameraSetting = Instantiate(cameraSettingPrefab);

        StartCoroutine(Wait());
    }

    public void InitSettingPlayerPos()
    {
        if (player == null)
            return;

        player.transform.position = playerInitPos;
    }

    /// <summary>
    /// 메인에서 시작하는 경우 플레이어 & 카메라 세팅 삭제
    /// </summary>
    void EndPlayer()
    {
        Destroy(player);
        player = null;

        Destroy(cameraSetting);
        cameraSetting = null;
    }

    public void GameOver(bool isClear)
    {
        isGameOver = true;
        PlayerHUDManager.instance.ShowGameOverUI(isClear);
    }

    //씬을 로드하는 게 아닌 처음 위치에서 다시 시작 
    public void Restart()
    {
        SoundManager.instance.PlaySFX(SoundClip.ClearButtonSFX, 1f);

        isGameOver = false;
        Player.instnace.playerData.Hp = Player.instnace.playerData.MaxHp;
        Player.instnace.ChangePlayerState(PlayerState.Idle);
        Player.instnace.GetComponent<SpriteRenderer>().flipX = true;
        Player.instnace.transform.position = playerInitPos;

        PlayerHUDManager.instance.EndGameOverUI();
    }

    public void GoBackMain()
    {
        SoundManager.instance.PlaySFX(SoundClip.ClearButtonSFX, 0.4f);

        isGameOver = false;
        //PlayerHUDManager.instance.EndGameOverUI();
        //Player.instnace.playerData.RecentMap = 1;
        //Player.instnace.InitPlayer();
        EndPlayer();

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
