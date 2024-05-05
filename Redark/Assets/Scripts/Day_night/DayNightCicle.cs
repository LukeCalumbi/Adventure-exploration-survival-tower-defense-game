using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;

public class DayNightCicle : MonoBehaviour
{
    public float dayLengthInSeconds = 360;
    public float nightLengthInSeconds = 210;

    public static float dayLength;
    public static float nightLength;
    static Timer wholeDayTimer;

    void Start()
    {
        dayLength = dayLengthInSeconds;
        nightLength = nightLengthInSeconds;

        wholeDayTimer = new Timer(dayLength + nightLength);
        wholeDayTimer.Start();
    }

    void FixedUpdate()
    {
        if (GameState.IsGameplayPaused())
            return;

        wholeDayTimer.Update(Time.fixedDeltaTime);
        wholeDayTimer.StartIfNotRunning();
        Debug.Log(String.Format("{0}: {1}", IsDay() ? "Day" : "Night", GetHourStr()));
    }

    public static bool IsDay()
    {
        return wholeDayTimer.GetTimeElapsed() <= dayLength;
    }

    public static bool IsNight()
    {
        return wholeDayTimer.GetTimeElapsed() > dayLength;
    }

    public static float GetMiddayPercentage()
    {
        float time = wholeDayTimer.GetTimeElapsed() + nightLength / 2f;
        if (time > wholeDayTimer.GetWaitTime())
            time -= wholeDayTimer.GetWaitTime();

        float midday = (nightLength + dayLength) / 2f;
        float distance = Mathf.Abs(midday - time);
        return 1f - distance / midday;
    }

    public static float GetMidnightPercentage()
    {
        return 1f - GetMiddayPercentage();
    }

    public static string GetHourStr()
    {
        float gameHourInSeconds = wholeDayTimer.GetWaitTime() / 24f;

        float hour = (wholeDayTimer.GetTimeElapsed() + nightLength / 2f) / gameHourInSeconds;
        while (hour >= 24f)
            hour -= 24f;

        int gameHour = Mathf.FloorToInt(hour);
        int gameMinutes = Mathf.FloorToInt((hour - Mathf.Floor(hour)) * 60f);

        return string.Format("{0}:{1}", gameHour, gameMinutes);
    }
}   
