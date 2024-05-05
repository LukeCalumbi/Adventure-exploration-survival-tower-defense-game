using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TimedText : MonoBehaviour
{
    public Timer timer;

    TextMeshProUGUI textUI;

    void Start()
    {
        textUI = GetComponent<TextMeshProUGUI>();
        timer.ForceEnd();
    }

    void FixedUpdate()
    {
        Color textColor = textUI.color;
        textColor.a = timer.GetRemainingTimePercentage();

        textUI.color = textColor;
        timer.Update(Time.fixedDeltaTime);
    }

    public void Trigger()
    {
        timer.Start();
    }
}
