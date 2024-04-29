using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronManager
{
    static Counter ironCounter = new Counter(0, 999);

    public static void AddIron(int count)
    {
        ironCounter.CountUp(count);
    }

    public static bool ConsumeIron(int count)
    {
        if (HasAtLeast(count)) {
            ironCounter.CountDown(count);
            return true;
        }

        return false;
    }

    public static bool HasAtLeast(int count)
    {
        return ironCounter.GetCurrentCount() >= count;
    }

    public static int GetIronCount()
    {
        return ironCounter.GetCurrentCount();
    }
}
