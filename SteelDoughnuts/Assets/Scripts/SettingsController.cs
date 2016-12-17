using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SettingsController : Settings {

	public Slider aiLevelSlider;
	public Button Music_On;
	public Button Music_Off;
	public Button SFX_On;
	public Button SFX_Off;
	public Button AR_On;
	public Button AR_Off;

	public Texture OFF_OFF;
	public Texture OFF_ON;
	public Texture ON_OFF;
	public Texture ON_ON;

	public void Start ()
	{
		if (Music_On != null) {
			if (Settings.ShouldPlayMusic ()) {
				SetMusicTrue ();
			} else {
				SetMusicFalse ();
			}
		}

		if (SFX_On != null) {
			if (Settings.ShouldPlaySFX ()) { 
				SetSoundEffectsTrue ();
			} else {
				SetSoundEffectsFalse ();
			}
		}

		if(AR_On != null) {
			if (Settings.ShouldPlayAR ()) {
				SetARTrue ();
			} else {
				SetARFalse ();
			}
		}

		if (aiLevelSlider != null) {
			aiLevelSlider.value = Settings.AIProbability ();
		}
	}

	public void SetARTrue()
	{
		AR_On.GetComponentInChildren<RawImage>().texture = ON_ON;
		AR_Off.GetComponentInChildren<RawImage>().texture = OFF_OFF;
		SetAR (true);
		if (aiLevelSlider != null) {
			aiLevelSlider.value = 0;
			SetNewAIProbability ();
		}
	}

	public void SetARFalse() 
	{
		AR_On.GetComponentInChildren<RawImage>().texture = ON_OFF;
		AR_Off.GetComponentInChildren<RawImage>().texture = OFF_ON;
		SetAR (false);
	}

	private void SetAR (bool value)
	{
		PlayerPrefs.SetInt (shouldPlayARKey, BoolToInt(value));
	}
		
	public void SetMusicTrue() 
	{
		Music_On.GetComponentInChildren<RawImage>().texture = ON_ON;
		Music_Off.GetComponentInChildren<RawImage>().texture = OFF_OFF;
		SetMusic (true);
	}

	public void SetMusicFalse() 
	{
		Music_On.GetComponentInChildren<RawImage>().texture = ON_OFF;
		Music_Off.GetComponentInChildren<RawImage>().texture = OFF_ON;
		SetMusic (false);
	}

	private void SetMusic (bool value)
	{
		PlayerPrefs.SetInt (shouldPlayMusicKey, BoolToInt(value));
	}

	public void SetSoundEffectsTrue() 
	{
		SFX_On.GetComponentInChildren<RawImage>().texture = ON_ON;
		SFX_Off.GetComponentInChildren<RawImage>().texture = OFF_OFF;
		SetSoundEffects (true);
	}

	public void SetSoundEffectsFalse() 
	{
		SFX_On.GetComponentInChildren<RawImage>().texture = ON_OFF;
		SFX_Off.GetComponentInChildren<RawImage>().texture = OFF_ON;
		SetSoundEffects (false);
	}

	private void SetSoundEffects (bool value)
	{
		PlayerPrefs.SetInt (shouldPlaySFXKey, BoolToInt(value));
	}

	public void SetNewAIProbability ()
	{
		if (Settings.ShouldPlayAR ()) {
			aiLevelSlider.value = 0;
		} else {
			PlayerPrefs.SetFloat (aiProbabilityKey, aiLevelSlider.value);
		}
	}

	private int BoolToInt(bool original)
	{
		return original ? 1 : 0;
	}
}
