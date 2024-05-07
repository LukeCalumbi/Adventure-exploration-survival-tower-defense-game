using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CharacterAnimator : MonoBehaviour
{
    public GridMovement gridMovement;
    public FacingDirection facingDirection;
    public float timeBetweenFrames = 0.16f;
    public List<Frame> up;
    public List<Frame> down;
    public List<Frame> left;
    public List<Frame> right;

    SpriteRenderer spriteRenderer;
    Vector3 current = Vector3.forward;
    int frameIndex = 0;
    Timer frameTimer;
    Dictionary<Vector3, List<Frame>> directionAnimations;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        directionAnimations = new Dictionary<Vector3, List<Frame>>();
        directionAnimations[Vector3.up] = up;
        directionAnimations[Vector3.down] = down;
        directionAnimations[Vector3.left] = left;
        directionAnimations[Vector3.right] = right;

        frameTimer = new Timer(timeBetweenFrames);
        frameIndex = 0;
        ChangeTo(Vector3.right);
    }

    void Update()
    {
        if (frameTimer.Finished())
        {
            AdvanceFrame();
            return;
        }

        Vector3 facing = facingDirection.Get();
        Vector3 movement = gridMovement.GetMovementDirection();

        if (facing == Vector3.zero && movement == Vector3.zero) 
        {
            ChangeTo(Vector3.right);
            return;
        }

        if (gridMovement.IsIdle())
            ChangeTo(facingDirection.Get());

        else if (current != gridMovement.GetMovementDirection())
            ChangeTo(gridMovement.GetMovementDirection());
    }

    void FixedUpdate() 
    { 
        frameTimer.Update(Time.fixedDeltaTime); 
    }

    void AdvanceFrame()
    {
        frameIndex = (frameIndex + 1) % directionAnimations[current].Count;
        UpdateSprite();
        frameTimer.Start();
    }

    void ResetCurrentAnimation()
    {
        frameIndex = 0;
        UpdateSprite();
        frameTimer.Start();
    }

    void ChangeTo(Vector3 direction)
    {
        current = direction;
        ResetCurrentAnimation();
    }

    void UpdateSprite()
    {
        spriteRenderer.sprite = directionAnimations[current][frameIndex].frame;
        spriteRenderer.flipX = directionAnimations[current][frameIndex].flipX;
        spriteRenderer.flipY = directionAnimations[current][frameIndex].flipY;
    }
}
