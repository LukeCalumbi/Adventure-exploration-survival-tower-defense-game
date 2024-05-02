using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

[RequireComponent(typeof(Selector))]
public class BulletSelector : MonoBehaviour
{
    Selector selector;
    GameObject last;

    void Start()
    {
        selector = GetComponent<Selector>();
    }

    void FixedUpdate()
    {
        GameObject now = selector.GetSelectedObject();

        if (last == now)
            return;

        selector.TryHit();
        last = now;
    }
}
