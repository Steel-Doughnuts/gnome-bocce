using UnityEngine;
using System.Collections;

public class EffectHandler : MonoBehaviour {

	public static void playCollisionEffects(GameObject throwable, Collision collision) {
		//print (collision.impulse.magnitude);
		string object1 = getTypeOfObject(throwable.name);
		string object2 = getTypeOfObject(collision.gameObject.name);
		Vector3 location = collision.contacts [0].point;				// contacts is guaranteed to have at least one element (comes from OnCollisionEnter)
		float volume = Mathf.Min(collision.impulse.magnitude/5, 1.0f);  // scale volume of sounds and size of animations

		switch (object1 + " " + object2) {
		//gnome-gnome collisions
		case "pallenaGnome normalGnome":
			SoundPlayer.PlaySound ("Sounds/Impacts/Gnomes/Hitting Other Gnomes/Hitting Normal", location, volume);
			playAnimation ("hitEffect", collision, volume);
			break;
		case "pallenaGnome heavyGnome":
			SoundPlayer.PlaySound ("Sounds/Impacts/Gnomes/Hitting Other Gnomes/Hitting Heavy", location, volume);
			playAnimation ("hitEffect", collision, volume);
			break;
		case "pallenaGnome dartGnome":
			SoundPlayer.PlaySound ("Sounds/Impacts/Gnomes/Hitting Other Gnomes/Hitting Skinny", location, volume);
			playAnimation ("hitEffect", collision, volume);
			break;

		case "normalGnome pallenaGnome":
			break;								//case covered above, avoid playing twice
		case "normalGnome normalGnome":
			SoundPlayer.PlaySound ("Sounds/Impacts/Gnomes/Hitting Other Gnomes/Hitting Normal", location, volume);
			playAnimation ("hitEffect", collision, volume);
			break;
		case "normalGnome heavyGnome":
			SoundPlayer.PlaySound ("Sounds/Impacts/Gnomes/Hitting Other Gnomes/Hitting Heavy", location, volume);
			playAnimation ("hitEffect", collision, volume);
			break;
		case "normalGnome dartGnome":
			SoundPlayer.PlaySound ("Sounds/Impacts/Gnomes/Hitting Other Gnomes/Hitting Skinny", location, volume);
			playAnimation ("hitEffect", collision, volume);
			break;

		case "heavyGnome pallenaGnome":
		case "heavyGnome normalGnome":
			break;								//cases covered above, avoid playing twice
		case "heavyGnome heavyGnome":
			SoundPlayer.PlaySound ("Sounds/Impacts/Gnomes/Hitting Other Gnomes/Hitting Heavy", location, volume);
			playAnimation ("hitEffect", collision, volume);
			break;
		case "heavyGnome dartGnome":
			SoundPlayer.PlaySound ("Sounds/Impacts/Gnomes/Hitting Other Gnomes/Hitting Skinny", location, volume);
			playAnimation ("hitEffect", collision, volume);
			break;

		case "dartGnome pallenaGnome":
		case "dartGnome normalGnome":
		case "dartGnome heavyGnome":
			break;								//cases covered above, avoid playing twice
		case "dartGnome dartGnome":
			SoundPlayer.PlaySound ("Sounds/Impacts/Gnomes/Hitting Other Gnomes/Hitting Skinny", location, volume);
			playAnimation ("hitEffect", collision, volume);
			break;

		//gnome-mushroom collisions
		case "pallenaGnome mushroom":
		case "normalGnome mushroom":
		case "heavyGnome mushroom":
		case "dartGnome mushroom":
			SoundPlayer.PlaySound ("Sounds/Impacts/Gnomes/Mushrooms/Mushroom", location, volume);
			playAnimation ("hitEffect", collision, volume);
			break;

		//gnome-fower collisions
		case "pallenaGnome flowerWall":
		case "normalGnome flowerWall":
		case "heavyGnome flowerWall":
		case "dartGnome flowerWall":

		case "pallenaGnome flowerFan":
		case "normalGnome flowerFan":
		case "heavyGnome flowerFan":
		case "dartGnome flowerFan":
			SoundPlayer.PlaySound ("Sounds/Impacts/Gnomes/Wall/Wall", location, volume);
			playAnimation ("hitEffect", collision, volume);
			break;

		/*case "pallenaGnome flowerBomb":
		case "normalGnome flowerBomb":
		case "heavyGnome flowerBomb":
		case "dartGnome flowerBomb":
		case "mushroomSeed flowerBomb":
		case "flowerFan flowerBomb":
		case "flowerWall flowerBomb":
			SoundPlayer.PlaySound ("Sounds/Impacts/Gnomes/Explosion/Explosion", location);
			playAnimation ("explodeEffect", collision, 3);
			break;*/

		//gnome-floor collisions
		case "pallenaGnome floor":
		case "normalGnome floor":
			SoundPlayer.PlaySound ("Sounds/Impacts/Gnomes/Ground/Normal Skinny Target Impact", location, volume);
			playAnimation ("poofEffect", collision, volume);
			break;
		case "heavyGnome floor":
			SoundPlayer.PlaySound ("Sounds/Impacts/Gnomes/Ground/Heavy Impact", location, volume);
			playAnimation ("poofEffect", collision, volume);
			break;
		case "dartGnome floor":
			SoundPlayer.PlaySound ("Sounds/Impacts/Gnomes/Ground/Normal Skinny Target Impact", location, volume);
			playAnimation ("poofEffect", collision, volume);
			break;

		//gnome-wall collisions
		case "pallenaGnome wall":
		case "normalGnome wall":
		case "heavyGnome wall":
		case "dartGnome wall":

		//gnome-ceiling collisions
		case "pallenaGnome ceiling":
		case "normalGnome ceiling":
		case "heavyGnome ceiling":
		case "dartGnome ceiling":
			//SoundPlayer.PlaySound (impactSound, location, volume);
			//playAnimation ("hitEffect", collision, volume);
			break;

		//gnome-windTunnel collisions
		case "pallenaGnome windTunnel":
		case "normalGnome windTunnel":
		case "heavyGnome windTunnel":
		case "dartGnome windTunnel":
			
		
		//flower will only come first as a seed
		//flower collisions
		case "flower floor":
		case "flower wall":
		case "flower ceiling":
			SoundPlayer.PlaySound ("Sounds/Impacts/Items/Item Plant", location, volume);
			break;
			
		default:
			//for debuging
			//print ("unhandled collision: " + throwable.name + ":" + object1 + " " + collision.gameObject.name + ":" + object2);
			break;
		}
	}

