using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{   
    [SerializeField] private Transform playerPosition;
    [SerializeField] private float speed = 2, minDistance = 0.5f, distance;
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private SpriteRenderer sprite;
    private Vector2 targetPosition, currentPosition, direction;

    private void FixedUpdate()
    {   
        targetPosition = this.playerPosition.position;
        currentPosition = this.transform.position;
        distance = Vector2.Distance(targetPosition,currentPosition);

        if(distance < this.minDistance) 
        {
            this.rigidBody.velocity = Vector2.zero;
            return;
        }

        if(Math.Abs(targetPosition.x - currentPosition.x) >= Math.Abs(targetPosition.y - currentPosition.y))
        {
            this.transform.position += Vector3.right * (targetPosition.x - currentPosition.x)/(targetPosition.x - currentPosition.x);
        }
        else
        {
            this.transform.position += Vector3.up * Time.fixedDeltaTime * (targetPosition.y - currentPosition.y)/(targetPosition.y - currentPosition.y);
        }


        // direction = targetPosition - currentPosition;
        // direction = direction.normalized;
        // rigidBody.MovePosition(rigidBody.position+direction*speed * Time.fixedDeltaTime);
        
        if(this.rigidBody.velocity.x > 0) this.sprite.flipX = false;
        else this.sprite.flipX = true;
    }
}
