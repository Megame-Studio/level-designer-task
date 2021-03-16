using System;
using UnityEngine;

public class FpsLock : MonoBehaviour
{
    public int fps = 60;

    private void Awake()
    {
        Application.targetFrameRate = fps;
    }
}
