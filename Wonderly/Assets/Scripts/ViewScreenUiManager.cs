﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sample;

public class ViewScreenUiManager : MonoBehaviour {

	//used get what target is currently being viewed
	public ImageTargetManager itm;
	//used to get each wonder's title and description
	public LoadManager lm;

	public Text wonderTitleDisplay;
	public Text wonderDescriptionDisplay;
	public GameObject wonderTitle;
	public GameObject wonderDescription;

	bool isCurrentTargetStillActive = false;
	int currentViewedTarget = 0;
	
	//determines what wonder (target) is being viewed and displays corresponding title and description on screen
	void Update () {

		//sets the currentTargetStillActive variable to the previously set target's active status
		switch(currentViewedTarget)
		{
				//if no target was set, force function to execute logic to check targets by setting to false
				case 0:
						isCurrentTargetStillActive = false;
						break;
				case 1:
						isCurrentTargetStillActive = itm.activeTarget1;
						break;
				case 2:
						isCurrentTargetStillActive = itm.activeTarget2;
						break;
				case 3:
						isCurrentTargetStillActive = itm.activeTarget3;
						break;
				case 4:
						isCurrentTargetStillActive = itm.activeTarget4;
						break;
				case 5:
						isCurrentTargetStillActive = itm.activeTarget5;
						break;
		}

		//this if statement ensures that the logic for changing targets only executes if the current set target gets deactivated
		//-sets the currentViewedTarget variable to the target currently being viewed
		if (isCurrentTargetStillActive == false)
		{
				wonderTitle.SetActive(false);
				wonderDescription.SetActive(false);

				if (itm.activeTarget1 == true)
				{
						currentViewedTarget = 1;
				}
				else if (itm.activeTarget2 == true)
				{
						currentViewedTarget = 2;
				}
				else if (itm.activeTarget3 == true)
				{
						currentViewedTarget = 3;
				}
				else if (itm.activeTarget4 == true)
				{
						currentViewedTarget = 4;
				}
				else if (itm.activeTarget5 == true)
				{
						currentViewedTarget = 5;
				}
				//if no targets are active, set to 0 so that function will execute target activity checking logic upon next call
				else
				{
						currentViewedTarget = 0;
						return;
				}
		}
		//do not execute further if last target set for title/description display is still being viewed
		else
			return;


		//below is the logic for displaying wonder title/descrption depending on what target is being viewed
		switch(currentViewedTarget)
		{
			case 0:
				wonderTitleDisplay.text = " ";
				wonderDescriptionDisplay.text = " ";
				wonderTitle.SetActive(false);
				wonderDescription.SetActive(false);
				break;
			case 1:
				wonderTitleDisplay.text = lm.scd.wonderTitle[0];
				wonderDescriptionDisplay.text = lm.scd.wonderDescription[0];
				wonderTitle.SetActive(true);
				break;
			case 2:
				wonderTitleDisplay.text = lm.scd.wonderTitle[1];
				wonderDescriptionDisplay.text = lm.scd.wonderDescription[1];
				wonderTitle.SetActive(true);
				break;
			case 3:
				wonderTitleDisplay.text = lm.scd.wonderTitle[2];
				wonderDescriptionDisplay.text = lm.scd.wonderDescription[2];
				wonderTitle.SetActive(true);
				break;
			case 4:
				wonderTitleDisplay.text = lm.scd.wonderTitle[3];
				wonderDescriptionDisplay.text = lm.scd.wonderDescription[3];
				wonderTitle.SetActive(true);
				break;
			case 5:
				wonderTitleDisplay.text = lm.scd.wonderTitle[4];
				wonderDescriptionDisplay.text = lm.scd.wonderDescription[4];
				wonderTitle.SetActive(true);
				break;
		}

	}
}