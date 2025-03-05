using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerHUDManager : MonoBehaviour
{
    public static PlayerHUDManager instance;

    [SerializeField] private Slider hpBar;
    [SerializeField] private Text expHUD;
    [SerializeField] private Image[] lights;
    [SerializeField] private Image specialAttackGauge;

    [Header("TutorialUI")]
    [SerializeField] Image boxUI;
    [SerializeField] Image enemyUI;

    [Header("StatUI")]
    [SerializeField] private GameObject statUI;
    [SerializeField] private Text remainExpText;

    [Header("OtherUI")]
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject gameClearUI;
    [SerializeField] private GameObject tutorialUI;
    [SerializeField] private GameObject noticeText;
    [SerializeField] private GameObject playerHUD;

    [Header("VirtualPad")]
    [SerializeField] private FloatingJoystick joystick;
    [SerializeField] private GameObject jumpButton;
    [SerializeField] private GameObject attackButton;
    [SerializeField] private GameObject specailAttackButton;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        //DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// 체력바 설정
    /// </summary>
    public void RefreshHPUI(int curValue, int maxValue)
    {
        hpBar.maxValue = maxValue;
        hpBar.value = curValue;
    }

    /// <summary>
    /// Exp 숫자 바꾸기
    /// </summary>
    public void RefreshExpUI(int value)
    {
        expHUD.text = $"{value}";
        remainExpText.text = $"{value}";
    }

    /// <summary>
    /// 빛의 조각 개수 갱신
    /// </summary>
    public void RefreshLightCountUI(int value)
    {
        for (int i = 0; i < lights.Length; i++) 
        {
            Color color = lights[i].color;
            color.a = (i < value) ? 1f : 0.25f;  
            lights[i].color = color;
        }
    }

    /// <summary>
    /// 특수 공격 게이지 업 
    /// </summary>
    public void AddSpecialAttackGauge(float speed, int value)
    {
        specialAttackGauge.fillAmount = speed * value;
    }

    public void ToggleStatUI(bool state)
    {
        statUI.SetActive(state);
        Time.timeScale = state ? 0 : 1;
    }

    void RefreshPointText(int value)
    {
        Button curButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        Text targetText = curButton.GetComponentInChildren<Text>();

        targetText.text = $"{value} / 20";
    }

    public void RefreshHPPoint()
    {
        int value = Player.instnace.AddHPPoint();
        RefreshPointText(value);
    }

    public void RefreshDefensePoint()
    {
        int value = Player.instnace.AddDefensePoint();
        RefreshPointText(value);
    }

    public void RefreshAttackPoint()
    {
        int value = Player.instnace.AttackPowerAdd();
        RefreshPointText(value);
    }

    public void RefreshChargeSpeedPoint()
    {
        int value = Player.instnace.ChargeSpeedAdd();
        RefreshPointText(value);
    }

    public void ShowGameOverUI(bool isClear)
    {
        if (isClear)
            gameClearUI.gameObject.SetActive(true);
        else
            gameOverUI.gameObject.SetActive(true);

        playerHUD.SetActive(false);
    }

    public void EndGameOverUI()
    {
        gameClearUI.gameObject.SetActive(false);
        gameOverUI.gameObject.SetActive(false);

        playerHUD.SetActive(true);
    }

    public void OnRestart()
    {
        GameManager.instance.Restart();
    }

    public void OnGoBackMain()
    {
        GameManager.instance.GoBackMain();
    }

    public void ToggleNoticeText(bool state)
    {
        noticeText.SetActive(state);
    }

    // 튜토리얼 ==============================================================================

    public void ToggleTutorialUI(bool state)
    {
        tutorialUI.SetActive(state);
    }

    public void SetTutorialImage(Camera cam, Vector3 boxPos, Vector3 enemyPos)
    {
        Vector3 boxUIPos = cam.WorldToScreenPoint(boxPos);
        boxUI.transform.position = boxUIPos;

        Vector3 enemyUIPos = cam.WorldToScreenPoint(enemyPos);
        enemyUI.transform.position = enemyUIPos;
    }

    // 조작 ==================================================================================
    public void ToggleVirtualPad(bool state)
    {
        jumpButton.SetActive(state);
        attackButton.SetActive(state);
        specailAttackButton.SetActive(state);
    }
    
    public Vector2 JoystickDir()
    {
        return joystick.Direction;
    }

    public void PlayerJump()
    {
        Player.instnace.Jump();
    }

    public void PlayerAttack()
    {
        Player.instnace.Attack();
    }

    public void PlayerSpecialAttack()
    {
        Player.instnace.SpecialAttack();
    }
}
