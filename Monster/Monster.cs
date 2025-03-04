using UnityEngine;

public class Monster : MonsterManager
{
    float moveTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        lightSlice = GameManager.instance.lightSlice;

        animator = GetComponent<Animator>();
        animator.SetInteger("state", 0);

        currentHp = maxHp;
        speed = 1f;
        randomInt = Random.Range(0, 2);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnterTrigger(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ExitTrigger(collision);
    }

    protected override void Move()
    {
        //attackTimer = 0;
        moveTimer += Time.deltaTime;

        if (moveTimer >= 2)
        {
            moveTimer = 0;
            randomInt = randomInt == 0 ? 1 : 0;
        }

        Vector2 dir;

        if (randomInt == 0)
        {
            dir = Vector2.left;
            transform.GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            dir = Vector2.right;
            transform.GetComponent<SpriteRenderer>().flipX = true;
        }

        gameObject.transform.Translate(dir * speed * Time.deltaTime);
    }
}
