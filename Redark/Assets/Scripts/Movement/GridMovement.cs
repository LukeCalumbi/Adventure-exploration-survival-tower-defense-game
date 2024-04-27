using System;
using System.Collections.Generic;
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
    Vector3 targetSnapPoint = Vector3.zero;
    bool moving = false;

    void Start()
    {
        snapComponent = GetComponent<GridSnapping>();
        targetSnapPoint = GridSnapping.closestSnapPointOf(transform.position);
        StopMoving();
    }

    void FixedUpdate()
    {
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
        moving = true;
    }

    private void MoveAndUpdateDirection(Vector3 direction)
    {
        float totalMovement = moveSpeed * Time.fixedDeltaTime;
        float distanceToTarget = (targetSnapPoint - transform.position).magnitude;
        float remainingMovement = totalMovement - distanceToTarget;

        transform.position = targetSnapPoint + direction * remainingMovement;
        targetSnapPoint = GetNextStepSnapPoint(direction);
    }

    private void MoveAndKeepDirection()
    {
        transform.position = GetNextPosition();
    }

    private void StopMoving()
    {
        transform.position = targetSnapPoint;
        snapComponent.EnableSnapping();
        moving = false;
    }

    bool WillOvershootTarget()
    {
        return WillOvershootPoint(targetSnapPoint);
    }

    bool WillOvershootCurrentTile()
    {
        Vector3 tilePosition = GridSnapping.closestSnapPointOf(transform.position);
        return WillOvershootPoint(tilePosition);
    }

    bool WillOvershootPoint(Vector3 point)
    {
        Vector3 distanceNow = point - transform.position;
        Vector3 distanceNext = point - GetNextPosition();

        return Vector3.Dot(distanceNow, distanceNext) <= 0.01f;
    }

    Vector3 GetNeighbourSnapPoint(Vector3 direction)
    {
        return GetSnapPointAt(direction, 1);
    }

    Vector3 GetNextStepSnapPoint(Vector3 direction)
    {
        return GetSnapPointAt(direction, stepCount);
    }

    Vector3 GetSnapPointAt(Vector3 direction, int distance)
    {
        Vector3 thisTilePosition = GridSnapping.closestSnapPointOf(transform.position);
        return GridSnapping.closestSnapPointOf(thisTilePosition + direction * GridSnapping.TILE_SIZE * distance);
    }

    public Vector3 GetMovementDirection()
    {
        if (!moving)
            return Vector3.zero;

        return (targetSnapPoint - transform.position).normalized;
    }

    public Vector3 GetNextPosition()
    {
        Vector3 direction = GetMovementDirection();
        return transform.position + direction * moveSpeed * Time.fixedDeltaTime;
    }

    bool IsNeighbourTileEmpty(Vector3 direction)
    {
        if (!checkCollision)
            return true;

        Vector2 boxEntents = (GridSnapping.TILE_SIZE - 0.1f) * Vector2.one;

        RaycastHit2D hit = Physics2D.BoxCast(GetNeighbourSnapPoint(direction), boxEntents, 0f, direction, 0.05f);

        if (hit.collider == null || hit.collider == this.gameObject || ignoreCollisionsWithTags.Contains(hit.collider.tag))
            return true;

        return false;
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
