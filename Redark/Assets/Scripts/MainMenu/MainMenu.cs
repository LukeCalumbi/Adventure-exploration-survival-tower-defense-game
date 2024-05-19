using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

enum MenuChoice
{
    Play,
    Exit,
    Nothing
}
public class MainMenu : MonoBehaviour
{
    private MenuChoice _choice = MenuChoice.Nothing;

    public void Start()
    {
        ScreenFade.StartFadeOut();
    }

    public void Update()
    {
        if (ScreenFade.IsFadeInComplete())
        {
            switch (_choice)
            {
                case MenuChoice.Play: 
                    GameState.ResumeGameplay();
                    ScreenFade.StartFadeOut();
                    SceneManager.LoadScene("TileMap");
                    break;
                
                case MenuChoice.Exit: 
                    Application.Quit();
                    break;
            }
        }
    }

    public void PlayGame()
    {
        ScreenFade.StartFadeIn();
        _choice = MenuChoice.Play;
    }

    public void QuitGame()
    {
        ScreenFade.StartFadeIn();
        _choice = MenuChoice.Exit;
    }
}
