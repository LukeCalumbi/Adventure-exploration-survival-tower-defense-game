using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DayNightScreenTint : MonoBehaviour
{
    public Color midnightColor;

    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        float nightMultipliyer = DayNightCicle.GetMidnightPercentage();
        if (DayNightCicle.IsNight())
            nightMultipliyer = Mathf.Sqrt(nightMultipliyer);

        spriteRenderer.color = midnightColor * nightMultipliyer;
    }
}
