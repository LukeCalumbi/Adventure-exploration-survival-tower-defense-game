using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TimedSpawner : MonoBehaviour
{
    public GameObject prefab;
    public Timer spawnTimer;

    void Start()
    {
        spawnTimer.Start();
    }

    void FixedUpdate()
    {
        if (spawnTimer.Finished())
        {
            TrySpawn();
            return;
        }

        spawnTimer.Update(Time.fixedDeltaTime);
    }

    bool TrySpawn()
    {
        if (Physics2D.OverlapCircleAll(transform.position, GridSnapping.TILE_SIZE * 0.4f).Any((Collider2D collider) => collider.gameObject != this.gameObject))
            return false;

        Spawn();
        return true;
    }

    void Spawn()
    {
        GameObject spawnedObject = Instantiate(prefab);
        spawnedObject.transform.position = transform.position;
        spawnTimer.Start();
    }
}
