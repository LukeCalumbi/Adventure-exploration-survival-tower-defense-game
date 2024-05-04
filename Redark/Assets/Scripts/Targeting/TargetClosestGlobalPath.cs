using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetClosestGlobalPath : TargetingSystem
{
    public TargetingSystem support;
    public float maxDistanceToVertex = 8f;
    GraphPath cachedPath = new GraphPath();

    public override void UpdateTarget()
    {
        Vector3? target = support.GetTarget();
        if (target == null)
        {
            cachedPath.Invalidate();
            cachedTarget = null;
            cachedObject = null;
            return;
        }

        cachedObject = support.GetTargetObject();
        cachedPath = cachedPath.IsValid() ? cachedPath : ZombieGlobalPath.PathTo(transform.position, target.Value);
        if (!cachedPath.IsValid())
        {
            cachedPath.Invalidate();
            cachedTarget = null;
            cachedObject = null;
            return;
        }

        Debug.Log(String.Format("cached path is {0}", cachedPath));

        KeyValuePair<int, Vector3> closestThis = ZombieGlobalPath.ClosestVertexTo(transform.position);
        KeyValuePair<int, Vector3> closestTarget = ZombieGlobalPath.ClosestVertexTo(target.Value);

        int end = cachedPath.GetLast();
        if (end != closestTarget.Key)
        {
            cachedPath.Invalidate();
            cachedTarget = null;
            cachedObject = null;
            return;
        }

        int start = cachedPath.GetFirst();

        if (start != closestThis.Key || Vector3.Distance(closestThis.Value, transform.position) > maxDistanceToVertex)
        {
            cachedTarget = ZombieGlobalPath.worldPaths.GetVertex(start);
            return;
        }
        
        cachedPath.RemoveStart(ZombieGlobalPath.worldPaths);
        return;
    }
}
