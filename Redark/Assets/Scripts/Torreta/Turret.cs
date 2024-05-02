using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject bullet;
    public List<string> targetTags = new List<string>();
    public TargetingSystem targetingSystem;
    public float cooldownTime = 0.5f;

    Timer cooldown;
    Transform target = null;

    void Start()
    {
        cooldown = new Timer(cooldownTime);
        cooldown.Start();
    }
    void FixedUpdate()
    {
        if (cooldown.IsRunning())
        {
            cooldown.Update(Time.fixedDeltaTime);
            return;
        }

        if (target == null)
        {
            target = targetingSystem.GetTarget(targetTags);
            return;
        }

        Shoot();
        target = null;
        cooldown.Start();
    }

    void Shoot()
    {
        GameObject gameObject = Instantiate(bullet, transform.position, Quaternion.identity);
        MoveTowardsDirection moveTowardsDirection = gameObject.GetComponent<MoveTowardsDirection>();

        if (moveTowardsDirection != null)
            moveTowardsDirection.SetDirection(target.transform.position - transform.position);
    }
}
