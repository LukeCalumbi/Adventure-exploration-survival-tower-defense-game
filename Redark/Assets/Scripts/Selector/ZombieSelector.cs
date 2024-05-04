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
        if (GameState.IsGameplayPaused())
            return;

        if (selector == null)
        {
            Destroy(this.gameObject);
            return;
        }

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
        if (GameState.IsGameplayPaused())
            return;

        actionTimer.Update(Time.fixedDeltaTime);
    }
}
