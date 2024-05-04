using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(FacingDirection))]
public class TargetRightInFront : TargetingSystem
{
    public float distanceInTiles = 5f;
    public float minDistance = 2f;
    FacingDirection facingDirection;
    bool checkVisibility = false;

    void Start()
    {
        facingDirection = GetComponent<FacingDirection>();
    }

    public override void UpdateTarget()
    {
        List<RaycastHit2D> hits = new List<RaycastHit2D>(Physics2D.RaycastAll(transform.position, facingDirection.Get(), distanceInTiles * GridSnapping.TILE_SIZE)
            .Where((RaycastHit2D hit) => hit.collider.gameObject != this.gameObject && Vector3.Distance(transform.position, hit.collider.transform.position) >= minDistance)
            .TakeWhile((RaycastHit2D hit) => !checkVisibility || HasTargetTag(hit.collider.tag))
        );

        if (hits.Count == 0)
        {
            cachedObject = null;
            cachedTarget = null;
            return;
        }

        hits.Sort(
            delegate (RaycastHit2D a, RaycastHit2D b) {
                float distA = Vector2.Distance(transform.position, a.collider.transform.position) / GetPreferenceOfTag(a.collider.tag);
                float distB = Vector2.Distance(transform.position, b.collider.transform.position) / GetPreferenceOfTag(b.collider.tag);
                return (distA < distB) ? -1 : (distA > distB) ? 1 : 0;
            }
        );

        cachedObject = hits[0].collider.gameObject;
        cachedTarget = hits[0].collider.transform.position;
    }
}
