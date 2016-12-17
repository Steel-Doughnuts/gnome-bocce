using UnityEngine;
using System.Collections;

//keeps the music audio source from being destroyed between scenes, and ensures only one exists

public class MusicSource : MonoBehaviour {

	private static MusicSource instance = null;

	public static MusicSource get () {
		return instance;
	}

	void Awake () {
		if (instance != null && instance != this) {
			Destroy (this.gameObject);
			return;
		} else {
			instance = this;
		}
		DontDestroyOnLoad (this.gameObject);
	}
}
