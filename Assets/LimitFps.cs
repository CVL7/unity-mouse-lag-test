using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitFps : MonoBehaviour
{

    public int fps = 60;
    private int oldFps;
    void Start()
    {
        Application.targetFrameRate = fps;
        oldFps = fps;
    }

    // Update is called once per frame
    void Update()
    {
        if (oldFps != fps)
        {
            Application.targetFrameRate = fps;
            oldFps = fps;
        }
    }
}
