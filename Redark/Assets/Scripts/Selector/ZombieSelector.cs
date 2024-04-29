using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Selector))]
[RequireComponent(typeof(PlaceInFront))]
public class ZombieSelector : MonoBehaviour
{
    public float timeToAct = 1f;

    Selector selector;
    Timer actionTimer;
    void Start()
    {
        selector = GetComponent<Selector>();
        actionTimer = new Timer(timeToAct);
        actionTimer.Start();
    }

    void Update()
    {
        if (actionTimer.Finished())
        {
            Act();
            actionTimer.Start();
        }
    }

    void Act()
    {
        selector.TryHit();
    }

    void FixedUpdate()
    {
        actionTimer.Update(Time.fixedDeltaTime);
    }
}
