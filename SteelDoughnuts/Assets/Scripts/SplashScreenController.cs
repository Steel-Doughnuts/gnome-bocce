using UnityEngine;
using System.Collections;

// Scene manager for the Gnome Bocce game.
public class SplashScreenController : MonoBehaviour {

	private int time = 0;
	private static int done = 50;

	// Updates to (?)
	void Update () {
		time++;
		if (time > done) 
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene ("Scenes/MainMenu");
		}
	}
}
