using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    public Slider healthBar;
    public Health health;
    
    void Update()
    {
        healthBar.value = 1-health.GetHealthPercentage();
    }
}
