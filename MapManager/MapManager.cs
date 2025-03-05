using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    protected void StartCoroutine_EnablePlayer()
    {
        GameManager.instance.TogglePlayer(true);
    }

    /*
    IEnumerator EnablePlayer()
    {
        if (Player.instnace == null)
            yield return null;

        GameManager.instance.TogglePlayer(true);
        Player.instnace.GetComponent<SpriteRenderer>().enabled = true;
        Player.instnace.GetComponent<Rigidbody2D>().isKinematic = false;
    }
    */
}
