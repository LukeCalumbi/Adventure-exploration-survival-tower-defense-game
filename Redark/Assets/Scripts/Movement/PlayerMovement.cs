using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridMovement))]
public class PlayerMovement : MonoBehaviour
{
    public float timeToWalkInDifferentDirection = 0.1f;

    GridMovement movement;
    Vector3 facingDirection = Vector3.right;
    Timer walkInDifferentDirectionTimer;

    void Start()
    {
        movement = GetComponent<GridMovement>();
        walkInDifferentDirectionTimer = new Timer(timeToWalkInDifferentDirection);
    }

    void Update()
    {
        Vector3 direction = GetDirection();

        if (direction == Vector3.zero)
        {
            walkInDifferentDirectionTimer.ForceEnd();
            return;
        }

        if (direction != facingDirection && movement.IsIdle()){
            walkInDifferentDirectionTimer.Start();
            facingDirection = direction;
            return;
        }

        facingDirection = direction;

        if (walkInDifferentDirectionTimer.Finished() || walkInDifferentDirectionTimer.NeverRan())
            movement.MoveTowardsDirection(direction);
    }

    void FixedUpdate()
    {
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

    public Vector3 GetFacingDirection()
    {
        return facingDirection;
    }

    public Vector3 GetSnapPointInFrontOfPlayer()
    {
        if (movement.TilesUntilTarget() > 0.5f)
            return movement.GetSnapPointAt(facingDirection, 2);

        return movement.GetNeighbourSnapPoint(facingDirection);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, facingDirection * GridSnapping.TILE_SIZE * 2);
    }
}