	public static void playSoundOnGrab(GameObject that) {
		Vector3 location = that.transform.position;
		string name = getTypeOfObject (that.name);
		switch (name) {
		case "pallenaGnome":
			SoundPlayer.PlaySound ("Sounds/Voices/Target/Toss Me", location);
			break;
		case "normalGnome":
			SoundPlayer.PlaySound ("Sounds/Voices/Normal/Toss Me", location);
			break;
		case "heavyGnome":
			SoundPlayer.PlaySound ("Sounds/Voices/Fat/Toss Me", location);
			break;
		case "dartGnome":
			SoundPlayer.PlaySound ("Sounds/Voices/Skinny/Toss Me", location);
			break;
		default:
			Debug.Log ("Tried to play sound on appearence of " + that.name + ":" + name);
			break;
		}
	}

	public static void playSoundOnThrow(GameObject that) {
		Vector3 location = that.transform.position;
		string name = getTypeOfObject (that.name);
		switch (name) {
		case "pallenaGnome":
			SoundPlayer.PlaySound ("Sounds/Voices/Target/Tossed", location);
			break;
		case "normalGnome":
			SoundPlayer.PlaySound ("Sounds/Voices/Normal/Tossed", location);
			break;
		case "heavyGnome":
			SoundPlayer.PlaySound ("Sounds/Voices/Fat/Tossed", location);
			break;
		case "dartGnome":
			SoundPlayer.PlaySound ("Sounds/Voices/Skinny/Tossed", location);
			break;
		default:
			Debug.Log ("Tried to play sound on appearence of " + that.name + ":" + name);
			break;
		}
	}

