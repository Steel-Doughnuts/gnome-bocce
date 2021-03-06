using UnityEngine;
using System.Collections;

public class WindTunnel : MonoBehaviour {

	public GameObject fanFlower;
	private int oscillationDir = 1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float flowerRotation = fanFlower.transform.rotation.eulerAngles.y;
		if (flowerRotation > 45f && flowerRotation < 315f) {
			oscillationDir = oscillationDir * -1;
		}
		fanFlower.transform.Rotate (0, (oscillationDir * .2f), 0);
	}

	//Using TriggerStay instead of OnCollisionEnter allows
	void OnTriggerStay(Collider col) 
	{
		if (col.gameObject.GetComponent<Rigidbody>()) {
			col.gameObject.GetComponent<Rigidbody> ().AddForce (0, 0, 3.8f);
		}
	}

	void OnTriggerEnter(Collider col)
	{
		EffectHandler.playWindTunnelEffects (col.transform.position);
	}
}
