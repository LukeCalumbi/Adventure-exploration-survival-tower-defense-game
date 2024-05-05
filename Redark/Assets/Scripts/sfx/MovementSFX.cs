using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(GridMovement))]
public class MovementSFX : MonoBehaviour
{
    public float pitchRange = 0.1f;
    public float volumeRange = 0.1f;

    float basePitch;
    float baseVolume;
    AudioSource source;
    GridMovement movement;

    void Start()
    {
        source = GetComponent<AudioSource>();
        movement = GetComponent<GridMovement>();

        basePitch = source.pitch;
        baseVolume = source.volume;
    }

    // Update is called once per frame
    void Update()
    {
        if (!source.isPlaying && movement.IsMoving())
            Play();

        if (movement.IsIdle())
            source.Stop();
    }

    void Play()
    {
        source.pitch = basePitch + Random.Range(-pitchRange, pitchRange);
        source.volume = baseVolume + Random.Range(-volumeRange, volumeRange);
        source.Play();
    }
}
