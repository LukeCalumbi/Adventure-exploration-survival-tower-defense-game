using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetClosest : TargetingSystem
{
    public float detectionRadiusInTiles = 5f;
    public float minDistance = 2f;
    public bool checkVisibility = false;

    public override void UpdateTarget()
    {
        List<Collider2D> colliders = new List<Collider2D>(Physics2D.OverlapCircleAll(transform.position, detectionRadiusInTiles * GridSnapping.TILE_SIZE).Where(
            (Collider2D collider) => collider.gameObject != this.gameObject && HasTargetTag(collider.tag) && Vector3.Distance(transform.position, collider.transform.position) >= minDistance && IsVisible(collider) 
        ));

        if (colliders.Count == 0)
        {
            cachedObject = null;
            cachedTarget = null;
            return;
        }

        colliders.Sort(
            delegate (Collider2D a, Collider2D b) {
                float distA = Vector2.Distance(transform.position, a.transform.position) / GetPreferenceOfTag(a.tag);
                float distB = Vector2.Distance(transform.position, b.transform.position) / GetPreferenceOfTag(b.tag);
                return (distA < distB) ? -1 : (distA > distB) ? 1 : 0;
            }
        );

        cachedObject = colliders[0].gameObject;
        cachedTarget = colliders[0].transform.position;
    }

    public bool IsVisible(Collider2D collider)
    {
        if (!checkVisibility)
            return true;

        List<RaycastHit2D> hits = Physics2D.RaycastAll(transform.position, collider.transform.position - transform.position, detectionRadiusInTiles * GridSnapping.TILE_SIZE).ToList();
        if (hits.Count > 0 && hits.First().collider.gameObject == collider.gameObject)
            return true;

        if (hits.Count > 1 && hits[1].collider.gameObject == collider.gameObject)
            return true;

        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, this.detectionRadiusInTiles * GridSnapping.TILE_SIZE);
    }
}
