using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamageArtifact : InteractableFunction
{
    Artifact artifact;

    public override void Initialize()
    {
        artifact = GetComponent<Artifact>();
    }

    public override void Action(Selector selector)
    {
        artifact.DoDamage();
    }
}