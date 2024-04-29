using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
[RequireComponent(typeof(Health))]
public class ArtifactInteracable : MonoBehaviour
{
    Interactable interactable;
    Health health;

    void Start()
    {
        interactable = GetComponent<Interactable>();
        health = GetComponent<Health>();

        interactable.AddOnHitCallback(TakeDamage);
    }

    void TakeDamage(Selector selector)
    {
        Damage damage = selector.gameObject.GetComponent<Damage>();
        if (damage == null)
            return;

        health.DoDamage(damage.GetDamage());
    }
}
