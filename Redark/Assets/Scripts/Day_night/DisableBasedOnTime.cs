using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableBasedOnTime : MonoBehaviour
{
    public List<MonoBehaviour> components;

    public bool disableOnDay = false;
    public bool disableOnNight = true;

    void FixedUpdate()
    {
        CheckAndUpdate();
    }

    void CheckAndUpdate()
    {
        if (DayNightCicle.IsDay())
        {
            if (disableOnDay) DisableAll();
            else EnableAll();
        }

        if (DayNightCicle.IsNight())
        {
            if (disableOnNight) DisableAll();
            else EnableAll();
        }
    }

    void DisableAll()
    {
        components.ForEach((MonoBehaviour component) => component.enabled = false);
    }

    void EnableAll()
    {
        components.ForEach((MonoBehaviour component) => component.enabled = true);
    }
}
