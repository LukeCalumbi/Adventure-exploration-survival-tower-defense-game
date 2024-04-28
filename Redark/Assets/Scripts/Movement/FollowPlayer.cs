using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(GridMovement))]
public class FollowPlayer : MonoBehaviour
{   
    [SerializeField] private GridMovement movement;
    [SerializeField] private Transform target, lastTargetPosition;
    [SerializeField] private float minDistance = 0.5f,radius = 5, distance;
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private SpriteRenderer sprite;
    Vector2 currentPosition, targetPosition, direction;
    float distAtX, distAtY, directionX,directionY;
    RaycastHit2D rayInfoX, rayInfoY;
    
    void Start()
    {
        movement = GetComponent<GridMovement>();
    }

    public void FixedUpdate()
    {   
        SearchPlayer();
        
        if(this.target == null){
            //StopMovement();
            return;
        }

        Move2();
    }

    void Move()
    {   
        this.distAtX = this.transform.position.x - target.position.x;
        this.distAtY = this.transform.position.y - target.position.y;
        this.distance = (float) Math.Sqrt(distAtX*distAtX + distAtY*distAtY);

        if(distance < minDistance) {
            //StopMovement();
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

        InvertSprite();
    }

    void Move2()
    {
        movement.MoveTowardsDirection(target.position - this.transform.position);
    }
    void MoveinAxisX(){
        movement.MoveTowardsDirection(directionX * Vector3.right);
    }
    void MoveinAxisY(){
        movement.MoveTowardsDirection(directionY * Vector3.up);
    }

    private void SearchPlayer()
    {  
        List<Collider2D> colliders = new List<Collider2D>(Physics2D.OverlapCircleAll(this.transform.position, this.radius));

        if(colliders.Count < 2){
            this.target = null;
            //Debug.Log("colliders.Count < 2");
            return;
        }
        
        colliders.Sort(delegate (Collider2D A,Collider2D B) {
            float distA = (this.transform.position - A.transform.position).sqrMagnitude;
            float distB = (this.transform.position - B.transform.position).sqrMagnitude; 
            return distA < distB ? -1 : distA > distB ? 1 : 0; 
        });

        Collider2D collider = colliders[1];

        if(!collider.CompareTag("Friendly")) {
            this.target = null;
            //Debug.Log("!collider.CompareTag Friendly");
            return;
        }

        this.target = collider.transform;
        //Debug.Log("Só maravilha irmao");
    }

    void InvertSprite()
    {   
        if(movement.IsIdle()) return;

        sprite.flipX = movement.GetMovementDirection().x < 0;
        sprite.flipY = movement.GetMovementDirection().y < 0;
    }

    private void OnDrawGizmos() // desenha circulo de visao
    {
        Gizmos.DrawWireSphere(this.transform.position,radius);    
        if(this.target != null) Gizmos.DrawLine(this.transform.position,target.transform.position);
    }

}
