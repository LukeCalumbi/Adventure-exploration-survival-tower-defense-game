using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

[RequireComponent(typeof(Selector))]
public class PlayerSelector : MonoBehaviour
{
    const KeyCode INTERACT_KEY = KeyCode.Space;
    const KeyCode HIT_KEY = KeyCode.E;

    public GameObject turret;
    Selector selector;

    void Start()
    {
        selector = GetComponent<Selector>();
    }

    void Update()
    {
        if (Input.GetKeyDown(INTERACT_KEY))
        {
            Interact();
            return;
        }

        if (Input.GetKeyDown(HIT_KEY))
            Hit();
    }

    void Interact()
    {
        switch (selector.TryInteract())
        {
            case InteractError.NoObjectSelected: 
                PlaceTurret();
                break;

            default:
                break;
        }
    }

    void Hit()
    {
        switch (selector.TryHit())
        {
            case InteractError.NoObjectSelected: 
                Debug.Log("Player's hit");
                break;

            default:
                break;
        }
    }

    void PlaceTurret()
    {
        Spawn(turret);
    }

    void RemoveSelectedObject()
    {
        GameObject selectedObject = selector.GetSelectedObject();
        if (selectedObject == null)
            return;

        Destroy(selectedObject);
    }

    GameObject Spawn(GameObject prefab) 
    {
        GameObject spawnedObject = Instantiate(prefab);
        spawnedObject.transform.position = selector.transform.position;
        return spawnedObject;
    }
}
