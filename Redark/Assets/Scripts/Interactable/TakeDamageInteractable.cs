using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
[RequireComponent(typeof(Health))]
public class ArtifactInteracable : MonoBehaviour
{
    public List<string> authorizedTags = new List<string>();
    public int defaultDamage = 1;
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
        if (!authorizedTags.Contains(selector.gameObject.tag))
            return;

        Damage damage = selector.gameObject.GetComponent<Damage>();
        health.DoDamage(damage != null ? damage.GetDamage() : defaultDamage);
    }
}
