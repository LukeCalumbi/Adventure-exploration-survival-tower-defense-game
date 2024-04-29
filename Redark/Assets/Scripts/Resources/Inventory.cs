using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    const KeyCode SELECT_KEY = KeyCode.LeftShift;

    [SerializeField] public List<Item> items;

    static Dictionary<Item, ItemWorldInfo> itemInfo = new Dictionary<Item, ItemWorldInfo>();
    static int selected = 0;

    void Start()
    {
        foreach (Item item in items)
            RegisterNewItem(item);
    }

    void Update()
    {
        if (Input.GetKeyDown(SELECT_KEY))
            selected = (selected + 1) % itemInfo.Count;
    }

    public static void OnItemPlaced(Item item)
    {
        if (!itemInfo.ContainsKey(item))
            return;

        itemInfo[item].amountPlaced += 1;
    }

    public static void OnItemRemoved(Item item)
    {
        if (!itemInfo.ContainsKey(item))
            return;

        itemInfo[item].amountPlaced -= 1;
    }

    public static void RegisterNewItem(Item item)
    {
        if (itemInfo.ContainsKey(item))
            return;

        itemInfo[item] = new ItemWorldInfo();
    }

    public bool IsSelectedItemPlaceable()
    {
        return IsItemPlaceable(GetSelectedItem());
    }

    public static Item GetSelectedItem()
    {
        List<Item> items = new List<Item>(itemInfo.Keys);
        return items[selected];
    }

    public static bool IsItemPlaceable(Item item)
    {
        if (!itemInfo.ContainsKey(item))
            return false;

        ItemWorldInfo info = itemInfo[item];
        Debug.Log(info.amountPlaced);
        return IronManager.HasAtLeast(item.cost) && !(item.isUnique && info.amountPlaced != 0);
    }

    public static List<Item> GetPlaceableItems()
    {
        List<Item> items = new List<Item>();

        foreach (KeyValuePair<Item, ItemWorldInfo> pair in itemInfo)
        {
            if (IronManager.HasAtLeast(pair.Key.cost) && !(pair.Key.isUnique && pair.Value.amountPlaced != 0))
                items.Add(pair.Key);
        }

        return items;
    }
}
