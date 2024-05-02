using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class IronSourceInteractable : InteractableFunction
{
    public int amount = 1;

    public override void Action(Selector selector)
    {
        IronManager.AddIron(amount);
    }
}
