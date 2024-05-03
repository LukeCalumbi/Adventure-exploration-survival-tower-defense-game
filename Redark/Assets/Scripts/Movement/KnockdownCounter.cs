using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockdownCounter : MonoBehaviour
{
    public float timeInKnockdown = 1f;
    Timer knockdownTimer;

    void Start()
    {
        knockdownTimer = new Timer(timeInKnockdown);
    }

    void FixedUpdate()
    {
        knockdownTimer.Update(Time.fixedDeltaTime);
    }

    public void StartKnockdown()
    {
        knockdownTimer.Start();
    }

    public bool IsKnockedDown()
    {
        return knockdownTimer.IsRunning();
    }
}
