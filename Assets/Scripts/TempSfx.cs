using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class TempSfx : MonoBehaviour {

	AudioSource source;
	bool played = false;

	void Start () {
		source = GetComponent<AudioSource> ();
		DontDestroyOnLoad (gameObject);
	}
	
	void Update () {
		if (played && !source.isPlaying) {
			Destroy (gameObject, 1f);
		}
	}

	public void Play () {
		source.Play ();
		played = true;
	}
}
