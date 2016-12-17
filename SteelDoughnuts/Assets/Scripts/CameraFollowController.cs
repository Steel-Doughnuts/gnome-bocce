using UnityEngine;
using System.Collections;

public class CameraFollowController : MonoBehaviour {

	private Camera cam;
	private Vector3 throwableScreenSpace;
	private bool resetNeeded = false;

	// Use this for initialization
	void Start () {
		cam = Camera.main;

	}

	// Update is called once per frame
	void Update () {

		//Finds where the thrown object is in relation to the screen
		throwableScreenSpace = cam.WorldToViewportPoint (transform.position);

		if (GetComponent<Throwable> ().thrown) {
			if (throwableScreenSpace.y >= 0.7f) {
				//Rotate Camera up
				cam.transform.Rotate (new Vector3 (-1.5f, 0, 0));
			} else if (throwableScreenSpace.y <= 0.25f) {
				//Rotate Camera down
				cam.transform.Rotate (new Vector3 (1.5f, 0, 0));
			} 
		} else if (GetComponent<Throwable> ().landed) {
			Debug.Log ("LANDED: " + cam.transform.rotation.eulerAngles);
		}
	
	}


}
