using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;

public class Artifact : MonoBehaviour
{
    public bool isPlayer = false;

    static Counter health = new Counter(10, 10);
    static bool isPlaced = false;

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
            health.CountDown(1);
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
