using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingSystemCascade : TargetingSystem
{
    public List<TargetingSystem> systems;

    public override void UpdateTarget()
    {
        foreach (TargetingSystem system in systems)
        {
            if (system.GetTarget() != null)
            {
                cachedTarget = system.GetTarget();
                cachedObject = system.GetTargetObject();
                return;
            }
        }

        cachedTarget = null;
        cachedObject = null;
        return;
    }
}
