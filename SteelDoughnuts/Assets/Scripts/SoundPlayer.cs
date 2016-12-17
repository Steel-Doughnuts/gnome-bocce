using UnityEngine;
using System.Collections;

// handles playing sounds
// only one can exist, and it will persist between scenes

public class SoundPlayer : MonoBehaviour {

	private static SoundPlayer instance=null;

	//audio source for the background music
	public AudioSource backgroundMusicSource;

	private static AudioClip mainSceneMusic;
	private static AudioClip loadingSceneMusic;
	private static AudioClip menusMusic;

	public static SoundPlayer get() {
		return instance;
	}

	//plays sound effects
	public static void PlaySound(string soundFileName, Vector3 position, float volume=1) {
		if (Settings.ShouldPlaySFX()) {
			AudioClip sound = Resources.Load<AudioClip>(soundFileName);
			AudioSource.PlayClipAtPoint (sound, position, volume);
		}
	}

	//keep alive
	void Awake()
	{
		if (instance != null && instance != this) {
			Destroy (this.gameObject);
			return;
		} else {
			instance = this;
		}
		DontDestroyOnLoad (this.gameObject);
		mainSceneMusic = Resources.Load<AudioClip>("Sounds/MainGame");
		loadingSceneMusic = Resources.Load<AudioClip>("Sounds/LoadingPausedEndOfGame");
		menusMusic = Resources.Load<AudioClip>("Sounds/Menus");
	}
		
	void Update() {
		string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene ().name;

		//set appropriate music to play
		//can't play music during loading to my knowledge
		if (sceneName.Equals("Scenes/Main") || sceneName.Equals("Scenes/MainAR")) {
			if (!backgroundMusicSource.clip.Equals(mainSceneMusic)) {
				backgroundMusicSource.Stop();
				backgroundMusicSource.clip = mainSceneMusic;
			}
		} else if (!backgroundMusicSource.clip.Equals(menusMusic)) {
			backgroundMusicSource.Stop();
			backgroundMusicSource.clip = menusMusic;
		}

		//play if supposed to
		if (Settings.ShouldPlayMusic ()) {
			if (!backgroundMusicSource.isPlaying) {
				backgroundMusicSource.Play ();
			}
		} else {
			if (backgroundMusicSource.isPlaying) {
				backgroundMusicSource.Stop ();
			}
		}
	}
}
