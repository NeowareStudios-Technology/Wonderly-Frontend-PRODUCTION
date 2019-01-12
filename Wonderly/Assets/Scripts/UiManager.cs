/******************************************************
*Project: Wonderly
*Created by: David Lee Ramirez
*Date: 12/28/18
*Description:  Handles resetting of ViewLibraryContent, NewAddViewContent
							screen, CompletejourneyCopy screen, and User Settings 
							screen. Handles error handling and displaying of error 
							message for Journey/Wonder title/description required. 
*Copyright 2018 LeapWithAlice,LLC. All rights reserved
 ******************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PolyToolkit;


public class UiManager : MonoBehaviour {
	//other script references
	public FilesManager fm;
	public FirebaseManager fbm;
	public SaveManager sm;
	public ArPairDisplayManager apdm;
	public ModelInitializer mi;
	public PanelController pc;
	//for activating/deactivating loading screen
	public GameObject loadingPanel;
	//for clearing ViewLibraryContent screen
	public Text contentAttribs;
	public Image contentImage;
	//for clearing ViewLibraryContent screen (Edit Flow)
	public Text contentAttribs2;
	public Image contentImage2;
	//for clearing preview screen
	public Text previewTitle;
	public Text previewDescription;
	public Text[] previewWonderTitles = new Text[5];
	public Text[] previewWonderDescriptions = new Text[5];
	public Image[] previewImages = new Image[5];
	public Image previewCoverImage;
	//for clearing summary screen
	public Text summaryTitle;
	public Text summaryDescription;
	public Text[] summaryWonderTitles = new Text[5];
	public Text[] summaryWonderDescriptions = new Text[5];
	public Image[] summaryTargetImages = new Image[5];
	public Image[] summaryLinkedImages = new Image[5];
	public Image summaryCoverImage;
	//for checking input on journey title screen (main flow)
	public InputField titleInput1;
	public GameObject journeyInputError1;
	public Animator scanWonderAnimator;
	//for checking input on journey title screen (edit flow)
	public InputField titleInput2;
	public GameObject journeyInputError2;
	public Animator summaryAnimator;
	//for checking input on wonder title screen (main flow)
	public InputField wonderTitleInput1;
	public GameObject wonderInputError1;
	//for checking input on wonder title screen (edit flow)
	public InputField wonderTitleInput2;
	public GameObject wonderInputError2;
	//for setting either a blank sprite or a Wonderly icon sprite
	public Image blankImage;
	public Image iconImage;
	//for clearing input fields on UserSettings screen
	public InputField firstName;
	public InputField lastName;
	public InputField currentPassword;
	public InputField newPassword;
	public InputField newMatchingPassword;
	//for dynamically sized/created screen shot view
	public RectTransform screenCapBorder;


	//determine screenshot size by size of screen
	void Awake() 
	{
	}


	//turns on loading screen
	public void SetLoadingPanelActive(bool settingActive){
		if (settingActive){
			Debug.Log("starting loading panel");
			loadingPanel.SetActive(true);
			loadingPanel.transform.parent.gameObject.SetActive(true);
		}
		else{
			loadingPanel.SetActive(false);
			loadingPanel.transform.parent.gameObject.SetActive(false);
		}
	}


	//delayed reset so that UI doesnt look bad with instant reset
	//called by ViewLibraryContent back button
	public void startResetViewLibraryContent()
	{
		StartCoroutine("ResetViewLibraryContent");
	}


	//resets the ViewLibraryContent screen after a 2 sec delay
	public IEnumerator ResetViewLibraryContent()
	{
		yield return new WaitForSeconds(2);
		contentAttribs.text = "  ";
		contentImage.sprite = blankImage.sprite;
	}

	//called by CompleteJourneyCopy add new wonder button
	public void instantResetViewLibraryContent()
	{
		contentAttribs.text = "  ";
		contentImage.sprite = blankImage.sprite;
	}


	//EDIT FLOW: delayed reset so that UI doesnt look bad with instant reset
	//called by ViewLibraryContent back button
	public void startResetViewLibraryContent2()
	{
		StartCoroutine("ResetViewLibraryContent2");
	}


	public IEnumerator ResetViewLibraryContent2()
	{
		yield return new WaitForSeconds(2);
		contentAttribs2.text = "  ";
		contentImage2.sprite = blankImage.sprite;
	}


	//delayed reset so that UI doesnt look bad with instant reset
	public void startResetPreviewScreen()
	{
		StartCoroutine("ResetPreviewScreen");
	}


	//resets preview screen (NewAddViewContent) after 2 sec delay
	public IEnumerator ResetPreviewScreen()
	{
		yield return new WaitForSeconds(2);
		previewTitle.text = " ";
		previewDescription.text = " ";
		previewCoverImage.sprite = iconImage.sprite;
		for(int i = 0; i<5; i++)
		{
			previewImages[i].sprite = iconImage.sprite;
			previewWonderTitles[i].text = " ";
			previewWonderDescriptions[i].text = " ";
		}
	}


	//instantly resets preview screen (NewAddViewContent)
	public void InstantResetPreviewScreen()
	{
		previewTitle.text = " ";
		previewDescription.text = " ";
		previewCoverImage.sprite = iconImage.sprite;
		for(int i = 0; i<5; i++)
		{
			previewImages[i].sprite = iconImage.sprite;
			previewWonderTitles[i].text = " ";
			previewWonderDescriptions[i].text = " ";
		}
	}


	//instantly resets summary screen (CompleteJourneyCopy)
	public void InstantResetSummaryScreen()
	{
		summaryTitle.text = " ";
		summaryDescription.text = " ";
		summaryCoverImage.sprite = iconImage.sprite;
		for(int i = 0; i<5; i++)
		{
			summaryLinkedImages[i].sprite = iconImage.sprite;
			summaryTargetImages[i].sprite = iconImage.sprite;
			summaryWonderTitles[i].text = " ";
			summaryWonderDescriptions[i].text = " ";
		}
	}


	//makes sure journey has title, error message if not
	public void EnsureJourneyTitleInput1()
	{
		if (titleInput1.text == "")
		{
			journeyInputError1.SetActive(true);
		}
		else 
		{
			pc.OpenPanel(scanWonderAnimator);
		}
	}


	//makes sure journey has title, error message if not EDIT FLOW
	public void EnsureJourneyTitleInput2()
	{
		if (titleInput2.text == "")
		{
			journeyInputError2.SetActive(true);
		}
		else 
		{
			pc.OpenPanel(summaryAnimator);
		}
	}


	//makes sure wonder has title, error message if not
	public void EnsureWonderTitleInput1()
	{
		if (wonderTitleInput1.text == "")
		{
			wonderInputError1.SetActive(true);
		}
		else 
		{
			pc.OpenPanel(summaryAnimator);
		}
	}


	//makes sure wonder has title, error message if not EDIT FLOW
	public void EnsureWonderTitleInput2()
	{
		if (wonderTitleInput2.text == "")
		{
			wonderInputError2.SetActive(true);
		}
		else 
		{
			pc.OpenPanel(summaryAnimator);
		}
	}


	//clears the user settings screen
	public void clearUserSettingsInputFields()
	{
		firstName.text = "";
		lastName.text = "";
		currentPassword.text = "";
		newPassword.text = "";
		newMatchingPassword.text = "";
	}
}

