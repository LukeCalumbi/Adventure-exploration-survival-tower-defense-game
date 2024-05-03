using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
[RequireComponent(typeof(Health))]
public class TakeDamageInteracable : InteractableFunction
{
    public int defaultDamage = 1;
    Health health;

    public override void Initialize()
    {
        health = GetComponent<Health>();
    }

    public override void Action(Selector selector)
    {
        Damage damage = selector.gameObject.GetComponent<Damage>();
        health.DoDamage(damage != null ? damage.damage : defaultDamage);
    }
}
