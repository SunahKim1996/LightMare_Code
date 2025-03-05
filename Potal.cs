using System;
using System.Collections;
using UnityEngine;

public class Potal : MonoBehaviour
{
    [SerializeField] private GameMap nextMap;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player != null)
        {
            PlayerHUDManager.instance.ToggleNoticeText(true);

            string mapName = Enum.GetName(typeof(GameMap), nextMap);
            StartCoroutine(GoNextMap(mapName));
        }
    }

    IEnumerator GoNextMap(string mapName)
    {
        yield return new WaitForSeconds(2f);

        Player.instnace.playerData.RecentMap = (int)nextMap;
        LoadSceneManager.LoadScene(mapName);
    }
}
