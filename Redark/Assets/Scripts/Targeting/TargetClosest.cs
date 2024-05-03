using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetClosest : TargetingSystem
{
    public float detectionRadiusInTiles = 5f;

    public override void UpdateTarget()
    {
        List<Collider2D> colliders = new List<Collider2D>(Physics2D.OverlapCircleAll(transform.position, detectionRadiusInTiles * GridSnapping.TILE_SIZE).Where(
            (Collider2D collider) => collider.gameObject != this.gameObject && HasTargetTag(collider.tag)
        ));

        if (colliders.Count == 0)
            return;

        colliders.Sort(
            delegate (Collider2D a, Collider2D b) {
                float distA = Vector2.Distance(transform.position, a.transform.position) / GetPreferenceOfTag(a.tag);
                float distB = Vector2.Distance(transform.position, b.transform.position) / GetPreferenceOfTag(b.tag);
                return (distA < distB) ? -1 : (distA > distB) ? 1 : 0;
            }
        );

        cachedTarget = colliders[0].transform;
    }
}
