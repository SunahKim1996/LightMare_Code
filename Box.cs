using System.Collections;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] private GameObject item;
    [SerializeField] private Sprite brokenImage;

    [SerializeField] private int maxHp;
    private int hp;

    public bool isLightBox;

    void Start()
    {
        hp = maxHp;
    }

    public void Damaged()
    {
        hp--;

        if (hp <= 0)
        {
            this.GetComponent<SpriteRenderer>().sprite = brokenImage;

            // 빛의 조각 생성
            if (isLightBox)
                Instantiate(GameManager.instance.lightSlice, this.transform.position, Quaternion.identity);

            //랜덤으로 체력 아이템 생성
            else
            {
                int randomInt = Random.Range(0, 3);
                if (randomInt == 0)
                    Instantiate(item, this.transform.position, Quaternion.identity);
            }

            GameManager.instance.RemoveFadeOut(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player != null)
        {
            player.target = transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player != null)
        {
            player.target = null;
        }
    }
}
