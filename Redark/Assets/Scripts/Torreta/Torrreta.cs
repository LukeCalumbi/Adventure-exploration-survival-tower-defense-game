using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torreta : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    float bulletCountdown = 0;

    bool ZombieOnArea = false;
    public static Vector3 posZombie;

    public static Vector3 TorretaDirection = Vector3.down; //direção de atirar
    private float angle;
    void Start(){
        
    }
    void Update(){
        bulletCountdown += Time.deltaTime;
        
        print(Mathf.RoundToInt(bulletCountdown));
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Zombie")) 
        {
        
            ZombieOnArea = true;
            posZombie = (collision.transform.position - transform.position).normalized;
            //angle = Mathf.Atan2(posZombie.y, posZombie.x) * Mathf.Rad2Deg;
            //transform.rotation = Quaternion.Euler(0, 0, angle);

            if (( Mathf.RoundToInt(bulletCountdown)>=1) && ZombieOnArea){
            Shoot();
            bulletCountdown = 0;
            }
        } 
    }
    private void OnTriggerExit2D(Collider2D collision){

        if (collision.CompareTag("Zombie")){
            ZombieOnArea = false;
            //transform.rotation = Quaternion.Euler(0, 0, 0); retona a torreta a rotação inicial
        }
    }
    void Shoot()
    {
        Instantiate(bullet, gameObject.transform.position, Quaternion.identity);
    }
}
