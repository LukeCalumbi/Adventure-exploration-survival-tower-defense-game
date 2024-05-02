using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetClosest : TargetingSystem
{
    public float detectionRadiusInTiles = 5f;

    public override Transform GetTarget(List<string> targetTags)
    {
        List<Collider2D> colliders = new List<Collider2D>(Physics2D.OverlapCircleAll(transform.position, detectionRadiusInTiles * GridSnapping.TILE_SIZE).Where(
            (Collider2D collider) => collider.gameObject != this.gameObject && targetTags.Contains(collider.tag)
        ));

        if (colliders.Count == 0)
            return null;

        colliders.Sort(
            delegate (Collider2D a, Collider2D b) {
                float distA = Vector2.Distance(transform.position, a.transform.position);
                float distB = Vector2.Distance(transform.position, b.transform.position);
                return (distA < distB) ? -1 : (distA > distB) ? 1 : 0;
            }
        );

        return colliders[0].transform;
    }
}
