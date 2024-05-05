using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUpgrade : InteractableFunction
{
    public List<Item> itemsReceived = new List<Item>();

    public override void Action(Selector selector)
    {
        foreach (Item item in itemsReceived)
            Inventory.RegisterNewItem(item);
    }
}
