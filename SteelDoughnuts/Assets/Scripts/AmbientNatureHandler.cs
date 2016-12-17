using UnityEngine;
using System.Collections;

public class AmbientNatureHandler : MonoBehaviour {

	public AudioSource natureAudioSource;
	public AudioClip testing;

	// Use this for initialization
	void Start () {
		if (Settings.ShouldPlaySFX()) {
			natureAudioSource.Play ();
		}
	}
}