	public static void playSoundOnLand(GameObject that) {
		Vector3 location = that.transform.position;
		string name = getTypeOfObject (that.name);
		switch (name) {
		case "pallenaGnome":
			SoundPlayer.PlaySound ("Sounds/Voices/Target/Cheering", location);
			break;
		case "normalGnome":
			SoundPlayer.PlaySound ("Sounds/Voices/Normal/Cheering", location);
			break;
		case "heavyGnome":
			SoundPlayer.PlaySound ("Sounds/Voices/Fat/Cheering", location);
			break;
		case "dartGnome":
			SoundPlayer.PlaySound ("Sounds/Voices/Skinny/Cheering", location);
			break;
		default:
			Debug.Log ("Tried to play sound on appearence of " + that.name + ":" + name);
			break;
		}
	}

	public static void playComment(GameObject that) {
		Vector3 location = that.transform.position;
		string name = getTypeOfObject (that.name);
		switch (name) {
		case "pallenaGnome":
			SoundPlayer.PlaySound ("Sounds/Voices/Target/Comment", location);
			break;
		case "normalGnome":
			SoundPlayer.PlaySound ("Sounds/Voices/Normal/Comment", location);
			break;
		case "heavyGnome":
			SoundPlayer.PlaySound ("Sounds/Voices/Fat/Comment", location);
			break;
		case "dartGnome":
			SoundPlayer.PlaySound ("Sounds/Voices/Skinny/Comment", location);
			break;
		default:
			Debug.Log ("Tried to play sound on appearence of " + that.name + ":" + name);
			break;
		}
	}

	public static void playWindTunnelEffects(Vector3 location) {
		SoundPlayer.PlaySound ("Sounds/Impacts/Gnomes/Fan/Fan", location);
		playAnimationAtPoint ("windEffect", location, 0.5f);
	}

	public static void playExplosionEffects(Vector3 location) {
		SoundPlayer.PlaySound ("Sounds/Impacts/Gnomes/Explosion/Explosion", location);
		playAnimationAtPoint ("explodeEffect", location, 3);
	}
		
	private static string getTypeOfObject(string name) {
		switch (name) {
		//gnomes
		case "Pallena":
			return "pallenaGnome";
		case "P1G1":
		case "P2G1":
			return "normalGnome";
		case "P1G2":
		case "P2G2":
			return "heavyGnome";
		case "P1G3":
		case "P2G3":
			return "dartGnome";

		//plants
		case "Mushroom(Clone)":
			return "mushroom";
		case "Mushroom Seed":
		case "Mushroom Seed (2)":
		case "Mushroom Seed (3)":
			return "mushroomSeed";
		case "Flower_Wall":
		case "Flower_Wall(Clone)":
		case "Freezy_v4(Clone)":
			return "flowerWall";
		case "Flower_Fan":
		case "Flower_Fan(Clone)":
			return "flowerFan";
		case "Wind Tunnel":
			return "windTunnel";
		case "Flower_Bomb":
		case "Flower_Bomb(Clone)":
			return "flowerBomb";

		//environment
		case "FrontFence":
		case "LeftFence":
		case "RightFence":
		case "BackFence":
		case "FrontWall":
		case "LeftWall":
		case "RightWall":
		case "BackWall":
		case "Backsplash":
			return "wall";
		case "Plane":
			return "floor";
		case "Ceiling":
			return "ceiling";

		//default
		default:
			return name;
		}
	}

	private static void playAnimation(string animationName, Collision collision, float scale=1) {
		if (collision.contacts.Length > 0) {
			GameObject animation = GameObject.Find (animationName);
			GameObject clone = Instantiate (animation, collision.contacts [0].point, collision.transform.rotation) as GameObject;
			clone.transform.localScale = new Vector3(scale,scale,scale);
			Destroy (clone, 10);
		}
	}

	private static void playAnimationAtPoint(string animationName, Vector3 location, float scale=1) {
		GameObject animation = GameObject.Find (animationName);
		GameObject clone = Instantiate (animation, location, new Quaternion(0,0,0,0)) as GameObject;
		clone.transform.localScale = new Vector3(scale,scale,scale);
		Destroy (clone, 10);
	}
}
