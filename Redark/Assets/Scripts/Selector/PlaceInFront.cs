using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridSnapping))]
public class PlaceInFront : MonoBehaviour
{
    [SerializeField] public FacingDirection facingDirection;

    GridSnapping snap;

    void Start()
    {
        snap = GetComponent<GridSnapping>();
        snap.EnableSnapping();

        if (facingDirection == null)
            Debug.LogError("No FacingDirection Especified for PlaceInFront");
    }

    void Update()
    {
        if (facingDirection != null)
            transform.position = facingDirection.FacingSnapPoint();
    }
}
