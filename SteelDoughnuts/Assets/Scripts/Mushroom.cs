using UnityEngine;
using System.Collections;

public class Mushroom : Throwable {

	// objects to replace the mushroom on collision
	public GameObject sprout1;
	// Objects to identify for a collision
	public GameObject frontWall;
	public GameObject leftWall;
	public GameObject rightWall;
	public GameObject backWall;
	public GameObject floor;
	public GameObject ceiling;

	public bool created = false;
	private GameObject bloomedMushroom;

	// Overriding Start so gravity is always true
	public override void Start() {
		base.Start ();
		rgdbdy.angularDrag = 0f;
		rgdbdy.drag = 0f;
	}

	void Update() {

	}

	// Deals with collisions with a wall, floor or ceiling
	void OnCollisionEnter(Collision collision) 
	{
		StopMove ();
		if (!created) {
			// Initial Creation/Throwing of Mushroom handles rotation for walls
			if (collision.gameObject == floor) {
				Bloom ();
			} else if (collision.gameObject != ceiling) {
				RotateBloom (collision);
			} else if (collision.gameObject.name.Equals ("Flower_Bomb(Clone)")) {
				EffectHandler.playCollisionEffects (this.gameObject, collision);
			}
		} else {
			// After the mushrooms have been created
			// make gnoems bounce off of it
			GameObject gnome = collision.gameObject;
			Rigidbody collider = gnome.GetComponent <Rigidbody> ();
			Vector3 velocity = collider.velocity;

			Vector3 gnomePosition = gnome.transform.position;
			Vector3 position = this.transform.position;

			float xReflect = 1.2f;
			float yReflect = 1.2f;
			float zReflect = 1.2f;

			if (gnomePosition.x < position.x) {
				xReflect *= -1;
			}

			if (gnomePosition.y < position.y) {
				yReflect *= -1;
			}

			if (gnomePosition.z < position.z) {
				zReflect *= -1;
			}

			Vector3 bounce = new Vector3 (velocity.x * xReflect, velocity.y * yReflect, velocity.z * zReflect);
			collider.AddForce (bounce, ForceMode.VelocityChange);
		}
	}

	// let's the Mushroom appear above the pallena and throws it at a random vector
	public void Spawn(Vector3 position, Vector3 launchVector, float factor)
	{
		SoundPlayer.PlaySound ("Sounds/Motion/Being Thrown", position);

		Vector3 pop = new Vector3 ((position.x + 0f), (position.y + 1.2f + factor), (position.z + 0f));

		// moves the object and creates the random vector
		this.gameObject.transform.position = pop;
	
	/*  //Random throwing	
	    float xThrow = Random.Range (-10,10);
		float yThrow = Random.Range (-10,10);
		float zThrow = Random.Range (-10,10);
		Vector3 randomThrow = new Vector3 (xThrow, yThrow, zThrow);*/

		Vector3 randomMag = launchVector * Random.Range (0, 5);

		// Makes the object visible and throws said object
		this.gameObject.SetActive (true);
		this.GetComponent <Rigidbody> ().AddForceAtPosition (randomMag, this.transform.position * 2f, ForceMode.VelocityChange);
		this.GetComponent <Rigidbody> ().useGravity = true;
		this.GetComponent <Rigidbody> ().angularDrag = 0.05f;
		this.thrown = true;
	}
		
	// Creates the mushroom object in place of the sphere and replaces the sphere with the object
	public void Bloom() {

		bloomedMushroom = Instantiate (sprout1);
		bloomedMushroom.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y, this.transform.position.z);
		gameObject.SetActive (false);
		created = true;
		SoundPlayer.PlaySound ("Sounds/Impacts/Items/Item Plant", bloomedMushroom.transform.position);
	}

	//Check which wall the Mushroom has collided with, and after blooming, rotate the mushroom to be perpendicular to that surface.
	public void RotateBloom(Collision collision)
	{

		if (collision.gameObject == frontWall) {
			Bloom ();
			bloomedMushroom.transform.Rotate (new Vector3 (0f, -90f, 90f));
			bloomedMushroom.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y, this.transform.position.z + 0.5f);
			created = true;
		}
		if (collision.gameObject == leftWall) {
			Bloom ();
			bloomedMushroom.transform.Rotate (new Vector3 (0f, 0f, -90f));
			bloomedMushroom.transform.position = new Vector3 (this.transform.position.x - 0.5f, this.transform.position.y, this.transform.position.z);
			created = true;
		}
		if (collision.gameObject == rightWall) {
			Bloom ();
			bloomedMushroom.transform.Rotate (new Vector3 (0f, 0f, 90f));
			bloomedMushroom.transform.position = new Vector3 (this.transform.position.x + 0.5f, this.transform.position.y, this.transform.position.z);
			created = true;
		}
		if (collision.gameObject == ceiling) {
			Bloom ();
			bloomedMushroom.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y, this.transform.position.z);
			bloomedMushroom.transform.Rotate (new Vector3 (180f, 0f, 0f));
		}
		//TODO: BackWall.  I just don't care yet. And you shouldn't either.
		else {
			//Don't rotate
		}
	}
		
}
