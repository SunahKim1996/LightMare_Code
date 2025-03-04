using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    [SerializeField] private Collider2D platformCollider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player != null)
        {
            player.ToggleLadderState(true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player != null)
        {
            Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), platformCollider, true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player != null)
        {
            player.ToggleLadderState(false);
            Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), platformCollider, false);
        }        
    }
}
