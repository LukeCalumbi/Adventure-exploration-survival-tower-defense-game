using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Selector))]
public class PlayerSelectorImage : MonoBehaviour
{
    public Sprite selectorSprite;
    public GridMovement movement;

    SpriteRenderer spriteRenderer;
    Selector selector;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        selector = GetComponent<Selector>();
    }

    void LateUpdate()
    {
        GameObject selectedObject = selector.GetSelectedObject();
        Item selectedItem = Inventory.GetSelectedItem();
        bool isItemPlaceable = Inventory.IsItemPlaceable(selectedItem);

        spriteRenderer.sprite = selectedObject != null ? selectorSprite : selectedItem.image;

        if (selectedObject != null) {
            Interactable interactable = selectedObject.GetComponent<Interactable>();
            spriteRenderer.color = Color.white * (interactable != null && (interactable.InteractsWith(tag) || interactable.GetsHitBy(tag)) ? 1f : 0f);
            return;
        }

        spriteRenderer.color = Color.white * (isItemPlaceable ? 0.8f : 0.35f);
    }
}
