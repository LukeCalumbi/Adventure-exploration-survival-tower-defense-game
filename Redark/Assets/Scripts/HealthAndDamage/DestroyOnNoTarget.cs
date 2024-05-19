using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnNoTarget : MonoBehaviour
{
    public TargetingSystem TargetingSystem;

    public void Update()
    {
        if (TargetingSystem.GetTarget() == null)
            Destroy(gameObject);
    }
}
