using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsTarget : MonoBehaviour
{
    public TargetingSystem targetingSystem;
    public FacingDirection facingDirection;
    private float baseAngle = 0f;

    void Start()
    {
        baseAngle = transform.rotation.eulerAngles.z;
    }

    void Update()
    {
        Transform target = targetingSystem.GetTarget();

        if (target != null) {
            SetRotation(target.position);
            return;
        }
        
        if (facingDirection != null)
        {
            SetRotation(transform.position + facingDirection.Get() * GridSnapping.TILE_SIZE);
            return;
        }
    }

    void SetRotation(Vector3 position)
    {
        Vector2 distance = (position - transform.position).normalized;
        float angle = Mathf.Acos(distance.x) * Mathf.Rad2Deg;
        float direction = Mathf.Sign(distance.y);
        transform.eulerAngles = new Vector3(0, 0, baseAngle + angle * direction);
    }
}
