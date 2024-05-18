using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class HourUI : MonoBehaviour
{
    public string header = "Hour: ";
    TextMeshProUGUI textUI;

    void Start()
    {
        textUI = GetComponent<TextMeshProUGUI>();
    }

    void FixedUpdate()
    {
        textUI.text = String.Format("{0}{1}", header, DayNightCicle.GetHourStr());
    }
}
