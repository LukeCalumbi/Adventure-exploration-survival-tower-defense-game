using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableWhenCloseToTag : MonoBehaviour
{
    public MonoBehaviour toEnable;
    public string targetTag = "Player";
    public float distanceToEnable = 15f;

    GameObject objectTag;

    void Start()
    {
        objectTag = GameObject.FindGameObjectWithTag(targetTag);
    }

    void Update()
    {
        toEnable.enabled = Vector3.Distance(objectTag.transform.position, transform.position) <= distanceToEnable;
    }
}
