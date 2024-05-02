using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField] public Dictionary<string, UnityEvent<Selector>> onInteract = new Dictionary<string, UnityEvent<Selector>>();
    [SerializeField] public Dictionary<string, UnityEvent<Selector>> onHit = new Dictionary<string, UnityEvent<Selector>>();

    public void Interact(Selector selector)
    {
        if (onInteract.ContainsKey(selector.gameObject.tag))
            onInteract[selector.gameObject.tag].Invoke(selector);
    }

    public void Hit(Selector selector)
    {
        if (onHit.ContainsKey(selector.gameObject.tag))
            onHit[selector.gameObject.tag].Invoke(selector);
    }

    public void AddInteractCallback(string tag, UnityAction<Selector> action)
    {
        if (!onInteract.ContainsKey(tag))
            onInteract.Add(tag, new UnityEvent<Selector>());

        onInteract[tag].AddListener(action);
    }

    public void AddHitCallback(string tag, UnityAction<Selector> action)
    {
        if (!onHit.ContainsKey(tag))
            onHit.Add(tag, new UnityEvent<Selector>());

        onHit[tag].AddListener(action);
    }
}
