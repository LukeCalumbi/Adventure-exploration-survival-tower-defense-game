using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnKeyPress : MonoBehaviour
{
    public GameObject prefab;
    public KeyCode key;
    
    Timer cooldown = new Timer(0.5f);

    void Start()
    {
        cooldown.ForceEnd();
    }

    void Update()
    {
        if (GameState.IsGameplayPaused())
        {
            cooldown.Start();
            return;
        }

        if (Input.GetKeyDown(key) && cooldown.Finished())
            Spawn();

        cooldown.Update(Time.deltaTime);
    }

    void Spawn()
    {
        Instantiate(prefab);
    }
}
