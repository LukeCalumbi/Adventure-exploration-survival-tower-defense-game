using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TargetingSystem : MonoBehaviour
{
    [SerializeField] public List<Target> targetTags = new List<Target>();
    
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

    public bool HasTargetTag(string tag)
    {
        foreach (Target target in targetTags)
        {
            if (tag == target.tag)
                return true;
        }

        return false;
    }

    public float GetPreferenceOfTag(string tag)
    {
        foreach (Target target in targetTags)
        {
            if (tag == target.tag)
                return target.preference;
        }

        return -1f;
    }

    public virtual void UpdateTarget() 
    {

    }
}

[Serializable]
public class Target
{
    [SerializeField] public string tag;
    [SerializeField] public float preference;
}
