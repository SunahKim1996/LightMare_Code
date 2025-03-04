using System.Collections;
using UnityEngine;

public class Potal : MonoBehaviour
{
    [SerializeField] private string nextMapName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player != null)
        {
            StartCoroutine(GoNextMap(nextMapName));
            //TODO: 플레이어 움직임 멈추게
        }
    }

    IEnumerator GoNextMap(string nextMap)
    {
        yield return new WaitForSeconds(2f);
        LoadSceneManager.LoadScene(nextMap);
    }
}
