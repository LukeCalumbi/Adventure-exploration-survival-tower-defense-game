using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGameplayOnLoad : MonoBehaviour
{
    void Start()
    {
        GameState.PauseGameplay();
    }

    void OnDestroy()
    {
        GameState.ResumeGameplay();
    }
}
