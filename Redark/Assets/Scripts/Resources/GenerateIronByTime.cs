using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GenerateIronByTime : MonoBehaviour
{
    public Timer timer = new Timer(1f);
    public int amount = 1;
    
    [Range(0f, 1f)]
    public float chance;

    private void Start()
    {
        timer.Start();
    }

    private void Update()
    {
        if (timer.Finished())
        {
            TryGetIron();
            timer.Start();
        }
    }

    private void FixedUpdate()
    {
        timer.Update(Time.fixedDeltaTime);
    }

    public void TryGetIron()
    {
        if (Random.Range(0f, 1f) < chance)
            IronManager.AddIron(amount);
    }
}
