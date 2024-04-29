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
    GridMovement movement;
    Vector3 target = Vector3.zero;
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

    public void Target(Vector3 target)
    {
        this.target = target;
        isTargeting = true;
    }

    public void Stop()
    {
        isTargeting = false;
    }

    public bool IsTargeting()
    {
        return isTargeting;
    }

    public Vector3 GetTargetDirection()
    {
        return target - this.transform.position;
    }

    private void OnDrawGizmos() // desenha linha ate target
    {
        if(this.target != null) 
            Gizmos.DrawLine(this.transform.position,target);
    }
}
