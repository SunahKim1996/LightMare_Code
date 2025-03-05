using UnityEngine;

public class Boss : MonsterManager
{
    bool isPlayerTraceTime = false;
    Player traceTarget = null;

    // Start is called before the first frame update
    void Start()
    {       
        lightSlice = GameManager.instance.lightSlice; 

        animator = GetComponent<Animator>();
        animator.SetInteger("state", 0);

        currentHp = maxHp;
        speed = 0.5f;
        hpBar.gameObject.SetActive(false);
    }

    protected override void EnterTrigger(Collider2D collision)
    {
        base.EnterTrigger(collision);

        Player player = collision.GetComponent<Player>();

        if (player != null)
        {
            isPlayerTraceTime = true;
            hpBar.gameObject.SetActive(true);
            traceTarget = target.GetComponent<Player>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnterTrigger(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ExitTrigger(collision);
    }


    protected override void Dead()
    {
        base.Dead();

        if (monsterType == MonsterType.SemiBoss)
        {
            portal.SetActive(true);
        }
        else if (monsterType == MonsterType.Boss)
        {
            removeWall.SetActive(false);

            Vector2 pos = appearWall.transform.position;
            Vector2 newPos = new Vector2(target.transform.position.x - 2, pos.y);
            appearWall.transform.position = newPos;
            appearWall.SetActive(true);

            //lightSlice.SetActive(true);
            //lightSlice.transform.position = transform.position;

            GameManager.instance.isGameClear = true;
        }
    }

    protected override void Move()
    {
        //TODO: 거리 제한 추가
        if (traceTarget == null || traceTarget.GetComponent<Player>().playerData.Hp <= 0 || !isPlayerTraceTime)
            return;

        Vector3 playerPos = traceTarget.transform.position;
        Vector3 localScale = transform.localScale;

        Vector3 dir = Vector3.zero;
        float localScaleX = localScale.x;

        if (playerPos.x < transform.position.x)
        {
            dir = Vector3.left;
            localScaleX = Mathf.Abs(localScale.x);
        }

        else if (playerPos.x > transform.position.x)
        {
            dir = Vector3.right;
            localScaleX = Mathf.Abs(localScale.x) * (-1);
        }

        transform.localScale = new Vector3(localScaleX, localScale.y, localScale.z);
        transform.Translate(dir * speed * Time.deltaTime);
    }
}
