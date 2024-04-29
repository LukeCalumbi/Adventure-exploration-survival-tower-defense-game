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
                TryPlaceSelectedItemFromInventory();
                break;

            default:
                break;
        }
    }

    void Hit()
    {
        selector.TryHit();
    }

    void TryPlaceSelectedItemFromInventory()
    {
        Item item = Inventory.GetSelectedItem();

        if (!Inventory.IsItemPlaceable(item))
            return;

        IronManager.ConsumeIron(item.cost);
        Spawn(item.gameObject);
        Inventory.OnItemPlaced(item);
    }

    GameObject Spawn(GameObject prefab) 
    {
        GameObject spawnedObject = Instantiate(prefab);
        spawnedObject.transform.position = selector.transform.position;
        return spawnedObject;
    }
}
