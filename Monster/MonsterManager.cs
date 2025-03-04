using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class MonsterManager : MonoBehaviour
{
    public enum MonsterType
    {
        Normal,
        SemiBoss,
        Boss,
    }

    [SerializeField] protected MonsterType monsterType;

    [Header("State")]
    [SerializeField] protected float maxHp;    //max체력
    protected float currentHp; //현재 체력

    [SerializeField] protected float defensePower;   //방어력
    [SerializeField] protected float attackPower;    //공격력

    [SerializeField] protected float attackDelay;
    float attackTimer;

    [SerializeField] protected Slider hpBar;
    protected Animator animator;
    private bool isMonsterDead = false;

    [Header("Move")]    
    protected float speed = 1f;
    protected int randomInt;

    [Header("Light Slice")]
    [SerializeField] protected bool isLightMonster;
    protected GameObject lightSlice;

    [Header("Player Target")]
    protected bool isTriggerPlayer = false;
    protected Transform target;

    [Header("Boss")]
    [SerializeField] protected GameObject portal;
    [SerializeField] protected GameObject removeWall;
    [SerializeField] protected GameObject appearWall;


    void Update()
    {
        if (GameManager.instance.isGameOver || isMonsterDead)
            return;

        if (!isTriggerPlayer)
            Move();
        else
            Attack();
    }

    protected abstract void Move();

    void Attack()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackDelay)
        {
            attackTimer = 0;

            StartCoroutine(AttackAnimation());
            target.GetComponent<Player>().Damaged(gameObject, attackPower);
        }
    }

    IEnumerator AttackAnimation()
    {
        animator.SetInteger("state", 1);
        yield return new WaitForSeconds(0.8f);
        animator.SetInteger("state", 0);

        StopCoroutine(AttackAnimation());
    }

    public void Damaged(float playerPower)
    {
        if (GameManager.instance.isGameOver || isMonsterDead)
            return;

        currentHp = currentHp - playerPower;

        if (currentHp <= 0)
            Dead();

        RefreshHpBar();
    }

    protected virtual void Dead()
    {
        isMonsterDead = true;

        if (isLightMonster)
        {
            Vector3 targetPos = new Vector3(this.transform.position.x, this.transform.position.y + 2f, 0f);
            GameObject newLight = Instantiate(lightSlice, targetPos, Quaternion.identity);
            newLight.SetActive(true);
        }

        target.GetComponent<Player>().GetExp();

        hpBar.gameObject.SetActive(false);
        animator.SetInteger("state", 2);

        GameManager.instance.RemoveFadeOut(gameObject);
    }

    protected virtual void EnterTrigger(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player != null)
        {
            isTriggerPlayer = true;
            target = collision.transform;

            player.target = transform;
        }
    }

    protected void ExitTrigger(Collider2D collision) 
    {
        Player player = collision.GetComponent<Player>();

        if (player != null)
        {
            isTriggerPlayer = false;
            target = null;

            player.target = null;
        }
    }

    void RefreshHpBar()
    {
        hpBar.maxValue = maxHp;
        hpBar.minValue = 0;
        hpBar.value = currentHp;
    }
}
