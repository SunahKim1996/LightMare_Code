using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSlice : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player != null)
        {
            player.playerData.LightCount++;

            SoundManager.instance.PlaySFX(SoundClip.LightSliceSFX, 0.1f);
            Destroy(gameObject);        
        }
    }
}
