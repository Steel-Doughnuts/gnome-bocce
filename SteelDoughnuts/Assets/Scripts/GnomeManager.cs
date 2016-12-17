using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Vuforia;

public class GnomeManager : MonoBehaviour {

	// Initializing the pallena, and the three gnomes for each player.
	public GnomeController pallena;
	public GnomeController p1gnome1;
	public GnomeController p2gnome1;
	public GnomeController p1gnome2;
	public GnomeController p2gnome2;
	public GnomeController p1gnome3;
	public GnomeController p2gnome3;
	public Flower[] flowers;

	// Initializing the mushrooms and a bool which deals with whether the mushrooms have been created
	public bool mushroomGen;
	public Mushroom mushroom1;
	public Mushroom mushroom2;
	public Mushroom mushroom3;

	// The message displayed when a player wins.
	public Text winText;
	public Text gameOverText;
	public bool scoreAnnounced;
	public bool gameOver;
	public Button nextRoundButton;
	public Button newGameButton;
	public Button returnToMainButton;

	// Score and gnome indicators
	public Text player1Score;
	public Text player2Score;
	public Button p1g1Dot;
	public Button p1g2Dot;
	public Button p1g3Dot;
	public Button p2g1Dot;
	public Button p2g2Dot;
	public Button p2g3Dot;

	// AR/Vuforia Stuff
    public bool isAR = false;
	public Camera arCamera;
	private static int AR_FRAME_RATE_BUFFER = 10;
	private int arMoveObjectFrameDelay = AR_FRAME_RATE_BUFFER;

	// Stores the player's gnomes.
	private ArrayList player1;
	private ArrayList player2;

	// Initializing a list to hold our throwables
	public List<Throwable> throwables;
	public int throwableIndex = 0;

	// Variables for the AI Autothrowing
	private static int AUTOTHROW_DELAY = 100;
	private int AutoThrowDelay = AUTOTHROW_DELAY;

	// variables that will be used for the instant replay feature
	public int replayFrameIndex = 0;
	private static bool replaying = false;
	public Button replayButton;
	private Camera gnomePro = null;
	public Text gnomeProText = null;

	public GameObject swipeUp = null;
	private int removeSwipeUp = 20;

	// Start of the game.
	public void Start ()
	{
		isAR = Settings.ShouldPlayAR ();
	    if (isAR)
	    {
            VuforiaBehaviour.Instance.RegisterVuforiaStartedCallback(OnVuforiaStarted);
            VuforiaBehaviour.Instance.RegisterOnPauseCallback(OnPaused);
        }
		// Assigns gnomes to player 1. (3 gnomes, all the same color)
		player1 = new ArrayList ();
		player1.Add (p1gnome1);
		player1.Add (p1gnome2);
		player1.Add (p1gnome3);

		// Assigns gnomes to player 2. (3 gnomes, all the same color)
		player2 = new ArrayList ();
		player2.Add (p2gnome1);
		player2.Add (p2gnome2);
		player2.Add (p2gnome3);

		//Add everything that is going to be thrown, one at a time,
		//to the throwables list, in the order it will be thrown
		Shuffle(flowers);
		throwables.Add (flowers [0]);
		throwables.Add (flowers [1]);
		throwables.Add (flowers [2]);
		throwables.Add (pallena);
		throwables.Add (p1gnome1);
		throwables.Add (p2gnome1);
		throwables.Add (p1gnome2);
		throwables.Add (p2gnome2);
		throwables.Add (p1gnome3);
		throwables.Add (p2gnome3);

		// Hides every game object except the first object
		foreach (Throwable g in throwables) {
			g.gameObject.SetActive (false);
		}
		p1gnome3.GetComponent<FixedJoint> ().connectedBody.gameObject.GetComponent<SphereCollider> ().enabled = false;
		p2gnome3.GetComponent<FixedJoint> ().connectedBody.gameObject.GetComponent<SphereCollider> ().enabled = false;
		if (!isAR) {
			throwables [0].gameObject.SetActive (true);
		}
		
		// Hides the mushrooms (these aren't thrown and will be set active once the pallena is done)
		mushroom1.gameObject.SetActive (false);
		mushroom2.gameObject.SetActive (false);
		mushroom3.gameObject.SetActive (false);
		mushroomGen = false;

		// Game has started, hide all the things!
		scoreAnnounced = false;
		winText.gameObject.SetActive (false);
		gameOverText.gameObject.SetActive (false);
		gameOver = ScoreManager.GameIsOver ();
		getGnomePro().gameObject.SetActive (false);
		gnomeProText.gameObject.SetActive (false);
		nextRoundButton.gameObject.SetActive (false);
		newGameButton.gameObject.SetActive (false);
		returnToMainButton.gameObject.SetActive (false);
		if (!isAR && swipeUp != null) {
			swipeUp.gameObject.SetActive (true);
		}
		DisplayScores ();
		ReplayManager.hideReplayButton (replayButton);
    }

