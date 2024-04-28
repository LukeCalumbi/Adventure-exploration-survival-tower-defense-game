using System;
using System.Collections.Generic;
using UnityEngine;

// All time related variables are in Seconds
public class Timer 
{
    private float waitTime = 1f;
    private float timeElapsed = 0f;
    private bool isPaused = true;
    private bool neverRan = true;

    public Timer(float waitTime)
    {
        this.waitTime = waitTime;
        timeElapsed = 0f;
        isPaused = true;
        neverRan = true;
    }

    public void Update(float deltaTime)
    {
        if (isPaused)
            return;

        timeElapsed = Mathf.Min(timeElapsed + deltaTime, waitTime);
        isPaused = isPaused || Finished();
    }

    public void Start()
    {
        timeElapsed = 0f;
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
        timeElapsed = waitTime;
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
        timeElapsed -= extraTime;
    }

    public bool Finished()
    {
        return timeElapsed == waitTime;
    }

    public bool IsRunning()
    {
        return !isPaused && timeElapsed < waitTime;
    }

    public bool IsPaused()
    {
        return isPaused;
    }

    public bool NeverRan()
    {
        return neverRan;
    }

    public float GetTimeElapsed()
    {
        return timeElapsed;
    }

    public float GetCompletionPercentage()
    {
        return Mathf.Max(GetTimeElapsed() / waitTime, 0f);
    }

    public float GetTimeRemaining()
    {
        return waitTime - timeElapsed;
    }

    public float GetRemainingTimePercentage()
    {
        return Mathf.Min(GetTimeRemaining() / waitTime, 1f);
    }
}