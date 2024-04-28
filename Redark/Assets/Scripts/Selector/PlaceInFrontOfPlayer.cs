using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridSnapping))]
public class PlaceInFrontOfPlayer : MonoBehaviour
{
    [SerializeField] public PlayerMovement player;

    GridSnapping snap;

    void Start()
    {
        snap = GetComponent<GridSnapping>();
        snap.EnableSnapping();

        if (player == null)
            Debug.LogError("No Player Especified for PlaceInFrontOfPlayer");
    }

    void Update()
    {
        transform.position = player.GetSnapPointInFrontOfPlayer();
    }
}
