using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportOnInteract : InteractableFunction
{
    public Vector3 endPosition = Vector3.zero;

    public override void Initialize()
    {
        endPosition = GridSnapping.ClosestSnapPointOf(endPosition);
    }

    public override void Action(Selector selector)
    {
        selector.gameObject.transform.parent.position = endPosition;
    }
}
