using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerController : MonoBehaviour
{
    private bool timerrunning;
    private static float totaltime = 0;
    private float time;

    void Update()
    {
        if (timerrunning)
            time += Time.deltaTime;
    }

    private void SetTotalTime()
    {
        totaltime += time;
    }

    public float GetTime()
    {
        return time;
    }

    public float GetTotalTime()
    {
        return totaltime;
    }

    public void SetTimer()
    {
        timerrunning = true;
    }

    public void StopTimer()
    {
        timerrunning = false;
        SetTotalTime();
    }

    public void ResetTimer()
    {
        time = 0;
    }

    public void ResetTotalTimer()
    {
        totaltime = 0;
    }
}
