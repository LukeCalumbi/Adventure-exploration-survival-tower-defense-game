using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class DamageByTime : MonoBehaviour
{
    public float time = 1f;
    public int damage = 1;
    public bool killOnFinish = false;

    Health health;
    Timer timer;

    void Start()
    {
        health = GetComponent<Health>();
        timer = new Timer(time);
        timer.Start();
    }

    void FixedUpdate()
    {
        if (GameState.IsGameplayPaused())
            return;

        if (timer.Finished()) 
        {
            if (killOnFinish)
                health.Kill();
            else
                health.DoDamage(damage);

            timer.Start();
        }

        timer.Update(Time.fixedDeltaTime);
    }
}
