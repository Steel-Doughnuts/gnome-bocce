using UnityEngine;
using System.Collections;

public class AutoThrow : MonoBehaviour {

	// Autothrow method called by the GnomeManager.
	public static void fire(Throwable gnome, GnomeController target) {
		bool dart = false;
		Rigidbody body = gnome.gameObject.GetComponent <Rigidbody> ();
		Vector3 position = gnome.transform.position;
		Vector3 distance = target.transform.position - position;

		distance *= 0.85f;
		distance.y *= -1f;

		// different objects have different masses. Usually the default mass (pallena, normal gnome is 1)
		float mass = body.mass;
		if (mass < 0.5f) {
			FixedJoint joint = gnome.GetComponent<FixedJoint> ();
			Rigidbody connect = joint.connectedBody;
			mass = connect.mass / 3f;
			distance = distance / 0.85f; //dart gnome lands on hit, so no adjusting

			distance *= mass;
			dart = true;
		}
		//distance *= mass;

		RaycastHit hit;
		Physics.SphereCast (gnome.transform.position, 2f, distance, out hit, Mathf.Infinity);
		if(hit.rigidbody != null) {
			Vector3 hitPosition = hit.rigidbody.position;
			float xMult = (hitPosition.z - position.z) / distance.z;
			distance.y *= (1 + xMult);
			distance.z *= (1 + xMult);
		}

		// AI level
		float accuracy = Settings.AIProbability();
		float adjAccuracy = 1 - (accuracy / 100);

		// setting over and underthrow values for the randomizer
		float maxOver = 1 + adjAccuracy;
		float minOver = 1 - adjAccuracy;
		Debug.Log (minOver + " " + maxOver);

		//randomizing each part of the vector
		distance.x *= Random.Range (minOver, maxOver);
		distance.y *= Random.Range (minOver, maxOver);
		distance.z *= Random.Range (minOver, maxOver);

		Vector3 push = new Vector3 (body.position.x * 2f, body.position.y * 2f, body.position.z * 2f);
		body.freezeRotation = false;
		if (!dart) {
			body.AddForceAtPosition (distance, push, ForceMode.VelocityChange);
		}
		else {
			FixedJoint joint = gnome.GetComponent<FixedJoint> ();
			Rigidbody connect = joint.connectedBody;
			connect.AddForceAtPosition (distance, push, ForceMode.VelocityChange);
			connect.useGravity = true;
		}
		body.useGravity = true;

		gnome.thrown = true;
	}

}
