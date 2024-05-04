using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(FacingDirection))]
public class ClosestInDirection : TargetingSystem
{
    public float detectionRadiusInTiles = 5f;
    public float minDistance = 2f;
    public bool checkVisibility = false;
    public float distanceMultiplier = 2.0f;
    public float angleMultiplier = 1.0f;
    FacingDirection facingDirection;

    void Start()
    {
        facingDirection = GetComponent<FacingDirection>();
    }

    public override void UpdateTarget()
    {
        List<Collider2D> colliders = new List<Collider2D>(Physics2D.OverlapCircleAll(transform.position, detectionRadiusInTiles * GridSnapping.TILE_SIZE).Where(
            (Collider2D collider) => collider.gameObject != this.gameObject && HasTargetTag(collider.tag) && GetPreference(collider) > float.Epsilon && Vector3.Distance(transform.position, collider.transform.position) >= minDistance && IsVisible(collider) 
        ));

        if (colliders.Count == 0)
        {
            cachedObject = null;
            cachedTarget = null;
            return;
        }

        colliders.Sort(
            delegate (Collider2D a, Collider2D b) {
                float preferenceA = GetPreference(a);
                float preferenceB = GetPreference(b);
                return (preferenceA > preferenceB) ? -1 : (preferenceA < preferenceB) ? 1 : 0;
            }
        );

        cachedObject = colliders[0].gameObject;
        cachedTarget = colliders[0].transform.position;
    }

    public float GetPreference(Collider2D collider)
    {
        Vector2 distance = collider.transform.position - transform.position;
        float distanceValue = distanceMultiplier * distance.sqrMagnitude;
        float angleValue = angleMultiplier * Vector2.Dot(distance, facingDirection.Get());

        return GetPreferenceOfTag(collider.tag) * angleValue / distanceValue;
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
}
