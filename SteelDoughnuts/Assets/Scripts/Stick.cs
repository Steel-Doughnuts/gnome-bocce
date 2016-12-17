using UnityEngine;
using System.Collections;

public class Stick : MonoBehaviour
{
	
	void OnCollisionEnter (Collision col) {

		//EffectHandler.playCollisionEffects (this.gameObject, col);


		GetComponent<Rigidbody> ().useGravity = false;
		GetComponent<Rigidbody> ().isKinematic = true;


		GetComponent<Rigidbody> ().freezeRotation = true;


	}
		
}

