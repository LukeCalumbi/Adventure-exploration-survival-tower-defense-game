using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FollowTarget))]
[RequireComponent(typeof(KnockdownCounter))]
public class ZombieMovement : MonoBehaviour
{
    FollowTarget followTarget;
    KnockdownCounter knockdownCounter;

    void Start()
    {
        followTarget = GetComponent<FollowTarget>();
        knockdownCounter = GetComponent<KnockdownCounter>();
    }

    // Update is called once per frame
    void Update()
    {
        if (knockdownCounter.IsKnockedDown())
        {
            print("gay");
            followTarget.StopFollowing();
            return;
        }

        followTarget.StartFollowing();
    }
}