	// Updates the screen after each object has thrown and stopped moving.
	public void Update()
	{
		if (isAR) {
			// Dependent on there being only one Terrain Manager in the scene.
			TerrainManager.begin (flowers[0]);
		}

		if (throwableIndex >= throwables.Count) {
			endRound ();
			handleReplay ();
			return;
		}

		Throwable current = throwables [throwableIndex];

		// at end of throw if the display isn't replaying
		if (!current.thrown && !replaying) {
		
			// if the pallena has been thrown
			if (pallena.thrown) {
				if (Auto()) {
					AutoThrow.fire (current, pallena);
				}
			}
			return;
		}

		if (current.landed) {
			StartCoroutine (uprightPallena ());
		}

		if (current.thrown && !current.landed && !replaying) {
			ReplayManager.hideReplayButton (replayButton);
			ReplayManager.AddFrame (this);
		} else if (!replaying) {
			if (throwableIndex > 2) {
				ReplayManager.showReplayButton (replayButton);
			}
			makeNextThrowableActive ();
		}

		handleReplay ();
		removeSwipeUp--;
		if (removeSwipeUp == 0 && swipeUp != null) {
			swipeUp.gameObject.SetActive (false);
		}
	}

	public void LateUpdate() {
		if (replaying) {
			// Set the gnome pro's position to near/right in front of the replaying (hence -1) gnome
			if (!isAR) {
				getGnomePro ().transform.position = throwables [throwableIndex - 1].gameObject.transform.position + new Vector3 (0.0f, 0.0f, 1f);		
				SplitMainCameraView ();
			}
		} else if (isAR) {
			if (arMoveObjectFrameDelay == 0) {
				var multiplier = 2f;
				foreach (Throwable throwable in throwables) {
					if (!throwable.onDragFired) {
						var forward = arCamera.transform.forward;
						throwable.transform.position = arCamera.transform.position + (multiplier * forward);
					}
				}
				arMoveObjectFrameDelay = AR_FRAME_RATE_BUFFER;
			} else {
				arMoveObjectFrameDelay--;
			}
		}
	}
		
	public static void setToReplaying() {
		replaying = true;
	}

	private void handleReplay() {
		if (replaying) {
			ReplayManager.hideReplayButton (replayButton);
			winText.gameObject.SetActive (false);
			nextRoundButton.gameObject.SetActive (false);
			newGameButton.gameObject.SetActive (false);
			returnToMainButton.gameObject.SetActive (false);
			replaying = ReplayManager.replayOneFrame (this);
			getGnomePro ().gameObject.SetActive (true);
			if (!isAR) {
				gnomeProText.gameObject.SetActive (true);
			}
			if (replaying) {
				replayFrameIndex++;
			} else {
				if (throwableIndex < throwables.Count) {
					throwables [throwableIndex].gameObject.SetActive (true);
				}
				ReplayManager.resetAll ();
				replayFrameIndex = 0;
				replaying = false;
				getGnomePro().transform.position = ReplayManager.initialCameraLocation;
				getGnomePro().gameObject.SetActive (false);
				gnomeProText.gameObject.SetActive (false);
				UnSplitMainCameraView ();
				if (gameOver) {
					newGameButton.gameObject.SetActive (true);
					returnToMainButton.gameObject.SetActive (true);
					winText.gameObject.SetActive (true); 
				} else if (scoreAnnounced) {
					winText.gameObject.SetActive (true);
					nextRoundButton.gameObject.SetActive (true);
				}
			}
		}
	}

	private Camera getGnomePro() {
		if(gnomePro == null) {
			Camera[] cameras = Camera.allCameras;
			for (int i = 0; i < cameras.Length; i++) {
				if (cameras [i].name.Equals("GnomePro")) {
					gnomePro = cameras [i];
				}
			}
		}
		return gnomePro;
	}

	private void SplitMainCameraView() {
		Rect rect = new Rect (Camera.main.rect.x, 1 - (-1 * getGnomePro ().rect.y), Camera.main.rect.width, Camera.main.rect.height);
		Camera.main.rect = rect;
	}

	private void UnSplitMainCameraView() {
		Rect rect = new Rect (Camera.main.rect.x, 0, Camera.main.rect.width, Camera.main.rect.height);
		Camera.main.rect = rect;
	}

