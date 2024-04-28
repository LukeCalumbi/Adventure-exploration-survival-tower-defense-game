using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Selector))]
public class PlayerSelector : MonoBehaviour
{
    public GameObject turret;
    Selector selector;

    void Start()
    {
        selector = GetComponent<Selector>();
    }

    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Space))
            return;

        if (selector.IsSelectingSomething())
            RemoveSelectedObject();

        else
            PlaceTurret();
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
