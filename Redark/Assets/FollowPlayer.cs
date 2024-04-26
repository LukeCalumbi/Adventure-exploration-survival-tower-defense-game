using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{   
    [SerializeField] private Transform target;
    [SerializeField] private float speed = 2, minDistance = 0.5f,radius = 5, distance;
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private SpriteRenderer sprite;

    public void FixedUpdate()
    {   
        SearchPlayer();

        if(this.target == null)
        {
            StopMovement();
            return;
        }

        Move();
    }

    void StopMovement(){
        rigidBody.velocity = Vector2.zero;
    }

    void Move()
    {   
        float distAtX, distAtY, directionX,directionY;
        distAtX = this.transform.position.x - target.position.x;
        distAtY = this.transform.position.y - target.position.y;
        distance = (float) Math.Sqrt(distAtX*distAtX + distAtY*distAtY);

        if(distance < minDistance) 
        {
            StopMovement();
            return;
        }

        directionX = -distAtX/Math.Abs(distAtX);
        directionY = -distAtY/Math.Abs(distAtY);

        if(Math.Abs(distAtX) >= Math.Abs(distAtY)) 
            this.transform.position += Vector3.right * speed * directionX * Time.fixedDeltaTime; 
        else
            this.transform.position += Vector3.up * speed * directionY * Time.fixedDeltaTime;

        InvertSpriteInX();
    }

    private void SearchPlayer()
    {
        Collider2D collider = Physics2D.OverlapCircle(this.transform.position,this.radius);

        if(collider == null) {
            this.target = null;
            return;
        }

        Vector2 currentPosition = this.transform.position;
        Vector2 targetPosition = collider.transform.position;
        Vector2 direction = (targetPosition - currentPosition).normalized;

        RaycastHit2D hit = Physics2D.Raycast(currentPosition,direction);
            
        if(hit.transform == null){
            this.target = null;
            return;
        }

        if(!hit.transform.CompareTag("Friendly")){
            this.target = null;
            return;
        }

        this.target = hit.transform;
    }

    void InvertSpriteInX()
    {
        if(rigidBody.velocity.x > 0) sprite.flipX = false;
        else sprite.flipX = true;
    }

    private void OnDrawGizmos() // desenha circulo de visao
    {
        Gizmos.DrawWireSphere(this.transform.position,radius);    
        if(this.target != null) Gizmos.DrawLine(this.transform.position,target.transform.position);
    }

}
