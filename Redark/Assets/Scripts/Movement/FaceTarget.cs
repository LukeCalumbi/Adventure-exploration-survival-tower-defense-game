using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FollowTarget))]
[RequireComponent(typeof(FacingDirection))]
public class FaceTarget : MonoBehaviour
{
    FollowTarget follow;
    FacingDirection facingDirection;

    void Start()
    {
        follow = GetComponent<FollowTarget>();
        facingDirection = GetComponent<FacingDirection>();
    }

    void Update()
    {
        if (follow.IsTargeting())
            facingDirection.Set(follow.GetTargetDirection());
    }
}
