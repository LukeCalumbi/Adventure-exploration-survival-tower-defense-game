using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TargetingSystem : MonoBehaviour
{
    public abstract Transform GetTarget(List<string> targetTags);
}
