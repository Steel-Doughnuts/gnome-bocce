using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ReplayManager : MonoBehaviour {

	private class GameObjectInfo {
		public Vector3 position;
		public Quaternion rotation;
		public bool active;
	}

	private static List<GameObjectInfo>[] infos = new List<GameObjectInfo>[13];

	private static int gnomeIndex = 0;
	private static bool replaying = false;
	private static int numFrames = -1;
	public static Vector3 initialCameraLocation;

	public static void AddFrame(GnomeManager game) {
		if (infos [0] == null || gnomeIndex != game.throwableIndex) {
			ReplayManager.resetAll ();
			initialCameraLocation = Camera.main.transform.position;
			gnomeIndex = game.throwableIndex;
		}

		infos[0].Add (createInfo(game.flowers[0].gameObject));
		infos[1].Add (createInfo(game.flowers[1].gameObject));
		infos[2].Add (createInfo(game.flowers[2].gameObject));
		infos[3].Add (createInfo(game.pallena.gameObject));
		infos[4].Add (createInfo(game.mushroom1.gameObject));
		infos[5].Add (createInfo(game.mushroom2.gameObject));
		infos[6].Add (createInfo(game.mushroom3.gameObject));
		infos[7].Add (createInfo(game.p1gnome1.gameObject));
		infos[8].Add (createInfo(game.p2gnome1.gameObject));
		infos[9].Add (createInfo(game.p1gnome2.gameObject));
		infos[10].Add (createInfo(game.p2gnome2.gameObject));
		infos[11].Add (createInfo(game.p1gnome3.gameObject));
		infos[12].Add (createInfo(game.p2gnome3.gameObject));
	}

	private static GameObjectInfo createInfo(GameObject gameObject) {
		GameObjectInfo temp = new GameObjectInfo ();
		temp.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
		temp.rotation = gameObject.transform.rotation;
		temp.active = gameObject.activeSelf;
		return temp;
	}

	public static void resetAll() {
		for(int i = 0; i < infos.Length; i++) {
			infos[i] = new List<GameObjectInfo>();
		}
	}

	public static bool replayOneFrame (GnomeManager game) {
		if(!replaying) {
			numFrames = infos[3].Count;
			replaying = true;

		}
		replaying = game.replayFrameIndex < numFrames;
		if (replaying) {
			updateTransform (game.pallena.gameObject, infos [3] [game.replayFrameIndex]);
			updateTransform (game.mushroom1.gameObject, infos [4] [game.replayFrameIndex]);
			updateTransform (game.mushroom2.gameObject, infos [5] [game.replayFrameIndex]);
			updateTransform (game.mushroom3.gameObject, infos [6] [game.replayFrameIndex]);
			updateTransform (game.p1gnome1.gameObject, infos [7] [game.replayFrameIndex]);
			updateTransform (game.p2gnome1.gameObject, infos [8] [game.replayFrameIndex]);
			updateTransform (game.p1gnome2.gameObject, infos [9] [game.replayFrameIndex]);
			updateTransform (game.p2gnome2.gameObject, infos [10] [game.replayFrameIndex]);
			updateTransform (game.p1gnome3.gameObject, infos [11] [game.replayFrameIndex]);
			updateTransform (game.p2gnome3.gameObject, infos [12] [game.replayFrameIndex]);
		} else {
			infos [0] = null;
			Camera.main.transform.position = initialCameraLocation;
		}
		return replaying;
	}

	private static void updateTransform(GameObject gameObject, GameObjectInfo info) {
		gameObject.transform.position = info.position;
		gameObject.transform.rotation = info.rotation;
		gameObject.SetActive (info.active);
	}

	public void beginReplay (){
		GnomeManager.setToReplaying ();
	}

	public static void showReplayButton (Button replayButton) {
		replayButtonActive (replayButton, true);
	}

	public static void hideReplayButton (Button replayButton) {
		replayButtonActive (replayButton, false);
	}

	private static void replayButtonActive(Button replayButton, bool active) {
		replayButton.gameObject.SetActive (active);
	}
}
