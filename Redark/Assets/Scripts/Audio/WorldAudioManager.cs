using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldAudioManager : MonoBehaviour
{
    public string layer = "world";
    public List<string> songs = new List<string>();

    void FixedUpdate()
    {
        if (AudioManager.IsLayerOccupied(layer))
            return;

        string song = songs[Random.Range(0, songs.Count - 1)];
        AudioManager.PlayOnLayer(layer, song);
    }
}
