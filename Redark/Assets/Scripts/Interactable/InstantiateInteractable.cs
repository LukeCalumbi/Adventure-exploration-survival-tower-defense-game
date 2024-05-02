using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class InstantiateInteractable : InteractableFunction
{
    public GameObject prefab;

    public override void Action(Selector selector)
    {
        Instantiate(prefab);
    }
}
