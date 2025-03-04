using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPlayer : MonoBehaviour
{
    [SerializeField] private List<Sprite> sprites;
    [SerializeField] private float speed;

    float timer;
    int curIndex = 1;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= speed)
        {
            timer = 0;

            if (curIndex >= sprites.Count)
                curIndex = 0;

            GetComponent<Image>().sprite = sprites[curIndex];
            curIndex++;
        }
    }
}
