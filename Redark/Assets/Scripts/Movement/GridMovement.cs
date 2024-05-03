using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(GridSnapping))]
public class GridMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public int stepCount = 1;
    public bool checkCollision = true;
    public List<string> ignoreCollisionsWithTags;

    GridSnapping snapComponent;
    Vector3 requestedDirection = Vector3.zero;
    Vector3 movementDirection = Vector3.zero;
    Vector3 targetSnapPoint = Vector3.zero;
    bool moving = false;

    void Start()
    {
        snapComponent = GetComponent<GridSnapping>();
        targetSnapPoint = GridSnapping.ClosestSnapPointOf(transform.position);
        StopMoving();
    }

    void FixedUpdate()
    {
        if (GameState.IsGameplayPaused())
            return;

        if (!moving && requestedDirection != Vector3.zero)
        {
            StartMoving(requestedDirection);
            return;
        }

        if (moving && WillOvershootCurrentTile() && !IsNeighbourTileEmpty(GetMovementDirection()))
        {
            StopMoving();
            return;
        }

        if (moving && WillOvershootTarget())
        {
            if (requestedDirection == Vector3.zero || !IsNeighbourTileEmpty(requestedDirection))
            {
                StopMoving();
                return;
            }

            MoveAndUpdateDirection(requestedDirection);
            return;
        }

        if (moving) 
            MoveAndKeepDirection();

        requestedDirection = Vector3.zero;
    }

    public void MoveTowardsDirection(Vector3 direction)
    {
        direction = ClosestDirectionVector(direction);
        requestedDirection = direction;
    }

    private void StartMoving(Vector3 direction)
    {
        if (direction == Vector3.zero || !IsNeighbourTileEmpty(direction))
            return;

        targetSnapPoint = GetNextStepSnapPoint(direction);
        snapComponent.DisableSnapping();
        movementDirection = ClosestDirectionVector(targetSnapPoint - GetCurrentSnapPoint());
        moving = true;

        MoveAndKeepDirection();
    }

    private void MoveAndUpdateDirection(Vector3 direction)
    {
        float totalMovement = moveSpeed * Time.fixedDeltaTime;
        float distanceToTarget = (targetSnapPoint - transform.position).magnitude;
        float remainingMovement = totalMovement - distanceToTarget;

        transform.position = targetSnapPoint + direction * remainingMovement;
        targetSnapPoint = GetNextStepSnapPoint(direction);
        movementDirection = ClosestDirectionVector(targetSnapPoint - GetCurrentSnapPoint());
    }

    private void MoveAndKeepDirection()
    {
        transform.position = GetNextPosition();
    }

    private void StopMoving()
    {
        transform.position = targetSnapPoint;
        movementDirection = Vector3.zero;
        snapComponent.EnableSnapping();
        moving = false;
    }

    public float TilesUntilTarget()
    {
        return (targetSnapPoint - transform.position).magnitude / GridSnapping.TILE_SIZE;
    }

    public bool WillOvershootTarget()
    {
        return WillOvershootPoint(targetSnapPoint);
    }

    public bool WillOvershootCurrentTile()
    {
        Vector3 tilePosition = GetCurrentSnapPoint();
        return WillOvershootPoint(tilePosition);
    }

    public bool WillOvershootPoint(Vector3 point)
    {
        Vector3 distanceNow = point - transform.position;
        Vector3 distanceNext = point - GetNextPosition();

        return Vector3.Dot(distanceNow, distanceNext) < 0f || distanceNow == Vector3.zero;
    }

    public Vector3 GetCurrentSnapPoint()
    {
        return GridSnapping.ClosestSnapPointOf(transform.position);
    }

    public Vector3 GetNeighbourSnapPoint(Vector3 direction)
    {
        return GetSnapPointAt(direction, 1);
    }

    public Vector3 GetNextStepSnapPoint(Vector3 direction)
    {
        return GetSnapPointAt(direction, stepCount);
    }

    public Vector3 GetSnapPointAt(Vector3 direction, int distance)
    {
        Vector3 thisTilePosition = GetCurrentSnapPoint();
        return GridSnapping.ClosestSnapPointOf(thisTilePosition + direction * GridSnapping.TILE_SIZE * distance);
    }

    public Vector3 GetMovementDirection()
    {
        return movementDirection;
    }

    public Vector3 GetNextPosition()
    {
        return transform.position + movementDirection * moveSpeed * Time.fixedDeltaTime;
    }

    public bool IsMoving()
    {
        return movementDirection != Vector3.zero;
    }

    public bool IsIdle()
    {
        return movementDirection == Vector3.zero;
    }

    bool IsNeighbourTileEmpty(Vector3 direction)
    {
        if (!checkCollision)
            return true;

        Vector2 boxEntents = 0.95f * GridSnapping.TILE_SIZE * Vector2.one;

        Vector3 position = GetCurrentSnapPoint() + 0.5f * GridSnapping.TILE_SIZE * direction;
        Vector3 neighbourPosition = GetNeighbourSnapPoint(direction);

        foreach (RaycastHit2D hit in Physics2D.CircleCastAll(position, GridSnapping.TILE_SIZE, direction, 1.5f * GridSnapping.TILE_SIZE))
        {
            if (hit.collider.gameObject == this.gameObject || ignoreCollisionsWithTags.Contains(hit.collider.tag))
                continue;

            GridMovement movement = hit.collider.gameObject.GetComponent<GridMovement>();
            if (movement == null) 
            {
                if (hit.collider.ClosestPoint(neighbourPosition) == (Vector2)neighbourPosition)
                    return false;
                
                continue;
            }

            if (movement.GetCurrentSnapPoint() == neighbourPosition)
                return false;

            Vector3 movementNeighbour = movement.GetNeighbourSnapPoint(movement.GetMovementDirection());
            if (movementNeighbour == neighbourPosition)
                return false;

            if (Vector2.Dot(movement.GetMovementDirection(), direction) > float.Epsilon && this.moveSpeed > movement.moveSpeed)
                return false;
        }

        return true;
    }

    public static Vector3 ClosestDirectionVector(Vector3 vector)
    {
        if (vector == Vector3.zero)
            return Vector3.zero;

        float dotUp = Vector3.Dot(Vector3.up, vector);
        float dotRight = Vector3.Dot(Vector3.right, vector);

        if (Mathf.Abs(dotUp) > Mathf.Abs(dotRight))
            return dotUp > 0f ? Vector3.up : Vector3.down;

        else
            return dotRight > 0f ? Vector3.right : Vector3.left;
    }
}
