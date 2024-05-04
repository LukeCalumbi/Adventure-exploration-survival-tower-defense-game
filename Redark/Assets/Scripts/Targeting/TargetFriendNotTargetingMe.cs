using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetFriendNotTargetingMe : TargetingSystem
{
    public TargetingSystem support;
    public List<GameObject> ignore = new List<GameObject>();
    
    public override void UpdateTarget()
    {
        GameObject target = support.GetTargetObject();
        if (target == null || ignore.Contains(target))
        {
            cachedObject = null;
            cachedTarget = null;
            return;
        }

        List<TargetFriendNotTargetingMe> systems = target.GetComponents<TargetFriendNotTargetingMe>().ToList();
        if (systems.Any(delegate (TargetFriendNotTargetingMe system) 
            { 
                system.ignore.AddRange(ignore);
                system.ignore.Add(this.gameObject);
                return system.GetTargetObject() == this.gameObject; 
            }
        ))
        {
            cachedObject = null;
            cachedTarget = null;
            return;
        }

        cachedObject = target;
        cachedTarget = target.transform.position;
        ignore = new List<GameObject>();
    }
}
