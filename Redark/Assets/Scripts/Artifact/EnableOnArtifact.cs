using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableOnArtifact : MonoBehaviour
{
    public List<MonoBehaviour> components;
    public bool enableWhenPlaced = true;

    private void Update()
    {
        foreach (var component in components)
            component.enabled = enableWhenPlaced == Artifact.IsPlaced();
    }
}
