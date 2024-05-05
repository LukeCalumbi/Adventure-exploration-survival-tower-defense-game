using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressKeyToDestroy : MonoBehaviour
{
    public List<KeyCode> keys;

    void FixedUpdate()
    {
        foreach (KeyCode key in keys)
        {
            if (Input.GetKeyDown(key))
            {
                Destroy(this.gameObject);
                return;
            }
        }
    }
}
