using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class AudioManager : MonoBehaviour
{
	public List<Sound> audioList = new List<Sound>();

	public static List<Sound> sounds = new List<Sound>();
	static Dictionary<string, AudioSource> layers = new Dictionary<string, AudioSource>();
	static GameObject currentManager = null;

	void Start()
	{
		if (currentManager != null)
		{
			sounds.AddRange(audioList);
			Destroy(this.gameObject);
			return;
		}

		currentManager = this.gameObject;
		DontDestroyOnLoad(currentManager);

		sounds.AddRange(audioList);
	}

	public static void AddLayer(string layer)
	{
		if (layers.ContainsKey(layer))
			return;

		AudioSource source = currentManager.AddComponent<AudioSource>();
		layers.Add(layer, source);
	}

	public static void PlayOnLayer(string layer, string audio)
	{
		Sound sound = sounds.First((Sound sound) => sound.name == audio);
		if (sound == null)
			return;

		AddLayer(layer);

		layers[layer].Stop();

		layers[layer].volume = sound.volume;
		layers[layer].loop = sound.loop;
		layers[layer].clip = sound.clip;
		layers[layer].Play();
	}

	public static bool IsLayerOccupied(string layer)
	{
		if (!layers.ContainsKey(layer))
			return false;

		return layers[layer].isPlaying;
	}
}
