using UnityEngine;
using System.Collections;
/*
 * Author: Joe Fennimore
 * Desc: Flower is a type of throwable that upon its first contact with, is supposed to freeze and transform
 * 		into a blossomed version of that flower. This script essentially describes the behavior of the SEED
 */
public class Flower : Throwable {

	//Prefab for what the flower should look like after it blooms
	public GameObject bloomedPrefab;
	//Need information about what surfaces are which walls so that the flower can bloom perpendicular to the surface
	public GameObject frontWall;
	public GameObject leftWall;
	public GameObject rightWall;
	public GameObject backWall;
	public GameObject floor;
	public GameObject ceiling;

	public bool blossomed = false;
	private GameObject bloomedflower;

	private static string tempSoundFolder = "sounds/placeholder sounds/";
	private static string thrownSound = tempSoundFolder + "punch_jack_02";
	private static string growSound = tempSoundFolder + "powerup_01";
	private static string bounceSound = tempSoundFolder + "boing_jack_01";

	public bool wall;

	public void Start()
	{
		base.Start ();
		GetComponent<Rigidbody> ().mass = 5;
	}



	void OnCollisionEnter(Collision collision) 
	{
		StopMove ();
		if (!blossomed) {
			if (collision.gameObject == floor || collision.gameObject.name == "Primary Surface(Clone)") {
				Bloom ();
			} else if (collision.gameObject != ceiling) {
				RotateBloom (collision);
			}
		}
	}

	void Update() 
	{
		//Don't un-refreeze the position and rotation.  This is essentially overriding the gnome's behavior of stop-and-go
	}

	//This is basically FloorBloom()
	public void Bloom()
	{
		bloomedflower = Instantiate(bloomedPrefab);
		//The cylinders that bloomed from the seeds had a strange habit of appearing below the floor, 
		//so if the seed thinks it's underground, set it above ground to a height of 1
		float flowerHeight = transform.localPosition.y > 0f ? transform.localPosition.y : 0f;
		if (Settings.ShouldPlayAR ()) {
			bloomedflower.transform.position = gameObject.transform.position;
			bloomedflower.transform.localScale = bloomedflower.transform.localScale * 0.1f;
		} else {
			//Again, I'm not sure why, but when blooming, the flower appeared to move to the right, so I've added 2f to correct.
			//Probably has something to do with the relative 'center' of the seed vs. the blossom.
			bloomedflower.transform.localPosition = new Vector3(transform.localPosition.x, flowerHeight, transform.localPosition.z);
			bloomedflower.transform.localRotation = new Quaternion (0f, 0f, 0f, 0f);
		}
		blossomed = true;
		//Remove the seed from the field
		gameObject.SetActive (false);
		SoundPlayer.PlaySound ("Sounds/Impacts/Items/Item Plant", transform.position);
	}

	//Check which wall the flower has collided with, and after blooming, rotate the flower to be perpendicular to that surface.
	public void RotateBloom(Collision collision)
	{
		if (collision.gameObject == frontWall) {
			Bloom ();
			bloomedflower.transform.Rotate (new Vector3 (0f, -90f, 90f));
		}
		if (collision.gameObject == leftWall) {
			Bloom ();
			bloomedflower.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y, this.transform.position.z);
			bloomedflower.transform.Rotate (new Vector3 (0f, 0f, -90f));
		}
		if (collision.gameObject == rightWall) {
			Bloom ();
			bloomedflower.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y, this.transform.position.z);
			bloomedflower.transform.Rotate (new Vector3 (0f, 0f, 90f));
		}
		if (collision.gameObject == ceiling) {
			Bloom ();
			bloomedflower.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y, this.transform.position.z);
			bloomedflower.transform.Rotate (new Vector3 (180f, 0f, 0f));
		}
		//TODO: BackWall.  I just don't care yet. And you shouldn't either.
		else {
			//Don't rotate
		}

		if (wall) {
			if (bloomedflower.transform.position.y < 2.0f) {
				Vector3 position = bloomedflower.transform.position;
				position.y = 2f;

				bloomedflower.transform.position = position;
			}
		}
		else if (bloomedflower.transform.position.y < 1.0f) {
			Vector3 position = bloomedflower.transform.position;
			position.y = 1.0f;

			bloomedflower.transform.position = position;
		}
	}
}
