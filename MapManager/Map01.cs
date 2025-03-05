using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map01 : MapManager
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine_EnablePlayer();
    }
}
