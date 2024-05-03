using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingSystem : MonoBehaviour
{
    public List<string> targetTags = new List<string>();
    
    protected Transform cachedTarget = null;
    
    bool updatedCache = false;

    public void LateUpdate()
    {
        cachedTarget = null;
        updatedCache = false;
    }

    public Transform GetTarget()
    {
        if (!updatedCache)
        {
            UpdateTarget();
            updatedCache = true;
        }

        return cachedTarget;
    }

    public virtual void UpdateTarget() 
    {

    }
}
