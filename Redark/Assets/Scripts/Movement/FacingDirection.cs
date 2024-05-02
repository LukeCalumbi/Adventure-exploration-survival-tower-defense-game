using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacingDirection : MonoBehaviour
{
    Vector3 facingDirection = Vector3.right;

    public void Set(Vector3 direction)
    {
        facingDirection = GridMovement.ClosestDirectionVector(direction);
    }

    public bool IsSame(Vector3 vector)
    {
        vector = GridMovement.ClosestDirectionVector(vector);
        return Vector3.Dot(facingDirection, vector) > 0.99f;
    }

    public Vector3 FacingSnapPoint()
    {
        Vector3 currentSnapPoint = GridSnapping.ClosestSnapPointOf(transform.position);
        Vector3 distance = transform.position - currentSnapPoint;

        if (Vector3.Dot(distance, facingDirection) < 0.01f)
            return GridSnapping.ClosestSnapPointOf(currentSnapPoint + GridSnapping.TILE_SIZE * facingDirection);

        return GridSnapping.ClosestSnapPointOf(currentSnapPoint + 2f * GridSnapping.TILE_SIZE * facingDirection);
    }

    public Vector3 Get()
    {
        return facingDirection;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Get() * GridSnapping.TILE_SIZE * 2);
    }
}
