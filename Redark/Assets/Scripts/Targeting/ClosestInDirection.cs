using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(FacingDirection))]
public class ClosestInDirection : TargetingSystem
{
    public float detectionRadiusInTiles = 5f;
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
            (Collider2D collider) => collider.gameObject != this.gameObject && HasTargetTag(collider.tag) && GetPreference(collider) > float.Epsilon
        ));

        if (colliders.Count == 0)
            return;

        colliders.Sort(
            delegate (Collider2D a, Collider2D b) {
                float preferenceA = GetPreference(a);
                float preferenceB = GetPreference(b);
                return (preferenceA > preferenceB) ? -1 : (preferenceA < preferenceB) ? 1 : 0;
            }
        );

        cachedTarget = colliders[0].transform;
    }

    public float GetPreference(Collider2D collider)
    {
        Vector2 distance = collider.transform.position - transform.position;
        float distanceValue = distanceMultiplier * distance.sqrMagnitude;
        float angleValue = angleMultiplier * Vector2.Dot(distance, facingDirection.Get());

        return GetPreferenceOfTag(collider.tag) * angleValue / distanceValue;
    }
}
