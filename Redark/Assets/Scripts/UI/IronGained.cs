using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
[RequireComponent(typeof(TimedText))]
public class IronGained : MonoBehaviour
{
    public string textToFormat = "{0} iron";
    TextMeshProUGUI textUI;
    TimedText timedText;
    int lastIronCount;

    void Start()
    {
        textUI = GetComponent<TextMeshProUGUI>();
        timedText = GetComponent<TimedText>();
    }

    void FixedUpdate()
    {
        if (lastIronCount == IronManager.GetIronCount())
            return;

        Trigger();
        lastIronCount = IronManager.GetIronCount();
    }

    void Trigger()
    {
        int gained = IronManager.GetIronCount() - lastIronCount;
        string gainedStr = gained >= 0 ? String.Format("+{0}", gained) : String.Format("-{0}", -gained);
        textUI.text = String.Format(textToFormat, gainedStr);

        timedText.Trigger();
    }
}
