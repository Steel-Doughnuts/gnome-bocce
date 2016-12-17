using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class HelpButtons : MonoBehaviour {

	public Button WhatIsGnomeBocceButton;
	public Button HowToPlayButton;
	public Button AboutFlowersButton;
	public Button AboutGnomesButton;
	public Button CallibratingButton;
	public Button CloseButton;
	public Button BackButton;

	public RawImage WhatIsGnomeBoccePopUp;
	public RawImage HowToPlayPopUp;
	public RawImage AboutFlowersPopUp;
	public RawImage AboutGnomesPopUp;
	public RawImage CallibratingPopUp;


	// Use this for initialization
	void Start () {
		WhatIsGnomeBoccePopUp.gameObject.SetActive(false);
		HowToPlayPopUp.gameObject.SetActive(false);
		AboutFlowersPopUp.gameObject.SetActive(false);
		AboutGnomesPopUp.gameObject.SetActive(false);
		CallibratingPopUp.gameObject.SetActive(false);
		CloseButton.gameObject.SetActive (false); 
	
	}

	private void HideButtons()
	{
		BackButton.gameObject.SetActive (false);
		WhatIsGnomeBocceButton.gameObject.SetActive(false); 
		HowToPlayButton.gameObject.SetActive(false); 
		AboutGnomesButton.gameObject.SetActive(false); 
		AboutFlowersButton.gameObject.SetActive(false); 
		CallibratingButton.gameObject.SetActive(false); 
	}

	private void ShowButtons()
	{
		BackButton.gameObject.SetActive (true);
		WhatIsGnomeBocceButton.gameObject.SetActive(true); 
		HowToPlayButton.gameObject.SetActive(true); 
		AboutGnomesButton.gameObject.SetActive(true); 
		AboutFlowersButton.gameObject.SetActive(true); 
		CallibratingButton.gameObject.SetActive(true); 
	}



	public void WhatisGnomeBocce()
	{
		HideButtons ();
		WhatIsGnomeBoccePopUp.gameObject.SetActive(true); 
		CloseButton.gameObject.SetActive (true);
	}

	public void HowToPlay()
	{
		HideButtons ();
		HowToPlayPopUp.gameObject.SetActive(true); 
		CloseButton.gameObject.SetActive (true);
	}

	public void AboutTheGnomes()
	{
		HideButtons ();
		AboutGnomesPopUp.gameObject.SetActive(true); 
		CloseButton.gameObject.SetActive (true);
	}

	public void AboutTheFlowers()
	{
		HideButtons ();
		AboutFlowersPopUp.gameObject.SetActive(true); 
		CloseButton.gameObject.SetActive (true);
	}

	public void CallibratingGameSpace()
	{
		HideButtons ();
		CallibratingPopUp.gameObject.SetActive(true); 
		CloseButton.gameObject.SetActive (true);
	}


	//closing the windows
	public void AllClose()
	{
		WhatIsGnomeBoccePopUp.gameObject.SetActive(false);
		HowToPlayPopUp.gameObject.SetActive(false);
		AboutFlowersPopUp.gameObject.SetActive(false);
		AboutGnomesPopUp.gameObject.SetActive(false);
		CallibratingPopUp.gameObject.SetActive(false);
		CloseButton.gameObject.SetActive (false);
		ShowButtons ();
	}
	

}
