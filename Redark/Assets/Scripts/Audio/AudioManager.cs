using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

public class AudioManager : MonoBehaviour
{
	public List<Sound> audioList = new List<Sound>();
	public float masterVolume = 0.5f;

	public static List<Sound> sounds = new List<Sound>();
	
	static Dictionary<string, AudioSource> layers = new Dictionary<string, AudioSource>();
	static GameObject currentManager = null;
	private static float _masterVolume = 0.5f;

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
		SetMasterVolume(masterVolume);
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
		if (Mathf.Approximately(_masterVolume, 0f))
			return;
		
		Sound sound;
		try
		{
			sound = sounds.First((Sound sound) => sound.name == audio);
		}
		catch
		{
			return;
		}

		AddLayer(layer);

		layers[layer].Stop();

		layers[layer].volume = sound.volume * _masterVolume;
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

	public static void SetMasterVolume(float volume)
	{
		var scale = volume / _masterVolume;
		foreach (var audioSource in layers.Values)
			audioSource.volume *= scale;
		
		_masterVolume = volume;
	}

	public static float GetMasterVolume()
	{
		return _masterVolume;
	}
}
