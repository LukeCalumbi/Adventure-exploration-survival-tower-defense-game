using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnInteract : InteractableFunction
{
    public override void Action(Selector selector)
    {
        Destroy(this.gameObject);
    }
}
