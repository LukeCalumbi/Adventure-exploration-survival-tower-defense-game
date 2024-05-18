using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artifact : MonoBehaviour
{
    public Health healthComponent;
    public bool isPlayer = false;

    static Counter health = new Counter(10, 10);
    static bool isPlaced = false;

    private void Update()
    {
        healthComponent.SyncWith(health);
        Debug.Log(health.GetCurrentCount());
    }

    void Start()
    {
        if (!isPlaced && !isPlayer)
            Place();
    }

    void OnDestroy()
    {
        if (isPlaced && !isPlayer)
            Remove();
    }

    public void DoDamage()
    {
        if (isPlayer ^ isPlaced)
            health.CountDown();
    }

    public static bool IsPlaced()
    {
        return isPlaced;
    }

    public static void Place()
    {
        isPlaced = true;
    }

    public static void Remove()
    {
        isPlaced = false;
        Item item = Inventory.GetItem("Artifact");
        if (item == null)
            return;

        Inventory.OnItemRemoved(item);
    }

    public static bool ArtifactDied()
    {
        return health.IsZero();
    }
}
