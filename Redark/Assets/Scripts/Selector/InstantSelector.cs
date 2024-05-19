using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Selector))]
public class InstantSelector : MonoBehaviour
{
    private Selector _selector;
    
    public bool interact = true;
    public bool hit = true;

    private void Start()
    {
        _selector = GetComponent<Selector>();
        if (interact) _selector.TryInteractAll();
        if (hit) _selector.TryHitAll();
    }
}
