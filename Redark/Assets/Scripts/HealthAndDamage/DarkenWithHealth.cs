using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Health))]
public class DarkenWithHealth : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Health health;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        float grayScale = Mathf.Clamp(health.GetHealthPercentage() + 0.2f, 0f, 1f);
        spriteRenderer.color = new Color(grayScale, grayScale, grayScale, 1f);
    }
}
