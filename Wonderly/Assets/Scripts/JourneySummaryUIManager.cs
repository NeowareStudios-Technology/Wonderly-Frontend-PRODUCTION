/******************************************************
*Project: Wonderly
*Created by: David Lee Ramirez
*Date: 12/28/18
*Description:  Handles displaying correct UI elements 
							on the CompleteJourneyCopy (summary) screen.
*Copyright 2018 LeapWithAlice,LLC. All rights reserved
 ******************************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JourneySummaryUIManager : MonoBehaviour {
	//script references
	public FilesManager fm;
	//used to disable button if there are 2 targets
	public GameObject addWonderButton;
	//for display on CompleteJourneyCopy screen
	public GameObject arPair1;
	public GameObject arPair2;
	public GameObject arPair3;
	public GameObject arPair4;
	public GameObject arPair5;
	//for display on CompleteJourneyCopy screen
	public GameObject modelIcon1;
	public GameObject imageIcon1;
	public GameObject videoIcon1;
	public GameObject modelIcon2;
	public GameObject imageIcon2;
	public GameObject videoIcon2;
	public GameObject modelIcon3;
	public GameObject imageIcon3;
	public GameObject videoIcon3;
	public GameObject modelIcon4;
	public GameObject imageIcon4;
	public GameObject videoIcon4;
	public GameObject modelIcon5;
	public GameObject imageIcon5;
	public GameObject videoIcon5;


	//THIS SCRIPT IS LOCATED ON THE CompleteJourneyCopy GameObject

	// handles what UI will be displayed based on the 1 model/video/pic per target rule
	void Update () {

		Debug.Log("update is running");

		//controls whether the addWonder button appears based on number of targets
		if (fm.targetCount == 3)
		{
			addWonderButton.SetActive(false);
		}
		else if (fm.targetCount < 3)
		{
			addWonderButton.SetActive(true);
		}
        
		//activate correct ar pair display and correct model/video/image icon
    switch (fm.targetStatus[0])
		{
			case "none":
				arPair1.SetActive(false);
				break;
			case "created":
				arPair1.SetActive(true);
				modelIcon1.SetActive(false);
				imageIcon1.SetActive(false);
				videoIcon1.SetActive(false);
				break;
			case "model":
				arPair1.SetActive(true);
				modelIcon1.SetActive(true);
				imageIcon1.SetActive(false);
				videoIcon1.SetActive(false);
				break;
			case "image":
				arPair1.SetActive(true);
				modelIcon1.SetActive(false);
				imageIcon1.SetActive(true);
				videoIcon1.SetActive(false);
				break;
			case "video":
				arPair1.SetActive(true);
				modelIcon1.SetActive(false);
				imageIcon1.SetActive(false);
				videoIcon1.SetActive(true);
				break;
		}

		switch(fm.targetStatus[1])
		{
			case "none":
				arPair2.SetActive(false);
				break;
			case "created":
				arPair2.SetActive(true);
				modelIcon2.SetActive(false);
				imageIcon2.SetActive(false);
				videoIcon2.SetActive(false);
				break;
			case "model":
				arPair2.SetActive(true);
				modelIcon2.SetActive(true);
				imageIcon2.SetActive(false);
				videoIcon2.SetActive(false);
				break;
			case "image":
				arPair2.SetActive(true);
				modelIcon2.SetActive(false);
				imageIcon2.SetActive(true);
				videoIcon2.SetActive(false);
				break;
			case "video":
				arPair2.SetActive(true);
				modelIcon2.SetActive(false);
				imageIcon2.SetActive(false);
				videoIcon2.SetActive(true);
				break;
		}

		switch(fm.targetStatus[2])
		{
			case "none":
				arPair3.SetActive(false);
				break;
			case "created":
				arPair3.SetActive(true);
				modelIcon3.SetActive(false);
				imageIcon3.SetActive(false);
				videoIcon3.SetActive(false);
				break;
			case "model":
				arPair3.SetActive(true);
				modelIcon3.SetActive(true);
				imageIcon3.SetActive(false);
				videoIcon3.SetActive(false);
				break;
			case "image":
				arPair3.SetActive(true);
				modelIcon3.SetActive(false);
				imageIcon3.SetActive(true);
				videoIcon3.SetActive(false);
				break;
			case "video":
				arPair3.SetActive(true);
				modelIcon3.SetActive(false);
				imageIcon3.SetActive(false);
				videoIcon3.SetActive(true);
				break;
		}

		switch(fm.targetStatus[3])
		{
			case "none":
				arPair4.SetActive(false);
				break;
			case "created":
				arPair4.SetActive(true);
				modelIcon4.SetActive(false);
				imageIcon4.SetActive(false);
				videoIcon4.SetActive(false);
				break;
			case "model":
				arPair4.SetActive(true);
				modelIcon4.SetActive(true);
				imageIcon4.SetActive(false);
				videoIcon4.SetActive(false);
				break;
			case "image":
				arPair4.SetActive(true);
				modelIcon4.SetActive(false);
				imageIcon4.SetActive(true);
				videoIcon4.SetActive(false);
				break;
			case "video":
				arPair4.SetActive(true);
				modelIcon4.SetActive(false);
				imageIcon4.SetActive(false);
				videoIcon4.SetActive(true);
				break;
		}

		switch(fm.targetStatus[4])
		{
			case "none":
				arPair5.SetActive(false);
				break;
			case "created":
				arPair5.SetActive(true);
				modelIcon5.SetActive(false);
				imageIcon5.SetActive(false);
				videoIcon5.SetActive(false);
				break;
			case "model":
				arPair5.SetActive(true);
				modelIcon5.SetActive(true);
				imageIcon5.SetActive(false);
				videoIcon5.SetActive(false);
				break;
			case "image":
				arPair5.SetActive(true);
				modelIcon5.SetActive(false);
				imageIcon5.SetActive(true);
				videoIcon5.SetActive(false);
				break;
			case "video":
				arPair5.SetActive(true);
				modelIcon5.SetActive(false);
				imageIcon5.SetActive(false);
				videoIcon5.SetActive(true);
				break;
		}
	}
}
