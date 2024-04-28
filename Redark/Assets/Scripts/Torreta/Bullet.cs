using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    

    private Vector3 direction = Torreta.TorretaDirection;
    // Start is called before the first frame update
    private float speed = 15f;
    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = direction * speed;
    }
    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.CompareTag("Zombie")){
            Destroy(gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision){
        if (collision.CompareTag("Torreta")){
            Destroy(gameObject);
        }
    }

}
