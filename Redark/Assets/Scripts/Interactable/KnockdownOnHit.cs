using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(KnockdownCounter))]
public class KnockdownOnHit : InteractableFunction
{
    KnockdownCounter knockdown;

    public override void Initialize()
    {
        knockdown = GetComponent<KnockdownCounter>();
    }

    public override void Action(Selector selector)
    {
        knockdown.StartKnockdown();
    }
}
