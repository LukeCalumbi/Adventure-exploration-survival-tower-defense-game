using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovPlayer : MonoBehaviour
{   
    [SerializeField] private float speed = 5f;
    [SerializeField] private Rigidbody2D playerRb;
    Vector2 moviment;
    void Start()
    {
        
    }

    void Update()
    {
        moviment.x = Input.GetAxisRaw("Horizontal");
        moviment.y = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {   
        moviment = moviment.normalized;
        playerRb.MovePosition(playerRb.position+moviment*speed * Time.fixedDeltaTime);
    }
}
