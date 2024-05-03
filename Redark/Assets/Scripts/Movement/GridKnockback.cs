using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridMovement))]
public class GridKnockback : MonoBehaviour
{
    public float timeInKnockback = 0.5f;

    float lastMoveSpeed = -1f;
    int lastStepCount = -1;

    GridMovement gridMovement;
    Timer knockbackTimer;

    void Start()
    {
        gridMovement = GetComponent<GridMovement>();
        knockbackTimer = new Timer(timeInKnockback);
        ResetLastValues();
    }

    void LateUpdate()
    {
        if (IsInKnockback())
            Debug.Log("bOIOLA");

        if (!IsInKnockback()) 
            return;
        
        if (knockbackTimer.Finished() || gridMovement.IsIdle())
            OnKnockbackEnd();

        knockbackTimer.Update(Time.deltaTime);
    }

    public void ApplyKnockback(Vector3 direction, float moveSpeed, int stepCount)
    {
        if (!IsInKnockback())
            GetValuesFromGridMovement();

        gridMovement.moveSpeed = moveSpeed;
        gridMovement.stepCount = stepCount;

        gridMovement.ForceMove(direction);
        knockbackTimer.Start();
    }

    void OnKnockbackEnd()
    {
        LoadLastValues();
        ResetLastValues();
        knockbackTimer.ForceEnd();
        gridMovement.ForceStop();
    }

    void GetValuesFromGridMovement()
    {
        lastMoveSpeed = gridMovement.moveSpeed;
        lastStepCount = gridMovement.stepCount;
    }

    void LoadLastValues()
    {
        gridMovement.moveSpeed = lastMoveSpeed;
        gridMovement.stepCount = lastStepCount;
    }

    void ResetLastValues()
    {
        lastMoveSpeed = -1f;
        lastStepCount = -1;
    }

    bool IsInKnockback() 
    {
        return lastMoveSpeed != -1f && lastStepCount != -1;
    }
}
