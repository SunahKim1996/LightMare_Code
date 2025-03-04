using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// animator state number와 같음 
/// </summary>
public enum PlayerState
{
    Idle, 
    Run, 
    Attack, 
    Jump, 
    Ladder,
    Dead,
    Damaged,
}

public class Player : MonoBehaviour
{
    public static Player instnace;

    [Header("State")]
    bool isDamgedTime = false;
    bool isPlayerOnGround = true;

    public PlayerState curState;

    [Header("Setting")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce = 5f;

    Rigidbody2D rigidBody;
    SpriteRenderer spriteRenderer;
    Animator animator;

    public Transform target = null;

    [Header("Ladder")]
    [SerializeField] private float ladderSpeed;
    [HideInInspector] public bool isLadder = false;
    float ladderCoolTimer = 0;

    [Header("PlayerData")]
    [HideInInspector] public PlayerData playerData;


    void Awake()
    {
        if (instnace != null) 
        {
            Destroy(gameObject);
            return;
        }

        instnace = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(InitPlayerData());

        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();
        
        //TODO: 이어하기 제공
        /*
        if(SceneManager.GetActiveScene().name == "Map02")
        {
            if (GameManager.isPlayerMeetSemiBoss == 1)
            {
                this.GetComponent<Transform>().position = new Vector3(380f, 2.5f, 0f);
                GameManager.isPlayerMeetSemiBoss = 0;
            }
        }
       */
    }

    IEnumerator InitPlayerData()
    {
        while (PlayerHUDManager.instance == null)
        {
            yield return null;
        }

        playerData = new PlayerData();

        playerData.Hp = playerData.MaxHp = 50;

        playerData.DefensePower = 0;
        playerData.SpecialAttackPower = 25;
        playerData.AttackPower = 5;
        playerData.SpecialAttackGauge = 0;

        playerData.Exp = 0;
        playerData.LightCount = 0;

        playerData.HpPoint = 0;
        playerData.DefensePoint = 0;
        playerData.ChargeSpeedPoint = 0;
        playerData.AttackPoint = 0;
    }

    void Update()
    {
        if (ladderCoolTimer > 0)
        {
            ladderCoolTimer -= Time.deltaTime;

            if (ladderCoolTimer <= 0)
                ladderCoolTimer = 0;
        }

        if (PlayerHUDManager.instance != null && !GameManager.instance.isGameOver && !GameManager.instance.isGameClear)
           CharacterMoving();
    }

    private void ChangePlayerState(PlayerState changeState)
    {
        animator.SetInteger("state", (int)changeState);
        curState = changeState;
    }    

    private void OnCollisionEnter2D(Collision2D other)
    {
        rigidBody.velocity = Vector2.zero;
    }

    public void Jump()
    {
        if (rigidBody.velocity.y == 0)
        {
            if (isLadder == false)
            {
                ChangePlayerState(PlayerState.Jump);
                rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }
    }

    void CharacterMoving()
    {
        Vector2 dir = PlayerHUDManager.instance.JoystickDir();

        if (curState == PlayerState.Attack)
        {
            //Debug.Log("3 -- AttackTime");
            //animator.SetInteger("state", 2);

            //return;
        }
        else if (isPlayerOnGround)
        {
            //Debug.Log("5");
            ChangePlayerState(PlayerState.Idle);
        }

        if (isLadder)
        {
            //Debug.Log("6 - Ladder");
            ChangePlayerState(PlayerState.Ladder);

            Vector3 targetDir = dir.y > 0 ? Vector3.up : dir.y < 0 ? Vector3.down : Vector3.zero;
            transform.Translate(targetDir * ladderSpeed * Time.deltaTime);
                        
            PlayerHUDManager.instance.ToggleVirtualPad(false);
            //return;
        }
        else
        {
            PlayerHUDManager.instance.ToggleVirtualPad(true);
        }
    
        if (dir.x < 0 || dir.x > 0)
        {
            //Debug.Log($"8 - Move / curState : {curState}");

            if (curState != PlayerState.Attack && curState != PlayerState.Jump && curState != PlayerState.Ladder && curState != PlayerState.Dead)
                ChangePlayerState(PlayerState.Run);


            float speed = moveSpeed;
            if (isLadder)
                speed = 0.8f;

            if (dir.x < 0)
            {
                transform.Translate(Vector3.left * speed * Time.deltaTime);
                spriteRenderer.flipX = false;
            }
            else
            {
                transform.Translate(Vector3.right * speed * Time.deltaTime);
                spriteRenderer.flipX = true;
            }
        }
        else if (dir.x == 0)
        {
            //Debug.Log("9 - MoveStop");
            if (curState != PlayerState.Attack && curState != PlayerState.Jump && curState != PlayerState.Ladder && curState != PlayerState.Dead)
                ChangePlayerState(PlayerState.Idle);
        }
    }

    // 트리거  -----------------------------------------------------------------
    //TODO
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isPlayerOnGround = false;
            ChangePlayerState(PlayerState.Jump);
        }
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isPlayerOnGround = true;
        }
    }

    // 공격  -----------------------------------------------------------------
    public void Attack()
    {
        StartCoroutine(PlayAttackAni());
        SoundManager.instance.PlaySFX(SoundClip.AttackSFX, 0.3f);

        if (target == null)
            return;

        MonsterAttack();
        BoxAttack();
    }

    void MonsterAttack()
    {
        MonsterManager monster = target.GetComponent<MonsterManager>();
        if (monster != null)
        {
            monster.Damaged(playerData.AttackPower);
            playerData.SpecialAttackGauge++;
        }
    }

    void BoxAttack()
    {
        Box box = target.GetComponent<Box>();
        if (box != null)
            box.Damaged();
    }

    public void SpecialAttack()
    {
        if (playerData.SpecialAttackGauge < 10)
            return;

        SoundManager.instance.PlaySFX(SoundClip.SpecialAttackSFX, 0.3f);
        playerData.SpecialAttackGauge = 0;

        MonsterAttack();
    }
    IEnumerator PlayAttackAni()
    {
        ChangePlayerState(PlayerState.Attack);
        
        float animationTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime; //GetCurrentAnimatorClipInfo(0).length;
        yield return new WaitForSeconds(animationTime);

        ChangePlayerState(PlayerState.Idle);
    }
    

    public void Damaged(GameObject target, float monsterPower)
    {
        ChangePlayerState(PlayerState.Damaged);

        //피격 효과
        Vector2 attackedVelocity = (target.transform.position.x > transform.position.x) ? new Vector2(-5f, 1f) : new Vector2(5f, 1f);
        rigidBody.AddForce(attackedVelocity, ForceMode2D.Impulse);

        //Hp 상실
        if (monsterPower - playerData.DefensePower > 0)
            playerData.Hp = playerData.Hp - (int)(monsterPower - playerData.DefensePower);

        //죽음
        if (playerData.Hp <= 0)
        {
            Dead();
            ChangePlayerState(PlayerState.Dead);

            return;
        }

        isDamgedTime = true;
        StartCoroutine(DamegedTime());
    }

    IEnumerator DamegedTime()
    {       
        int countTime = 0;

        while (countTime < 10)
        {
            this.GetComponent<SpriteRenderer>().color = (countTime % 2 == 0) ? new Color32(255, 255, 255, 90) : new Color32(255, 255, 255, 180);
            yield return new WaitForSeconds(0.1f);
            countTime++;
        }

        this.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
        isDamgedTime = false;
        ChangePlayerState(PlayerState.Idle);
    }

    //  -----------------------------------------------------------------
    public void GetExp()
    {
        playerData.Exp++;
    }

    public void ToggleLadderState(bool state)
    {
        if (state && ladderCoolTimer > 0)
            return;

        isLadder = state;
        rigidBody.gravityScale = state ? 0 : 1;

        PlayerState playerState = state ? PlayerState.Ladder : PlayerState.Idle;
        ChangePlayerState(playerState);

        if (!state)
            ladderCoolTimer = 0.5f;
    }

    // 스탯 강화 -----------------------------------------------------------------
    void AddPoint(Action addPointData)
    {
        if (playerData.Exp <= 0)
            return;

        SoundManager.instance.PlaySFX(SoundClip.StatUIButtonSFX, 0.3f);
        playerData.Exp--;

        addPointData.Invoke();
    }

    public int AddHPPoint()
    {
        AddPoint(() => playerData.HpPoint++ );
        return playerData.HpPoint;
    }

    public int AddDefensePoint()
    {
        AddPoint(() => playerData.DefensePoint++);
        return playerData.DefensePoint;
    }

    public int AttackPowerAdd()
    {
        AddPoint(() => playerData.AttackPoint++);
        return playerData.AttackPoint;
    }

    public int ChargeSpeedAdd()
    {
        AddPoint(() => playerData.ChargeSpeedPoint++);
        return playerData.ChargeSpeedPoint;
    }

    // -----------------------------------------------------------------
    public void Dead()
    {
        GameManager.instance.GameOver(false);
    }
}