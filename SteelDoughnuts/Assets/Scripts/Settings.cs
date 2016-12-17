using UnityEngine;
using System.Collections;

public class Settings : MonoBehaviour {

	private static Settings instance;

	protected static string shouldPlayARKey = "playAR";
	protected static string shouldPlayMusicKey = "playMusic";
	protected static string shouldPlaySFXKey = "playSoundEffects";
	protected static string aiProbabilityKey = "aiProbability";

	private static void Init ()
	{
		if (!PlayerPrefs.HasKey(shouldPlayARKey)) 
		{
			// Initialize to defaults only if there are not any PlayerPrefs at all
			PlayerPrefs.SetInt (shouldPlayARKey, 0);
			PlayerPrefs.SetInt (shouldPlayMusicKey, 1);
			PlayerPrefs.SetInt (shouldPlaySFXKey, 1);
			PlayerPrefs.SetFloat (aiProbabilityKey, 0.7f);
		}
	}

	public static bool ShouldPlayAR ()
	{
		Init ();
		return IntToBool(PlayerPrefs.GetInt(shouldPlayARKey));
	}

	public static bool ShouldPlayMusic ()
	{
		Init ();
		return IntToBool(PlayerPrefs.GetInt(shouldPlayMusicKey));
	}

	public static bool ShouldPlaySFX ()
	{
		Init ();
		return IntToBool(PlayerPrefs.GetInt(shouldPlaySFXKey));
	}

	public static bool ShouldPlayAI() 
	{
		Init ();
		return AIProbability () > 0;
	}

	public static float AIProbability ()
	{
		Init ();
		return PlayerPrefs.GetFloat(aiProbabilityKey);
	}

	private static bool IntToBool (int value) {
		return value != 0;
	}

}
