using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridKnockback))]
public class TakeKnockbackInteractable : InteractableFunction
{
    public float defaultKnockSpeed = 1f;
    public int defaultStepCount = 1;
    GridKnockback gridKnockback;

    public override void Initialize()
    {
        gridKnockback = GetComponent<GridKnockback>();
    }

    public override void Action(Selector selector)
    {
        KnockbackInfo knockback = selector.gameObject.GetComponent<KnockbackInfo>();
        FacingDirection facingDirection = selector.gameObject.GetComponentInParent<FacingDirection>();

        if (facingDirection == null)
            return;

        gridKnockback.ApplyKnockback(
            facingDirection.Get(),
            (knockback != null) ? knockback.knockSpeed : defaultKnockSpeed,
            (knockback != null) ? knockback.stepCount : defaultStepCount
        );
    }
}