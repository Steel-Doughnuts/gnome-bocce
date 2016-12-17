using UnityEngine;
using System.Collections;

public class Throwable : MonoBehaviour {

	// Fields storing the start position and time of the throw.
	private Vector3 beginMousePosition;
	private System.DateTime beginTime;

	// Field storing the initial mouse pointer when calculating velocity vector
	private Vector3 oldposition;

	// Following variables are used to determine whether the gnome has been thrown 
	// and whether it has landed on the ground long enough to be counted 'at rest'.
	public bool onDragFired = false;
	public bool thrown;
	public bool landed;
	public bool dragging;
	public double restCount;

	// Effect spin has on the gnome
	// Sets magnusForce to 0 vector when multiplied?
	//public float magnusStrength = 1;

	//Let's the program know this gnome is a dart gnome
	public bool dart;
	public bool heavyGnome;

	public Vector3 zSpace;
	public Vector3 offset;

	public Rigidbody rgdbdy;
	public Rigidbody point;
	public Vector3 pointPos;

	// Start of a throw. The gnome is floating in air until it is thrown.
	public virtual void Start()
	{	
		rgdbdy = GetComponent<Rigidbody> ();

		if (dart) {
			point = GetComponent<FixedJoint> ().connectedBody;
			pointPos = point.transform.position;
		}
		// Initialize the gnome's physical properties
		rgdbdy.useGravity = false;
		rgdbdy.angularDrag = 0.5f;
		rgdbdy.drag = 0.5f;
		if (dart) {
			rgdbdy.angularDrag = 99999.9f;
			rgdbdy.drag = 25.0f;
		}

		if (heavyGnome) {
			rgdbdy.mass = 300;
			rgdbdy.angularDrag = 1f;//2.5
			rgdbdy.drag = 1f;//1.5
		}

		if (GetComponent<BoxCollider> ()) {
			GetComponent<BoxCollider> ().enabled = false;
		}
		if (GetComponent<SphereCollider> ()) {
			GetComponent<SphereCollider> ().enabled = true;
		}

		// Setting the thrown and landed variables as false.
		thrown = false;
		landed = false;
		dragging = false;
	}

