using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DestroyOnDestroy : MonoBehaviour
{
    public GameObject daddy;

    void LateUpdate()
    {
        if (daddy.IsDestroyed())
            Destroy(this.gameObject);
    }
}
