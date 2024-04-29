using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DisplayCurrentlySelectedItem : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Item item = Inventory.GetSelectedItem();
        spriteRenderer.sprite = (item != null) ? item.image : null;
        spriteRenderer.color = Color.white * (Inventory.IsItemPlaceable(item) ? 0.8f : 0.3f);
    }
}
