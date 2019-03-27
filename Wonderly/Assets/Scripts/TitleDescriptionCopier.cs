/******************************************************
*Project: Wonderly
*Created by: David Lee Ramirez
*Date: 12/28/18
*Description: Used to ensure that when a user edits a journeys
							title or description, the current title and description
							is placed in the input fields. Used in CreateJourney-panel SINGLE.
*Copyright 2018 LeapWithAlice,LLC. All rights reserved
 ******************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleDescriptionCopier : MonoBehaviour {

	//references to current title and description of journey
	public Text currentTitle;
	public Text currentDescription;
	//input fields for editing title and description of journey
	public InputField newTitle;
	public InputField newDescription;


	//copy the existing title and description to the input fields
	void OnEnable () {
		newTitle.text = currentTitle.text;
		newDescription.text = currentDescription.text;
	}
	
	//clear the text on the screen
	void OnDisable () {
		newTitle.text = "";
		newDescription.text = "";
	}
}
