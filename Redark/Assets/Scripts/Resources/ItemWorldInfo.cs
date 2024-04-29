using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemWorldInfo
{
    [SerializeField] public int amountPlaced;

    public ItemWorldInfo()
    {
        amountPlaced = 0;
    }

    public ItemWorldInfo(int amountPlaced)
    {
        this.amountPlaced = amountPlaced;
    }
}
