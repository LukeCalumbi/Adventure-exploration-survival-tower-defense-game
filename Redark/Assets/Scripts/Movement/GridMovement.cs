using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementDirection {
    Up,
    Down,
    Left,
    Right,
    Zero
}

[RequireComponent(typeof(GridSnapping))]
public class GridMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    GridSnapping snapComponent;
    MovementDirection requestedDirection = MovementDirection.Zero;
    MovementDirection movementDirection = MovementDirection.Zero;

    void Start()
    {
        snapComponent = GetComponent<GridSnapping>();
        movementDirection = MovementDirection.Zero;
        requestedDirection = MovementDirection.Zero;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W))
            requestedDirection = MovementDirection.Up;

        else if (Input.GetKey(KeyCode.S))
            requestedDirection = MovementDirection.Down;

        else if (Input.GetKey(KeyCode.A))
            requestedDirection = MovementDirection.Left;

        else if (Input.GetKey(KeyCode.D))
            requestedDirection = MovementDirection.Right;

        else
            requestedDirection = MovementDirection.Zero;
    }

    void FixedUpdate()
    {
        if (movementDirection == MovementDirection.Zero && requestedDirection != MovementDirection.Zero)
        {
            movementDirection = requestedDirection;
            snapComponent.DisableSnapping();
            return;
        }
        

        if (movementDirection != MovementDirection.Zero && isPositionCloseToSnapPosition())
        {
            if (requestedDirection == MovementDirection.Zero) 
            {  
                movementDirection = MovementDirection.Zero;
                snapComponent.EnableSnapping();
                return;
            }

            movementDirection = requestedDirection;
            MoveAndTurn(requestedDirection);
            return;
        }

        Move();
    }

    private bool isPositionCloseToSnapPosition()
    {
        Vector3 differenceBefore = GridSnapping.closestSnapPointOf(transform.position) - transform.position;
        Vector3 differenceAfter = GridSnapping.closestSnapPointOf(transform.position) - GetNextPosition();
        return Mathf.Sign(differenceBefore.x) != Mathf.Sign(differenceAfter.x) || Mathf.Sign(differenceBefore.y) != Mathf.Sign(differenceAfter.y);
    }

    private void MoveAndTurn(MovementDirection newDirection)
    {
        float totalMovement = moveSpeed * Time.fixedDeltaTime;
        float distanceWalkedBeforeTurn = (GridSnapping.closestSnapPointOf(transform.position) - transform.position).magnitude;
        float remainingMovement = totalMovement - distanceWalkedBeforeTurn;

        transform.position = GridSnapping.closestSnapPointOf(transform.position) + GetDirectionVector(newDirection) * remainingMovement;
    }

    private void Move()
    {
        transform.position = GetNextPosition();
    }

    private Vector3 GetNextPosition()
    {
        Vector3 direction = GetMovementVector();
        return transform.position + direction * moveSpeed * Time.fixedDeltaTime;
    }

    public Vector3 GetMovementVector()
    {
        return GetDirectionVector(movementDirection);
    }

    public void MoveTowardsDirection(MovementDirection direction)
    {
        requestedDirection = direction;
    }

    public static Vector3 GetDirectionVector(MovementDirection direction)
    {
        switch (direction)
        {
            case MovementDirection.Up: return Vector3.up;
            case MovementDirection.Down: return Vector3.down;
            case MovementDirection.Left: return Vector3.left;
            case MovementDirection.Right: return Vector3.right;
            default: return Vector3.zero;
        }
    }
}
