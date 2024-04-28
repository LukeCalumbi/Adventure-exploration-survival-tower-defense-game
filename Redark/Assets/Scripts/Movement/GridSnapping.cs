using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSnapping : MonoBehaviour
{
    public const float TILE_SIZE = 1f;
    public bool keepSnapping = true;

    void LateUpdate()
    {
        if (!keepSnapping)
            return;

        transform.position = ClosestSnapPointOf(transform.position);
    }

    public void DisableSnapping()
    {
        keepSnapping = false;
    }

    public void EnableSnapping()
    {
        keepSnapping = true;
    }

    public static Vector3 ClosestSnapPointOf(Vector3 position) {
        return new Vector3(
            Mathf.Sign(position.x) * (Mathf.Floor(Mathf.Abs(position.x)) + TILE_SIZE / 2f), 
            Mathf.Sign(position.y) * (Mathf.Floor(Mathf.Abs(position.y)) + TILE_SIZE / 2f), 
            Mathf.Sign(position.z) * (Mathf.Floor(Mathf.Abs(position.z)) + TILE_SIZE / 2f)
        );
    }
}
