using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class IronUi : MonoBehaviour
{
    public string header = "Iron: ";
    TextMeshProUGUI textUI;

    void Start()
    {
        textUI = GetComponent<TextMeshProUGUI>();
    }

    void FixedUpdate()
    {
        textUI.text = String.Format("{0}{1}", header, IronManager.GetIronCount());
    }
}
