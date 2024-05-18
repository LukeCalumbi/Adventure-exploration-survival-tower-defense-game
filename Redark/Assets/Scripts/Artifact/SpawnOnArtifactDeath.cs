using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnArtifactDeath : MonoBehaviour
{
    public GameObject prefab;

    private bool died = false;

    private void LateUpdate()
    {
        if (died ^ Artifact.ArtifactDied())
        {
            Spawn();
            died = Artifact.ArtifactDied();
        }
    }

    private void Spawn()
    {
        Instantiate(prefab);
    }
}