	//Allows the gnome to move with your cursor/finger as you drag it across the screen
	public void OnDrag() {
		if (!thrown) {

			oldposition = transform.position;
			beginTime = System.DateTime.Now;

			var currentScreen = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, zSpace.z);
			var currentPosition = Camera.main.ScreenToWorldPoint (currentScreen) + offset;

			transform.position = currentPosition;
			if (dart) {
				Vector3 pointDiff = pointPos - oldposition; 
				point.position = currentPosition + pointDiff;
				pointPos = point.position;
			}


		}
	}

	// What happens when the throw is started.
	public void BeginDragHandler() 
	{
		onDragFired = true;
		if (!thrown) {
			// Records astarting mouse position and start time of throw.
			beginMousePosition = Input.mousePosition;
			beginTime = System.DateTime.Now;
			dragging = true;

			zSpace = Camera.main.WorldToScreenPoint (transform.position);
			offset = transform.position - Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, zSpace.z));
		}
	}

	// End of throw. (click released)
	public virtual void EndDragHandler() 
	{
		if (!thrown) {
			SoundPlayer.PlaySound ("Sounds/Motion/Being Thrown", transform.position);
			// The end position of the mouse drag as well as the time the left mouse button
			// was released.
			Vector3 endMousePosition = Input.mousePosition;
			System.DateTime endTime = System.DateTime.Now;
			dragging = false;

			// Finding the time of the throw in milliseconds.
			System.TimeSpan diffTime = endTime.Subtract (beginTime);
			float diffMilli = diffTime.Milliseconds;
			float diffSecs = diffMilli / 1000;

			// Calculates the velocity vector of object at the instant it is released
			Vector3 vel = new Vector3 ();
			vel.x = 0.75f * (transform.position.x - oldposition.x) / (diffSecs);
			vel.y = 0.5f * (transform.position.y - oldposition.y) / (diffSecs);
			vel.z = vel.y / 0.7f;

			// Calculating the distance traveled by the mouse drag.
			float endX = endMousePosition.x;
			float endY = endMousePosition.y;
			float beginX = beginMousePosition.x;
			float beginY = beginMousePosition.y;

			// Mouse position in relation to center of mass of thrown object
			Vector3 relativeMousePosition = Input.mousePosition;
			relativeMousePosition.x -= Screen.width / 2;
			relativeMousePosition.y -= Screen.height / 2;

			float diffX = Mathf.Abs (endX - beginX);
			float diffY = Mathf.Abs (endY - beginY);

			// Calculating the on screen distance of the throw.
			float distance = Mathf.Sqrt (Mathf.Pow (diffX, 2) + Mathf.Pow (diffY, 2));

			// Calculating the velocity of the throw and using that speed to calculate the z vector.
			Vector3 yVelocity = 0.05f * (new Vector3 (endX - beginX, endY- beginY, distance));
			Vector3 totalVelocity = (endMousePosition - beginMousePosition) / diffMilli;
			float fudge = 5f;
			Vector3 result = (fudge * totalVelocity) + yVelocity;

			// Applying the calculated force on the gnome after the throw has been complete.
			// The gnome will bounce according to the physics engine if it still has speed
			// and runs into an object .

			if (dart) {
				GetComponent<FixedJoint> ().connectedBody.isKinematic = false;
				GetComponent<FixedJoint> ().connectedBody.useGravity = true;
				vel.z = 2.0f * vel.z;
				//Speed limit for Dart Gnome. Any force over 32.0f in the z direction is unnecessary
				if (vel.z > 31.0f) {
					vel.z = 31.0f;
				}
				if (Mathf.Abs(vel.y) > 12.0f) {
					if (vel.y > 0)
						vel.y = 12.0f;
					else
						vel.y = -12.0f;
				}
				if (Mathf.Abs (vel.x) > 10.0f) {
					if (vel.x > 0)
						vel.x = 10.0f;
					else
						vel.x = -10.0f;
				}
				GetComponent<FixedJoint> ().connectedBody.AddForce (vel, ForceMode.VelocityChange);
				thrown = true;
			} else {
				GetComponent<Rigidbody> ().AddForceAtPosition (vel, relativeMousePosition, ForceMode.VelocityChange);
				GetComponent<Rigidbody> ().useGravity = true;
				thrown = true;
			}
			Invoke("allThisForAQuarterSecondDelay",0.25f);
		}
	}

	void allThisForAQuarterSecondDelay() {
		SoundPlayer.PlaySound ("Sounds/Motion/Flying", transform.position);
	}

	void FixedUpdate () {

		if (dart && GetComponent<FixedJoint> ().connectedBody.freezeRotation) {
			GetComponent<Rigidbody> ().useGravity = false;
			GetComponent<Rigidbody> ().isKinematic = true;
			GetComponent<Rigidbody> ().freezeRotation = true;
		}

		if (GetComponent<Rigidbody>().angularVelocity.magnitude == 0) {
			// Do nothing, no spin
		}
		else {
			Vector3 angVelocity = GetComponent<Rigidbody> ().angularVelocity;
			Vector3 magnusForce = Vector3.Cross (angVelocity, new Vector3 (0, 0, .5f));
			GetComponent<Rigidbody> ().AddForce (magnusForce);
		}
	}

	//Flower uses this same method to stop moving
	public void StopMove()
	{
		rgdbdy.freezeRotation = true;
		rgdbdy.velocity.Set (0f, 0f, 0f);
		rgdbdy.angularVelocity.Set (0f, 0f, 0f);
		landed = true;

		// Set these two as a fail safe to keep the game moving
		onDragFired = true;
		thrown = true;
		dragging = false;
	}
		
}
