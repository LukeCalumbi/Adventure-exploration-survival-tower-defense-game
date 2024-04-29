using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class InstantiateInteractable : MonoBehaviour
{
    public List<string> authorizedTags;
    public GameObject prefab;
    Interactable interactable;

    void Start()
    {
        interactable = GetComponent<Interactable>();
        interactable.AddOnInteractCallback(Instantiate);
    }

    void Instantiate(Selector selector)
    {
        if (authorizedTags.Contains(selector.gameObject.tag))
            Instantiate(prefab);
    }
}
