using System;
using System.Collections.Generic;
using UnityEngine;

public class Timer 
{
    private float waitTime = 1f;
    private float time = 0f;
    private bool isPaused = true;
    private bool neverRan = true;

    public Timer(float waitTime)
    {
        this.waitTime = waitTime;
        time = 0f;
        isPaused = true;
        neverRan = true;
    }

    public void Update(float deltaTime)
    {
        if (isPaused)
            return;

        time = Mathf.Min(time + deltaTime, waitTime);
        isPaused = isPaused || Finished();
    }

    public void Start()
    {
        time = 0f;
        isPaused = false;
        neverRan = false;
    }

    public void StartIfNotRunning()
    {
        if (IsRunning())
            return;

        Start();
    }

    public void Pause()
    {
        isPaused = true;
    }

    public void Resume()
    {
        isPaused = false;
    }

    public void ForceEnd()
    {
        time = waitTime;
        isPaused = true;
    }

    public void SetWaitTime(float waitTime)
    {
        this.waitTime = waitTime;
    }

    public void SetWaitTimeAndRestart(float waitTime)
    {
        SetWaitTime(waitTime);
        Start();
    }

    public void ExtendTime(float extraTime)
    {
        time -= extraTime;
    }

    public bool Finished()
    {
        return time == waitTime;
    }

    public bool IsRunning()
    {
        return !isPaused && time < waitTime;
    }

    public bool IsPaused()
    {
        return isPaused;
    }

    public bool NeverRan()
    {
        return neverRan;
    }
}