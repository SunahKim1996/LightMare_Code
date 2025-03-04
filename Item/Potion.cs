using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : Item
{
    protected override void Use()
    {    
        base.Use();
        Player.instnace.playerData.Hp = Player.instnace.playerData.Hp + 10;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player != null) 
        {
            Use();
        }
    }
}
