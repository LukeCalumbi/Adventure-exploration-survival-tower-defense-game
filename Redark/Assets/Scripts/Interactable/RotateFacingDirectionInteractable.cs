using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FacingDirection))]
public class RotateFacingDirectionInteractable : InteractableFunction
{
    FacingDirection facingDirection;

    public override void Initialize()
    {
        facingDirection = GetComponent<FacingDirection>();
    }

    public override void Action(Selector selector)
    {
        Debug.Log("oi");
        facingDirection.Rotate();
    }
}
