using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBGMTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();

        if (player != null)
        {
            SoundManager.instance.PlayBGM(SoundClip.SemiBossBGM, 0.3f);
        }
    }
}
