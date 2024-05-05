using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Counter
{
    [SerializeField] private int max;
    [SerializeField] private int current;

    public Counter(int max)
    {
        this.max = max;
        current = 0;
    }

    public Counter(int current, int max)
    {
        this.max = Mathf.Max(max, 0);
        this.current = Mathf.Clamp(current, 0, this.max);
    }

    public void CountUp(int amount = 1)
    {
        if (amount < 0) {
            CountDown(-amount);
            return;
        }

        current = Mathf.Min(current + amount, max);
    }

    public void CountDown(int amount = 1)
    {
        if (amount < 0) {
            CountUp(-amount);
            return;
        }

        current = Mathf.Max(current - amount, 0);
    }

    public void Reset()
    {
        current = 0;
    }

    public void Fill()
    {
        current = max;
    }

    public void Resize(int newMax)
    {
        max = Mathf.Max(newMax, 0);
        current = Mathf.Min(current, max);
    }

    public void ResizeAndReset(int newMax)
    {
        Resize(newMax);
        Reset();
    }

    public void ResizeAndCountDifference(int newMax)
    {
        int difference = newMax - max;
        Resize(newMax);
        CountUp(difference);
    }

    public bool Maxed()
    {
        return current == max;
    }

    public bool IsZero()
    {
        return current == 0;
    }

    public int GetCurrentCount()
    {
        return current;
    }

    public int GetMaxCount()
    {
        return max;
    }

    public float GetCompletionPercentage()
    {
        return (float)current / max;
    }

    public float GetRemainingPercentage()
    {
        return (float)(max - current) / max;
    }
}
