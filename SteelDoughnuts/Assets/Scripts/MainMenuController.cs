using UnityEngine;
using System.Collections;
//this is a merging test.

public class MainMenuController : MonoBehaviour {

	//this is also a test
	public GameObject loadingImage;

	// Use this for initialization

	public void StartGame () {
		ScoreManager.resetScores ();
		string sceneToLoad = Settings.ShouldPlayAR () ? "Main_AR" : "Main";
		UnityEngine.SceneManagement.SceneManager.LoadScene ("Scenes/" + sceneToLoad);
	}

	public void Rules () {
		UnityEngine.SceneManagement.SceneManager.LoadScene ("Scenes/Rules");
	}

	public void SettingsMenu () {
		UnityEngine.SceneManagement.SceneManager.LoadScene ("Scenes/Settings");
	}

	public void Credits () {
		UnityEngine.SceneManagement.SceneManager.LoadScene ("Scenes/Credits");
	}

	public void MainMenu () {
		UnityEngine.SceneManagement.SceneManager.LoadScene ("Scenes/MainMenu");
	}

	// Update is called once per frame
	void Update () {

	}
}
