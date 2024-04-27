using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridMovement))]
public class PlayerMovement : MonoBehaviour
{
    GridMovement movement;

    void Start()
    {
        movement = GetComponent<GridMovement>();
    }

    void Update()
    {
        Vector3 direction = Vector3.zero;

        Func<KeyCode, int> keyStrength = (key) => Input.GetKey(key) ? 1 : 0;
        Func<KeyCode, KeyCode, int> GetAxis = (negative, positive) => keyStrength(positive) - keyStrength(negative);
        
        direction.x = GetAxis(KeyCode.A, KeyCode.D);
        direction.y = GetAxis(KeyCode.S, KeyCode.W);
        movement.MoveTowardsDirection(direction);
    }
}
