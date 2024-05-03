using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReadText : MonoBehaviour
{
    public TextMeshProUGUI textUI;
    public List<string> textSections = new List<string>();
    int current = 0;

    void Start()
    {
        SetTextOrKill();
        GameState.PauseGameplay();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            current += 1;
            SetTextOrKill();
        }
    }

    public void SetTextOrKill()
    {
        String text = GetCurrentTextSection();
        if (text == null) 
        {
            GameState.ResumeGameplay();
            Destroy(this.gameObject);
            return;
        }

        textUI.text = text;
    }

    public String GetCurrentTextSection()
    {
        if (current < textSections.Count)
            return textSections[current];

        return null;
    }
}
