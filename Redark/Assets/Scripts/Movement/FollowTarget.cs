using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(GridMovement))]
public class FollowTarget : MonoBehaviour
{   
    public TargetingSystem targetingSystem;

    GridMovement movement;
    bool isTargeting = false;

    void Start()
    {
        movement = GetComponent<GridMovement>();
        isTargeting = false;
    }

    public void FixedUpdate()
    {   
        if (!isTargeting)
            return;

        Move();
    }

    void Move()
    {
        movement.MoveTowardsDirection(GetTargetDirection());
    }

    public void StartFollowing()
    {
        isTargeting = true;
    }

    public void StopFollowing()
    {
        isTargeting = false;
    }

    public bool IsTargeting()
    {
        return isTargeting;
    }

    public Vector3 GetTargetDirection()
    {
        if (targetingSystem.GetTarget() == null)
            return Vector3.zero;

        return targetingSystem.GetTarget().Value - this.transform.position;
    }

    private void OnDrawGizmos() // desenha linha ate target
    {
        return;
        
        if (targetingSystem == null)
            return;

        if(targetingSystem.GetTarget() != null) 
            Gizmos.DrawLine(this.transform.position, targetingSystem.GetTarget().Value);
    }
}
