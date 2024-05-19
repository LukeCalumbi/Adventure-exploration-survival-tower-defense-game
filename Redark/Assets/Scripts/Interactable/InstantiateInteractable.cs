using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class InstantiateInteractable : InteractableFunction
{
    public GameObject prefab;
    public bool setPosition = false;

    public override void Action(Selector selector)
    {
        GameObject instantiatedObject = Instantiate(prefab);
        if (setPosition)
            instantiatedObject.transform.position = transform.position;
    }
}
