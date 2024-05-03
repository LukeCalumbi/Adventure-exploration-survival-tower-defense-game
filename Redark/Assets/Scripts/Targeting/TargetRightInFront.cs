using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(FacingDirection))]
public class TargetRightInFront : TargetingSystem
{
    public float distanceInTiles = 5f;
    FacingDirection facingDirection;

    void Start()
    {
        facingDirection = GetComponent<FacingDirection>();
    }

    public override void UpdateTarget()
    {
        List<RaycastHit2D> hits = new List<RaycastHit2D>(Physics2D.RaycastAll(transform.position, facingDirection.Get(), distanceInTiles * GridSnapping.TILE_SIZE).Where(
            (RaycastHit2D hit) => hit.collider.gameObject != this.gameObject && targetTags.Contains(hit.collider.tag)
        ));

        if (hits.Count == 0)
            return;

        hits.Sort(
            delegate (RaycastHit2D a, RaycastHit2D b) {
                float distA = Vector2.Distance(transform.position, a.collider.transform.position);
                float distB = Vector2.Distance(transform.position, b.collider.transform.position);
                return (distA < distB) ? -1 : (distA > distB) ? 1 : 0;
            }
        );

        cachedTarget = hits[0].collider.transform;
    }
}
