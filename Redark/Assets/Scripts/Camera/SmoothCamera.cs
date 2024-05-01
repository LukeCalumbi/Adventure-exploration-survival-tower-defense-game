using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCamera : MonoBehaviour
{
    public Transform target;
    public float speed = 10;

    void Update()
    {
        Vector2 pos = Vector2.Lerp(transform.position, target.transform.position, 1 - Mathf.Exp(-Time.deltaTime * speed));
        transform.position = new Vector3(pos.x, pos.y, transform.position.z);
    }
}
