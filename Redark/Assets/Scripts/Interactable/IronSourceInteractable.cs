using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class IronSourceInteractable : MonoBehaviour
{
    public List<string> authorizedTags = new List<string>();
    public int amount = 1;

    Interactable interactable;

    void Start()
    {
        interactable = GetComponent<Interactable>();
        interactable.AddOnHitCallback(AddIron);
    }

    void AddIron(Selector selector)
    {
        if (authorizedTags.Contains(selector.gameObject.tag))
            IronManager.AddIron(amount);
    }
}
