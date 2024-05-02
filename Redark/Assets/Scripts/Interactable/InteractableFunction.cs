using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class InteractableFunction : MonoBehaviour
{
    public List<string> onInteractTags = new List<string>();
    public List<string> onHitTags = new List<string>();
    Interactable interactable;

    void Start()
    {
        interactable = GetComponent<Interactable>();
        
        foreach (string tag in onInteractTags)
            interactable.AddInteractCallback(tag, Action);

        foreach (string tag in onHitTags)
            interactable.AddHitCallback(tag, Action);

        Initialize();
    }

    public virtual void Initialize() {}
    public virtual void Action(Selector selector) {}
}
