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

public class Selector : MonoBehaviour
{
    public List<string> authorizedTags = new List<string>();
    public float detectionTiles = 1.0f;
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
        List<Collider2D> colliders = new List<Collider2D>(Physics2D.OverlapBoxAll(transform.position, Vector2.one * (detectionTiles * GridSnapping.TILE_SIZE - 0.1f), 0f).Where(
            (Collider2D collider) => authorizedTags.Contains(collider.gameObject.tag)
        ));
        
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
        return Physics2D.OverlapBox(transform.position, Vector2.one * (detectionTiles * GridSnapping.TILE_SIZE - 0.1f), 0f) != null;
    }
}
