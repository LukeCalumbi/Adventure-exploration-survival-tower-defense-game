using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridMovement))]
[RequireComponent(typeof(FacingDirection))]
public class PlayerMovement : MonoBehaviour
{
    public float timeToWalkInDifferentDirection = 0.1f;

    GridMovement movement;
    FacingDirection facingDirection;
    Timer walkInDifferentDirectionTimer;

    void Start()
    {
        movement = GetComponent<GridMovement>();
        facingDirection = GetComponent<FacingDirection>();
        walkInDifferentDirectionTimer = new Timer(timeToWalkInDifferentDirection);
    }

    void Update()
    {
        if (GameState.IsGameplayPaused())
            return;

        Vector3 direction = GetDirection();

        if (direction == Vector3.zero)
        {
            walkInDifferentDirectionTimer.ForceEnd();
            return;
        }

        if (!facingDirection.IsSame(direction) && movement.IsIdle()){
            walkInDifferentDirectionTimer.Start();
            facingDirection.Set(direction);
            return;
        }

        facingDirection.Set(direction);

        if (walkInDifferentDirectionTimer.Finished() || walkInDifferentDirectionTimer.NeverRan()) 
        {
            movement.MoveTowardsDirection(direction);
        }
    }

    void FixedUpdate()
    {
        if (GameState.IsGameplayPaused())
            return;

        walkInDifferentDirectionTimer.Update(Time.fixedDeltaTime);
    }

    Vector3 GetDirection()
    {
        Vector3 direction = Vector3.zero;

        Func<KeyCode, int> keyStrength = (key) => Input.GetKey(key) ? 1 : 0;
        Func<KeyCode, KeyCode, int> GetAxis = (negative, positive) => keyStrength(positive) - keyStrength(negative);

        direction.x = GetAxis(KeyCode.A, KeyCode.D);
        direction.y = GetAxis(KeyCode.S, KeyCode.W);

        return GridMovement.ClosestDirectionVector(direction);
    }
}
