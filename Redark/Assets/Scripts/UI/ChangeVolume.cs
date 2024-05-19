using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeVolume : MonoBehaviour
{
    public float bump = 0.1f;
    public KeyCode volumeUpKey = KeyCode.D;
    public KeyCode volumeDownKey = KeyCode.A;

    public void FixedUpdate()
    {
        if (!GameState.IsGameplayPaused())
            return;
        
        if (Input.GetKeyDown(volumeDownKey))
            VolumeDown();
        
        if (Input.GetKeyDown(volumeUpKey))
            VolumeUp();
    }

    public void VolumeUp()
    {
        var newVolume = Mathf.Clamp(AudioManager.GetMasterVolume() + bump, 0f, 1f);
        AudioManager.SetMasterVolume(newVolume);
    }

    public void VolumeDown()
    {
        var newVolume = Mathf.Clamp(AudioManager.GetMasterVolume() - bump, 0f, 1f);
        AudioManager.SetMasterVolume(newVolume);
    }
}
