using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridSnapping))]
public class PlaceInFrontOfPlayer : MonoBehaviour
{
    [SerializeField] public FacingDirection facingDirection;

    GridSnapping snap;

    void Start()
    {
        snap = GetComponent<GridSnapping>();
        snap.EnableSnapping();

        if (facingDirection == null)
            Debug.LogError("No FacingDirection Especified for PlaceInFrontOfPlayer");
    }

    void Update()
    {
        transform.position = facingDirection.FacingSnapPoint();
    }
}
