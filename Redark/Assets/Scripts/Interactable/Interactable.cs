using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField] public UnityEvent<Selector> onInteract;
    [SerializeField] public UnityEvent<Selector> onHit;

    public void Interact(Selector selector)
    {
        onInteract.Invoke(selector);
    }

    public void Hit(Selector selector)
    {
        onHit.Invoke(selector);
    }

    public void AddOnInteractCallback(UnityAction<Selector> action)
    {
        onInteract.AddListener(action);
    }

    public void AddOnHitCallback(UnityAction<Selector> action)
    {
        onHit.AddListener(action);
    }
}