	// Sets the next object as active and let's the player throw it
	private void makeNextThrowableActive() {
		if (swipeUp != null) {
			swipeUp.gameObject.SetActive (false);
		}
		UpdateGnomeDots (throwableIndex);
		throwableIndex++;
		if (throwableIndex < throwables.Count) {
			throwables [throwableIndex].gameObject.SetActive (true);
			if (throwables [throwableIndex].gameObject.GetComponent<FixedJoint> ()) {
				throwables [throwableIndex].GetComponent<FixedJoint> ().connectedBody.gameObject.GetComponent<SphereCollider> ().enabled = true;
			} 
		}

		// if the last thrown item was the pallena, spawns the 3 mushrooms and sets the mushroomGen boolean as true
		if (pallena.thrown && pallena.landed && !mushroomGen) {
			if (!isAR) {
				mushroom1.Spawn (pallena.transform.position, new Vector3 (0, 0.25f, 10f), 1f);
				mushroom2.Spawn (pallena.transform.position, new Vector3 (1f, 0.25f, -1f), 2f);
				mushroom3.Spawn (pallena.transform.position, new Vector3 (-1f, 0.25f, -1f), 3f);
			}
			mushroomGen = true;
		}
	}

	private void UpdateGnomeDots(int currentIndex) {
		if (currentIndex == 4) {
			SetButtonToWhite (p1g1Dot);
		}
		if (currentIndex == 5) {
			SetButtonToWhite (p2g1Dot);
		}
		if (currentIndex == 6) {
			SetButtonToWhite (p1g2Dot);
		}
		if (currentIndex == 7) {
			SetButtonToWhite (p2g2Dot);
		}
		if (currentIndex == 8) {
			SetButtonToWhite (p1g3Dot);
		}
		if (currentIndex == 9) {
			SetButtonToWhite (p2g3Dot);
		}

	}

	private void SetButtonToWhite(Button button) {
		button.enabled = false;
		button.image.rectTransform.sizeDelta = new Vector2 (20, 20);
	}

	public bool Auto() {
		bool P2Gnome = throwableIndex == 5 || throwableIndex == 7 || throwableIndex == 9;
		bool AIPlaying = Settings.ShouldPlayAI () && !Settings.ShouldPlayAR ();
		bool auto = P2Gnome && AIPlaying;
		if (auto) {
			if (AutoThrowDelay == 0) {
				AutoThrowDelay = AUTOTHROW_DELAY;
				return true;
			} else {
				AutoThrowDelay--;
				return false;
			}
		}
		return false;
	}

	// End of the round
	private void endRound() {
		if (!scoreAnnounced) {
			ScoreManager.CalculateScore (this, player1, player2);
			DisplayScores ();
			if (gameOver || ScoreManager.GameIsOver ()) {
				gameOverText.text = ScoreManager.GetGameOverText ();
				gameOverText.gameObject.SetActive (true);
				newGameButton.gameObject.SetActive (true);
				returnToMainButton.gameObject.SetActive (true);
				gameOver = true;
			} else {
				nextRoundButton.gameObject.SetActive (true);
			}
		}
	}

	public static void GoToNextRound() {
		string sceneName = "Scenes/Main";
		if (Settings.ShouldPlayAR()) {
			sceneName = "Scenes/Main_AR";
		}
		UnityEngine.SceneManagement.SceneManager.LoadScene (sceneName);
	}

	public void DisplayScores() {
		player1Score.text = ScoreManager.getPlayer1Score ().ToString();
		player2Score.text = ScoreManager.getPlayer2Score ().ToString();
	}

    private void OnVuforiaStarted()
    {
        CameraDevice.Instance.SetFocusMode(
            CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
    }

    private void OnPaused(bool paused)
    {
        if (!paused) // resumed
        {
            // Set again autofocus mode when app is resumed
            CameraDevice.Instance.SetFocusMode(
                CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
        }
	}

	//Fisher-Yates shuffle: http://stackoverflow.com/questions/273313/randomize-a-listt
	private void Shuffle(Flower[] flowers) {
		System.Random rng = new System.Random();  

		int n = flowers.Length;  
		while (n > 1) {  
			n--;  
			int k = rng.Next(n + 1);  
			Flower value = flowers[k];  
			flowers[k] = flowers[n];  
			flowers[n] = value;  
		}
	}

	//Resets the pallena to upright position after a throw
	private IEnumerator uprightPallena () {

		//Waits one second before trying to upright pallena
		yield return new WaitForSeconds (1);

		//Attempts to right the gnome for 100 frames
		for (int i = 0; i < 100; i++){
			Quaternion q = Quaternion.FromToRotation (pallena.transform.up, Vector3.up) * pallena.transform.rotation;
			pallena.transform.rotation = Quaternion.Slerp (pallena.transform.rotation, q, Time.deltaTime * 3.5f);

			q = Quaternion.FromToRotation (pallena.transform.forward, Vector3.back) * pallena.transform.rotation;
			pallena.transform.rotation = Quaternion.Slerp (pallena.transform.rotation, q, Time.deltaTime * 3.5f);

			yield return null;
		}
			
	}

}
