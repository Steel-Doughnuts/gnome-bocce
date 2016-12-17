using UnityEngine;
using System.Collections;

public class ScoreManager : MonoBehaviour {

	private static string player1Key = "player1Score";
	private static string player2Key = "playe2Score";

	private static int MAX_SCORE = 11;

	// Calculates the score based on the distance of each player's gnomes from the pallena.
	public static void CalculateScore(GnomeManager game, ArrayList player1, ArrayList player2) 
	{
		// The closest throw to the pallena by each player.
		float p1Nearest = getNearestGnomeDist (player1, game.pallena);
		float p2Nearest = getNearestGnomeDist (player2, game.pallena);

		// Find who wins the frame and display the points that they won by.
		if (p1Nearest > p2Nearest) {
			int winPoints = winnerPoints (player2, p1Nearest, game.pallena);
			string points = getPluralPoints (winPoints);
			ScoreManager.addToPlayerScore (player2Key, winPoints);
			game.winText.text = "Player2 (Blue) scored " + winPoints + " " + points + " this round!";
		} else {
			// Pssst!  You'll notice that Player1 sort of wins ties right now, but we consider a tie to be INCREDIBLY unlikely!
			int winPoints = winnerPoints (player1, p2Nearest, game.pallena);
			string points = getPluralPoints (winPoints);
			ScoreManager.addToPlayerScore (player1Key, winPoints);
			game.winText.text = "Player1 (Red) scored " + winPoints + " " + points + " this round!";
		}

		// A winner has been found.
		game.winText.gameObject.SetActive (true);
		game.scoreAnnounced = true;
	}

	private static string getPluralPoints(int winnerPoints) {
		return winnerPoints == 1 ? "point" : "points";
	}

	// Finds the nearest thrown gnome by a player.
	// Returns the distance of the gnome nearest to the pallena.
	private static float getNearestGnomeDist(ArrayList player, Throwable pallena) 
	{
		float nearestDist = float.MaxValue;

		// Iterates through all of the gnomes and finds the smallest distance from the pallena.
		foreach(Throwable g in player) {
			float gDist = Vector3.Distance (pallena.gameObject.transform.position, g.gameObject.transform.position);
			if (gDist < nearestDist) {
				nearestDist = gDist;
			}
		}
		return nearestDist;
	}

	// Number of points won by a player
	// Player is the player who won the game, distance represents the closest gnome by the losing player.
	// According to bocce rules, the winning player scores points equal to all balls (or in this case gnomes)
	// closer to the pallena than the losing player's closest ball (gnome).
	//
	// Returns the amount of points the winning player scored.
	private static int winnerPoints(ArrayList player, float distance, Throwable pallena)
	{
		int points = 0;

		// Iterates through all of the winning player's gnome distances and compares them to the losing player's
		// closest throw. A point is won for every throw that has a distance smaller than the closest throw by the 
		// losing player
		foreach(Throwable g in player) {
			float gDist = Vector3.Distance (pallena.gameObject.transform.position, g.gameObject.transform.position);
			if (gDist < distance) {
				points++;
			}
		}
		return points;
	}

	public void GoToNextRound() {
		GnomeManager.GoToNextRound ();
	}

	public void BeingNewGame() {
		ScoreManager.resetScores ();
		GnomeManager.GoToNextRound ();
	}

	public void ReturnToMainMenu() {
		ScoreManager.resetScores ();
		UnityEngine.SceneManagement.SceneManager.LoadScene ("Scenes/MainMenu");
	}

	public static bool GameIsOver() {
		return getPlayer1Score () >= MAX_SCORE || getPlayer2Score () >= MAX_SCORE;
	}

	public static string GetGameOverText() {
		string winner = GetWinnerText ();
		return "The game is over." + " " + winner;
	}

	private static string GetWinnerText () {
		int p1Score = getPlayer1Score ();
		int p2Score = getPlayer2Score ();
		if (p1Score == p2Score) {
			return "It was a tie.";
		}
		return p1Score > p2Score ? "Player 1 (Red) won." : "Player 2 (Blue) won.";
	}

	public static int getPlayer1Score() {
		return getScore (player1Key);
	}

	public static int getPlayer2Score() {
		return getScore (player2Key);
	}

	private static void addToPlayerScore(string key, int adding) {
		PlayerPrefs.SetInt(key, PlayerPrefs.GetInt(key) + adding);
	}

	public static void resetScores() {
		PlayerPrefs.SetInt (player1Key, 0);
		PlayerPrefs.SetInt (player2Key, 0);
	}

	private static int getScore(string key) {
		return PlayerPrefs.GetInt (key);
	}
}
