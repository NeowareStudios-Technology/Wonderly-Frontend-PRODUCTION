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

	public Text chosenModelText;
	public Text chosenVideoText;
	public Text chosenImageText;
	public GameObject loadingPanel;

	//public GameObject rotateButton1;
	//public Text rotateButton1Text;
	//public GameObject rotateButton2;

	public Text description;

	//for sizing Summary screen depending on target number
	public RectTransform wonderPanelRectTrans;
	public Vector2 noWondersHeight = new Vector2(0,1600);
	public Vector2 oneWondersHeight= new Vector2(0,1850);
	public Vector2 twoWondersHeight =  new Vector2(0,2550);
	public Vector2 fullWondersHeight = new Vector2(0,3200);

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
	//uses summaryAnimator

	//for checking input on wonder title screen (edit flow)
	public InputField wonderTitleInput2;
	public GameObject wonderInputError2;
	//uses summaryAnimator

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
		screenCapBorder.sizeDelta = new Vector2 (Screen.width/2+255, Screen.height/2+380);
		//screenCapBorder.anchoredPosition = new Vector2(Screen.width / 4, Screen.height / 3);

	}

	public void SetLoadingPanelActive(bool settingActive){
		if (settingActive){
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

	public void clearUserSettingsInputFields()
	{
		firstName.text = "";
		lastName.text = "";
		currentPassword.text = "";
		newPassword.text = "";
		newMatchingPassword.text = "";
	}
}

