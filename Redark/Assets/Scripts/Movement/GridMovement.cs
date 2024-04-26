using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
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
    public bool checkCollision = true;
    public List<String> ignoreCollisionsWithTags;

    GridSnapping snapComponent;
    MovementDirection requestedDirection = MovementDirection.Zero;
    MovementDirection movementDirection = MovementDirection.Zero;
    Vector3 targetSnapPoint = Vector3.zero;

    void Start()
    {
        snapComponent = GetComponent<GridSnapping>();
        movementDirection = MovementDirection.Zero;
        requestedDirection = MovementDirection.Zero;
        targetSnapPoint = GridSnapping.closestSnapPointOf(transform.position);
    }

    /*
    Enable This is you want to test movement very quick
    void Update()
    {
        Vector3 direction = Vector3.zero;
        direction.x = Convert.ToInt32(Input.GetKey(KeyCode.D)) - Convert.ToInt32(Input.GetKey(KeyCode.A));
        direction.y = Convert.ToInt32(Input.GetKey(KeyCode.W)) - Convert.ToInt32(Input.GetKey(KeyCode.S));
        MoveTowardsDirection(direction);
    }
    */

    void FixedUpdate()
    {
        if (movementDirection == MovementDirection.Zero && requestedDirection != MovementDirection.Zero && IsTileInDirectionEmpty(requestedDirection))
        {
            movementDirection = requestedDirection;
            targetSnapPoint = GetNextSnapPointInDirection(movementDirection);
            snapComponent.DisableSnapping();
            return;
        }

        if (movementDirection != MovementDirection.Zero && IsPositionCloseToSnapPosition())
        {
            if (requestedDirection == MovementDirection.Zero || !IsTileInDirectionEmpty(requestedDirection)) 
            {  
                movementDirection = MovementDirection.Zero;
                targetSnapPoint = GridSnapping.closestSnapPointOf(transform.position);
                snapComponent.EnableSnapping();
                return;
            }

            movementDirection = requestedDirection;
            targetSnapPoint = GetNextSnapPointInDirection(movementDirection);
            MoveAndTurn(requestedDirection);
            return;
        }

        Move();
    }

    private bool IsPositionCloseToSnapPosition()
    {
        Vector3 differenceBefore = targetSnapPoint - transform.position;
        Vector3 differenceAfter = targetSnapPoint - GetNextPosition();

        Func<float, int> sign = x => (x > 0) ? 1 : (x < 0) ? -1 : 0;
        return sign(differenceBefore.x) != sign(differenceAfter.x) || sign(differenceBefore.y) != sign(differenceAfter.y);
    }

    private void MoveAndTurn(MovementDirection newDirection)
    {
        float totalMovement = moveSpeed * Time.fixedDeltaTime;
        float distanceWalkedBeforeTurn = (targetSnapPoint - transform.position).magnitude;
        float remainingMovement = totalMovement - distanceWalkedBeforeTurn;

        transform.position = targetSnapPoint + DirectionToVector(newDirection) * remainingMovement;
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

    private Vector3 GetNextSnapPointInDirection(MovementDirection direction)
    {
        Vector3 directionVector = DirectionToVector(direction);
        Vector3 currentSnapPoint = GridSnapping.closestSnapPointOf(transform.position);
        return GridSnapping.closestSnapPointOf(currentSnapPoint + directionVector * GridSnapping.TILE_SIZE);
    }

    private bool IsTileInDirectionEmpty(MovementDirection direction)
    {
        if (!checkCollision)
            return true;

        Vector2 boxExtents = GridSnapping.TILE_SIZE * Vector2.one;
        Vector2 directionVector = DirectionToVector(direction);
        RaycastHit2D hit = Physics2D.BoxCast(GridSnapping.closestSnapPointOf(transform.position), boxExtents, 0f, directionVector, GridSnapping.TILE_SIZE * 1f);

        if (hit.collider == null || hit.collider == this || ignoreCollisionsWithTags.Contains(hit.collider.tag))
            return true;

        return false;
    }

    public Vector3 GetMovementVector()
    {
        return DirectionToVector(movementDirection);
    }

    public Vector2 GetMovementVector2D()
    {
        Vector3 direction3d = GetMovementVector();
        return new Vector2(direction3d.x, direction3d.y);
    }

    public void MoveTowardsDirection(MovementDirection direction)
    {
        requestedDirection = direction;
    }

    public void MoveTowardsDirection(Vector3 direction)
    {
        MoveTowardsDirection(VectorToDirection(direction));
    }

    public static Vector3 DirectionToVector(MovementDirection direction)
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

    public static MovementDirection VectorToDirection(Vector3 vector)
    {
        if (vector.x == 0f && vector.y == 0f)
            return MovementDirection.Zero;

        List<MovementDirection> directions = new List<MovementDirection> { MovementDirection.Up, MovementDirection.Down, MovementDirection.Left, MovementDirection.Right };
        directions.Sort(delegate(MovementDirection a, MovementDirection b) { 
            return (int)(Vector3.Dot(DirectionToVector(a), vector) - Vector3.Dot(DirectionToVector(b), vector));
        });

        return directions[3];
    }

    public static Vector3 ClosestDirectionVector(Vector3 vector)
    {
        List<Vector3> directions = new List<Vector3> { Vector3.up, Vector3.down, Vector3.left, Vector3.right };
        directions.Sort(delegate(Vector3 a, Vector3 b) { 
            return (int)(Vector3.Dot(a, vector) - Vector3.Dot(b, vector));
        });

        return directions[3];
    }
}
