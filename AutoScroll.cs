﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoScroll : MonoBehaviour
{

    public float ScrollSpeed = 0.5f;
    float Target_offset;

    // Update is called once per frame
    void Update()
    {
        Target_offset += Time.deltaTime * ScrollSpeed;
        this.gameObject.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(Target_offset, 0);
    }
}
