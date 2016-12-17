using UnityEngine;
using System.Collections;

namespace AssemblyCSharp
{
	public class BombFlower : MonoBehaviour
	{
		public float blastRadius;

		public BombFlower ()
		{
		}

		void OnCollisionEnter(Collision collision) {
			if (collision.gameObject.GetComponent<Rigidbody> ()) {
				Debug.Log ("BOOM");
				EffectHandler.playExplosionEffects (transform.position);
				//Find all objects in the blastZone and apply a force away from the center
				Collider[] gnomes = Physics.OverlapSphere(transform.position,blastRadius);
				foreach (Collider col in gnomes) {
					if (col.gameObject.GetComponent<Rigidbody> ()) {
						Vector3 bombPos = transform.position;
						Vector3 gnomePos = col.transform.position;
						if (bombPos.Equals (gnomePos)) {
							//Then this IS the bomb flower
							continue;
						}
						Vector3 explosiveForce = new Vector3 (250f / (gnomePos.x - bombPos.x),
							250f / Vector3.Distance(bombPos,gnomePos), //The difference between the bomb and the gnome will be next to nothing
							250f / (gnomePos.z - bombPos.z));
						//Debug.Log (col.name + ": " + explosiveForce);
						col.gameObject.GetComponent<Rigidbody> ().AddForce (explosiveForce);
					}
				}
				//Get rid of the bomb flower after it has exploded
				gameObject.SetActive (false);
			}
		}
	}
}

