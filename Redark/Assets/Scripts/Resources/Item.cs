using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item
{
    [SerializeField] public string name = "";
    [SerializeField] public Sprite image;
    [SerializeField] public GameObject gameObject;
    [SerializeField] public int cost;
    [SerializeField] public bool isUnique;
}
