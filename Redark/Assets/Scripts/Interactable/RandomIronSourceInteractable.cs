using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomIronSourceInteractable : InteractableFunction
{
    public int amount = 1;
    public float chance = 0.5f;

    public override void Action(Selector selector)
    {
        if (Random.Range(0f, 1f) > chance)
            return;

        IronManager.AddIron(amount);
    }
}
