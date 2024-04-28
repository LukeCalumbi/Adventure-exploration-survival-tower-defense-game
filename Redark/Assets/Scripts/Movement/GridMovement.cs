using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(GridSnapping))]
public class GridMovement : MonoBehaviour
{
    public float moveSpeed = 5f,tolerance = float.Epsilon;
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
        targetSnapPoint = GridSnapping.ClosestSnapPointOf(transform.position);
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

        MoveAndKeepDirection();
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
        requestedDirection = Vector3.zero;
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

        return Vector3.Dot(distanceNow, distanceNext) <= 0.01f;
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
        if (!moving)
            return Vector3.zero;

        return (targetSnapPoint - transform.position).normalized;
    }

    public Vector3 GetNextPosition()
    {
        Vector3 direction = GetMovementDirection();
        return transform.position + direction * moveSpeed * Time.fixedDeltaTime;
    }

    public bool IsMoving()
    {
        return moving;
    }

    public bool IsIdle()
    {
        return !moving;
    }

    bool IsNeighbourTileEmpty(Vector3 direction)
    {
        if (!checkCollision)
            return true;

        Vector2 boxEntents = (GridSnapping.TILE_SIZE - tolerance) * Vector2.one;

        List<Collider2D> colliders = new List<Collider2D>(Physics2D.OverlapBoxAll(GetNeighbourSnapPoint(direction), boxEntents, 0f));
        List<Collider2D> collidersFiltered = new List<Collider2D>(
            colliders.Where(
                (Collider2D collider) => collider.gameObject != gameObject && !ignoreCollisionsWithTags.Contains(collider.gameObject.tag)
            )
        );

        return collidersFiltered.Count == 0;
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
