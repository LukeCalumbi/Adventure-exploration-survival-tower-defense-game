using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public enum InteractError
{
    NoError,
    NoInteractComponent,
    NoObjectSelected
}

[RequireComponent(typeof(GridSnapping))]
public class Selector : MonoBehaviour
{
    GridSnapping snap;
    void Start()
    {
        snap = GetComponent<GridSnapping>();
        snap.EnableSnapping();
    }

    public InteractError TryInteract()
    {
        GameObject gameObject = GetSelectedObject();

        if (gameObject == null)
            return InteractError.NoObjectSelected;
        
        Interactable interactable = gameObject.GetComponent<Interactable>();
        if (interactable == null)
            return InteractError.NoInteractComponent;

        interactable.Interact(this);
        return InteractError.NoError;
    }

    public InteractError TryHit()
    {
        GameObject gameObject = GetSelectedObject();

        if (gameObject == null)
            return InteractError.NoObjectSelected;
        
        Interactable interactable = gameObject.GetComponent<Interactable>();
        if (interactable == null)
            return InteractError.NoInteractComponent;

        interactable.Hit(this);
        return InteractError.NoError;
    }

    public GameObject GetSelectedObject()
    {
        List<Collider2D> colliders = new List<Collider2D>(Physics2D.OverlapBoxAll(transform.position, Vector2.one * (GridSnapping.TILE_SIZE - 0.1f), 0f));
        
        if (colliders.Count == 0)
            return null;

        Func<Collider2D, Collider2D, int> sortFunction = (Collider2D a, Collider2D b) => {
            float za = a.transform.position.z;
            float zb = b.transform.position.z;
            return za > zb ? -1 : za < zb ? 1 : 0;
        };

        colliders.Sort((Collider2D a, Collider2D b) => sortFunction(a, b));
        return colliders[0].gameObject;
    }

    public bool IsSelectingSomething()
    {
        return Physics2D.OverlapBox(transform.position, Vector2.one * (GridSnapping.TILE_SIZE - 0.1f), 0f) != null;
    }
}
