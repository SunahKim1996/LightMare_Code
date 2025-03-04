using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClearTrigger : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = GetComponent<Player>();

        if (player != null)
        {
            GameManager.instance.GameOver(true);
        }
    }
}
