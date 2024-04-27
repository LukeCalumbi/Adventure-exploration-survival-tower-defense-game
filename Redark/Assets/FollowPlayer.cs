using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{   
    [SerializeField] private Transform target, lastTargetPosition;
    [SerializeField] private float speed = 2, minDistance = 0.5f,radius = 5, distance;
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private SpriteRenderer sprite;
    Vector2 currentPosition, targetPosition, direction;
    float distAtX, distAtY, directionX,directionY;
    RaycastHit2D rayInfoX, rayInfoY;

    public void FixedUpdate()
    {   
        SearchPlayer();
        
        if(this.target == null){
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
        this.distAtX = this.transform.position.x - target.position.x;
        this.distAtY = this.transform.position.y - target.position.y;
        this.distance = (float) Math.Sqrt(distAtX*distAtX + distAtY*distAtY);

        if(distance < minDistance) {
            StopMovement();
            return;
        }
        
        this.rayInfoX = Physics2D.Raycast(Vector2.right * currentPosition.x, Vector2.right * direction.x);
        this.rayInfoY = Physics2D.Raycast(Vector2.up * currentPosition.y, Vector2.up * direction.y);
        
        this.directionX = -distAtX/Math.Abs(distAtX);
        this.directionY = -distAtY/Math.Abs(distAtY);
        
        if(rayInfoX.collider != null && rayInfoX.transform.CompareTag("Friendly")) MoveinAxisX();
        else if(rayInfoY.collider != null && rayInfoY.transform.CompareTag("Friendly")) MoveinAxisY();
        else if(Math.Abs(distAtX) >= Math.Abs(distAtY)) MoveinAxisX();    
        else MoveinAxisY();

        InvertSpriteInX();
    }
    void MoveinAxisX(){
        this.transform.position += directionX * speed * Time.fixedDeltaTime * Vector3.right;
    }
    void MoveinAxisY(){
        this.transform.position += directionY * speed * Time.fixedDeltaTime * Vector3.up;
    }

    private void SearchPlayer()
    {  
        List<Collider2D> colliders = new List<Collider2D>(Physics2D.OverlapCircleAll(this.transform.position, this.radius));

        if(colliders.Count == 0){
            this.target = null;
            return;
        }
        
        colliders.Sort(delegate (Collider2D A,Collider2D B) {
            float distA = (this.transform.position - A.transform.position).sqrMagnitude;
            float distB = (this.transform.position - B.transform.position).sqrMagnitude; 
            return distA < distB ? -1 : distA > distB ? 1 : 0; 
        });

        Collider2D collider = colliders[0];

        if(collider == null || collider == this.gameObject) {
            this.target = null;
            Debug.Log("Executando2");
            return;
        }

        this.currentPosition = this.transform.position;
        this.targetPosition = collider.transform.position;
        this.direction = (targetPosition - currentPosition).normalized;

        RaycastHit2D hit = Physics2D.Raycast(this.currentPosition,this.direction);
        
        if(hit.collider == null) {
            Debug.Log("è mano");
            return;
        }
        if(!hit.collider.CompareTag("Friendly")){
            this.target = null;
            Debug.Log("Executando1");
            return;
        }

        this.target = hit.transform;
        Debug.Log("Executando");
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
