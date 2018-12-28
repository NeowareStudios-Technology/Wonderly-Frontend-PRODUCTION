using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sample;

public class CreateOrEditController : MonoBehaviour {

public Button saveJourneyButton;
public FilesManager fm;
public SaveManager sm;

public PanelController pc;
public Animator shareScreenAnimator;
public GameObject backgroundImage;

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
		}
  }

}
