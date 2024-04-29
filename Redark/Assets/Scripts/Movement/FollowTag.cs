using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(FollowTarget))]
public class FollowTag : MonoBehaviour
{   
    public string targetTag = "Friendly";
    public float radius = 5;
    public float minDistance = 0.5f;
    FollowTarget follow;

    void Start()
    {
        follow = GetComponent<FollowTarget>();
    }

    public void FixedUpdate()
    {   
        SearchPlayer();
    }

    private void SearchPlayer()
    {  
        List<Collider2D> colliders = new List<Collider2D>(Physics2D.OverlapCircleAll(this.transform.position, this.radius));

        colliders = new List<Collider2D>(colliders.Where((Collider2D collider) =>
            collider.gameObject != this.gameObject && 
            collider.CompareTag(targetTag) &&
            (collider.transform.position - this.transform.position).sqrMagnitude >= Mathf.Pow(minDistance, 2)
        ));

        if (colliders.Count == 0)
            return;
        
        colliders.Sort(delegate (Collider2D A,Collider2D B) {
            float distA = (this.transform.position - A.transform.position).sqrMagnitude;
            float distB = (this.transform.position - B.transform.position).sqrMagnitude; 
            return distA < distB ? -1 : distA > distB ? 1 : 0; 
        });

        Collider2D chosenCollider = colliders[0];
        follow.Target(chosenCollider.transform.position);
    }

    private void OnDrawGizmos() // desenha circulo de visao
    {
        Gizmos.DrawWireSphere(this.transform.position,radius);    
    }

}
