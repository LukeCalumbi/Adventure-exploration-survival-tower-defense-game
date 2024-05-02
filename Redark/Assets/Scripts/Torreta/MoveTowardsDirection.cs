using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MoveTowardsDirection : MonoBehaviour
{
    public Vector3 initialDirection;
    // Start is called before the first frame update
    public float speed = 15f;
    Rigidbody2D rigidBody;

    // Start is called before the first frame update
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();

        SetDirection(initialDirection);
    }

    public void SetDirection(Vector3 direction)
    {
        direction = direction.normalized;
        rigidBody.velocity = direction * speed;
    }
}
