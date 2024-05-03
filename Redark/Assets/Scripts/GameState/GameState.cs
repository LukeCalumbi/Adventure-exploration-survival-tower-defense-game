using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    static bool pauseGameplay = false;

    public static bool IsGameplayPaused()
    {
        return pauseGameplay;
    }

    public static void PauseGameplay()
    {
        pauseGameplay = true;
    }

    public static void ResumeGameplay()
    {
        pauseGameplay = false;
    }
}
