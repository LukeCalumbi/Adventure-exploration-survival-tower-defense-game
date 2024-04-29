using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class DestroyOnDeath : MonoBehaviour
{
    Health health;
    void Start()
    {
        health = GetComponent<Health>();
    }

    void LateUpdate()
    {
        if (health.IsDead())
            Destroy(this.gameObject);
    }
}
