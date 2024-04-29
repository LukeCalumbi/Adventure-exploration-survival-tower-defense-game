using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FacingDirection))]
[RequireComponent(typeof(GridMovement))]
public class FaceLastMoveDirection : MonoBehaviour
{
    FacingDirection facingDirection;
    GridMovement movement;

    void Start()
    {
        facingDirection = GetComponent<FacingDirection>();
        movement = GetComponent<GridMovement>();
    }

    void Update()
    {
        if (movement.IsMoving())
            facingDirection.Set(movement.GetMovementDirection());
    }
}
