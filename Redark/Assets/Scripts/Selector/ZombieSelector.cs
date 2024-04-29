using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Selector))]
[RequireComponent(typeof(PlaceInFront))]
public class ZombieSelector : MonoBehaviour
{
    public float timeToAct = 1f;

    public int damage = 1;

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
        GameObject selectedObject = selector.GetSelectedObject();

        if (selectedObject == null)
            return;

        Health health = selectedObject.GetComponent<Health>();

        if (health == null)
            return;

        health.DoDamage(damage);
    }

    void FixedUpdate()
    {
        actionTimer.Update(Time.fixedDeltaTime);
    }
}
