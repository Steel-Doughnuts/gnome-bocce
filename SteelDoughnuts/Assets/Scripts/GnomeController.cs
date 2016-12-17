using UnityEngine;
using System.Collections;

// Throwing mechanic of the game. Applied to throwable objects.
public class GnomeController : Throwable 
{

	public Animator anim;

	private bool firstActiveFrame = true;
	private bool firstLanding = true;

	new public void Start()
	{
		base.Start ();
		//Sometimes the mushroom spores knock the gnome out of the way, so we're going to try to prevent that
		rgdbdy.freezeRotation = true; 
		if (transform.GetComponentInChildren<Animator> ()) {
			anim = transform.GetComponentInChildren<Animator> (); 
		}
	}

	// What happens when the throw is started.
	new public void BeginDragHandler() 
	{
		rgdbdy.freezeRotation = false;
		base.BeginDragHandler ();
		if (!thrown) {
			// Picked up sound
			EffectHandler.playSoundOnGrab(this.gameObject);
		}
		if (anim) {
			anim.SetBool("grabbed",true);
		}
	}

	// End of throw. (click released)
	new public void EndDragHandler() 
	{
		if (!thrown) {
			// Thrown sound
			EffectHandler.playSoundOnThrow (this.gameObject);
		}
		base.EndDragHandler ();
	}

	void Update() 
	{
		// Because the cube is at rest (velocity 0) for just a frame before the force is applied to it,
		// we need a countdown for the gnome to come to rest before it counts as 'landed'.
		if (thrown && GetComponent<Rigidbody> ().velocity.magnitude < 0.3f && restCount > 0) {
			restCount--;
		}
		if (restCount <= 0) {
			StopMove ();
			rgdbdy.freezeRotation = false;
			if (anim) {
				anim.SetBool ("grabbed", false);
			}
			if (firstLanding) {
				EffectHandler.playSoundOnLand (this.gameObject);
				firstLanding = false;
				Invoke("makeAComment", (Random.value*10)+5); //randomly play a comment betweeen 5 and 15 seconds after landing
			}
		}
		if (firstActiveFrame) {
			firstActiveFrame = false;
			//EffectHandler.playSoundOnGrab (this.gameObject); //play on grab speech on appearance instead
		}
	}

	private void makeAComment() {
		EffectHandler.playComment (this.gameObject);
	}

	void OnCollisionEnter(Collision collision) {
		restCount = 10;
		//collide sound
		if (thrown) {
			/*if(collision.gameObject.name.Equals("Mushroom(Clone)")) {
				SoundPlayer.PlaySound(bounceSound,transform.position);
				print ("boing!");
			}
			else {
			SoundPlayer.PlaySound (impactSound, transform.position);
			}*/
			EffectHandler.playCollisionEffects (this.gameObject, collision);
		}
	}
}
