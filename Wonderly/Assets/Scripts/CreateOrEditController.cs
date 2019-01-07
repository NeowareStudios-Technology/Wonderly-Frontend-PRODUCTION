/******************************************************
*Project: Wonderly
*Created by: David Lee Ramirez
*Date: 12/28/18
*Description: Determines if the user is creating a new journey or editing 
*							an existing journey and changes save button functionality 
*							accordingly.
*Copyright 2018 LeapWithAlice,LLC. All rights reserved
 ******************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateOrEditController : MonoBehaviour {
//script references
public FilesManager fm;
public SaveManager sm;
//for changing screens
public PanelController pc;
public Animator shareScreenAnimator;
public Animator summaryScreenAnimator;
public Animator createJourneyScreenAnimator;
//for activating or deactivating purple background image
public GameObject backgroundImage;
//for changing function of button based on create flow or edit flow
public Button saveJourneyButton;
//for changing function of button based on create flow or edit flow
public Button backButton;

	//make sure entry into create flow sets createOrEdit to "create"
	//make sure entry into edit flow sets createOrEdit to "edit"

	//changes "save" button functionality depending on whether the user is editing or creating journey
	//-needed because edit requires the old journey to be deleted
	public void setCreateOrEdit(string createOrEdit)
  {
    if (createOrEdit == "create")
		{
			//clear button functonality
			saveJourneyButton.onClick.RemoveAllListeners();
			//add "create" save functionality to button
			saveJourneyButton.onClick.AddListener(delegate {sm.CreateSaveFile(); });
			saveJourneyButton.onClick.AddListener(delegate {pc.OpenPanel(shareScreenAnimator); });
			saveJourneyButton.onClick.AddListener(delegate {backgroundImage.SetActive(true); });
			saveJourneyButton.onClick.AddListener(delegate {fm.arCamera.SetActive(false); });
			//clear button functionality
			backButton.onClick.RemoveAllListeners();
			//add "create" back button functionality to button
			backButton.onClick.AddListener(delegate {pc.OpenPanel(createJourneyScreenAnimator); });
			backButton.onClick.AddListener(delegate {fm.arCamera.SetActive(false); });

		}
		else if (createOrEdit == "edit")
		{
			//clear button functonality
			saveJourneyButton.onClick.RemoveAllListeners();
			//add "edit" save functionality to button
			saveJourneyButton.onClick.AddListener(delegate {sm.CreateSaveFileForEdit(); });
			saveJourneyButton.onClick.AddListener(delegate {pc.OpenPanel(shareScreenAnimator); });
			saveJourneyButton.onClick.AddListener(delegate {backgroundImage.SetActive(true); });
			saveJourneyButton.onClick.AddListener(delegate {fm.arCamera.SetActive(false); });
			//clear button functionality
			backButton.onClick.RemoveAllListeners();
			//add "create" back button functionality to button
			backButton.onClick.AddListener(delegate {pc.OpenPanel(summaryScreenAnimator); });
			backButton.onClick.AddListener(delegate {fm.arCamera.SetActive(false); });
		}
  }

}
