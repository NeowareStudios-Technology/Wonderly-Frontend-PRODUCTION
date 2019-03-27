/******************************************************
*Project: Wonderly
*Created by: David Lee Ramirez
*Date: 12/28/18
*Description: Used for edit wonder info flow
*Copyright 2018 LeapWithAlice,LLC. All rights reserved
 ******************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class editWonderTitleDescription : MonoBehaviour {

	public SaveManager sm;
	public PanelController pc;
	public GameObject editWonderInfoPanel;
	public InputField wonderTitle;
	public InputField wonderDescription;
	public Animator editWonderInfoAnimator;

	//opens the SetupWonder SINGLE screen and tells it which target to apply changes to
	public void openEditWonderInfoScreen(int whichTarget)
	{
	  pc.OpenPanel(editWonderInfoAnimator);
		editWonderInfoPanel.GetComponent<whichTargetHolder>().whichTarget = whichTarget;
	}

	//saves the title and description of the wonder locally
	public void SaveTitleDesc()
	{
		sm.LocalSaveWonderTitleDescManualIndex(editWonderInfoPanel.GetComponent<whichTargetHolder>().whichTarget);
	}

	//clears input fields in SetupWonder screen after delay
	public void startDelayedClearInputFields()
	{
		StartCoroutine("delayedClearInputFields");
	}
	public IEnumerator delayedClearInputFields()
	{
		yield return new WaitForSeconds(2);
		wonderTitle.text = "";
		wonderDescription.text = "";
	}
}
