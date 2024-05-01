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
        bool isSelectingSomething = selector.IsSelectingSomething();
        Item selectedItem = Inventory.GetSelectedItem();
        bool isItemPlaceable = Inventory.IsItemPlaceable(selectedItem);

        spriteRenderer.sprite = isSelectingSomething ? selectorSprite : selectedItem.image;
        spriteRenderer.color = Color.white * (isSelectingSomething ? 1.0f : isItemPlaceable ? 0.8f : 0.2f); 
    }
}
