using UnityEngine;
using System.Collections;
using Vuforia;
using System;
using UnityEngine.UI;

public class TerrainManager : MonoBehaviour, ITrackableEventHandler {
    private ReconstructionBehaviour mReconstructionBehaviour;
    private TrackableBehaviour mTrackableBehaviour;
    private GameObject primarySurface;
    private Rect maxArea;
    public GameObject rightWall;
    public GameObject leftWall;
    public GameObject frontWall;
    public GameObject backWall;
    public GameObject fakeFloor;
	public Text placeDollarText;
	public Text mapFloorText;
	public GameObject dollarCorners;

	private static bool alreadyDidStuff = false;
    
    // Use this for initialization
    void Start () {
        VuforiaBehaviour.Instance.RegisterOnPauseCallback(OnVuforiaPaused);
		if (!alreadyDidStuff) {
			placeDollarText.gameObject.SetActive (true);
			dollarCorners.gameObject.SetActive (true);
			mapFloorText.gameObject.SetActive (false);
		} else {
			placeDollarText.gameObject.SetActive (false);
			dollarCorners.gameObject.SetActive (false);
		}
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
    }

    private IEnumerator AllowUserToMap()
    { 
		if (!alreadyDidStuff) {
			CameraDevice.Instance.SetFlashTorchMode(true);
			placeDollarText.gameObject.SetActive (false);
			dollarCorners.gameObject.SetActive (false);
			mapFloorText.gameObject.SetActive (true);

			mapFloorText.text = "Move the camera slowly around the dollar for about 10 seconds.";
			yield return new WaitForSeconds (10);
			mapFloorText.text = "Begin game by throwing the flower seeds.";
			yield return new WaitForSeconds (5);
			mapFloorText.gameObject.SetActive (false);
			CameraDevice.Instance.SetFlashTorchMode(false);
			alreadyDidStuff = true;
		}			
        mReconstructionBehaviour = FindObjectOfType<ReconstructionBehaviour>();
        primarySurface = GameObject.Find("Primary Surface(Clone)");

        mReconstructionBehaviour.Reconstruction.Stop();
        mReconstructionBehaviour.Reconstruction.GetMaximumArea(out maxArea);

        //primarySurface.transform.right = new Vector3(maxArea.xMax, 0, 0);

        rightWall.SetActive(true);
        leftWall.SetActive(true);
        frontWall.SetActive(true);
        backWall.SetActive(true);
        fakeFloor.SetActive(true);
    }

	public static void begin(Throwable firstFlowerSeed) {
		if (alreadyDidStuff) {
			firstFlowerSeed.gameObject.SetActive (true);
		}
	}

    private void OnVuforiaPaused(bool paused)
    {
        if (paused) // resumed
        {
            // Set again autofocus mode when app is resumed
            CameraDevice.Instance.SetFlashTorchMode(false);
            StopAllCoroutines();
        }
    }

    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
               newStatus == TrackableBehaviour.Status.TRACKED ||
               newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            StartCoroutine(AllowUserToMap());
        }
    }
}
