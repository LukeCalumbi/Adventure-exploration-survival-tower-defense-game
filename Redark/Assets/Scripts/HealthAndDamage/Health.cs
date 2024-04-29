using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    
    public int maxHealth = 10;
    Counter health;

    void Start()
    {
        health = new Counter(maxHealth, maxHealth);
    }

    public void Regenerate(int amount)
    {
        health.CountUp(amount);
    }

    public void Fill()
    {
        health.Fill();
    }

    public void DoDamage(int amount)
    {
        health.CountDown(amount);
    }

    public void Kill()
    {
        health.Reset();
    }

    public bool IsDead()
    {
        return health.IsZero();
    }
}