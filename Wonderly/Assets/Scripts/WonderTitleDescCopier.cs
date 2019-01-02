/******************************************************
*Project: Wonderly
*Created by: David Lee Ramirez
*Date: 12/28/18
*Description: Used to ensure that when a user edits a wonderss
							title or description, the current title and description
							is placed in the input fields. Used in SetupWonder-panel SINGLE.
*Copyright 2018 LeapWithAlice,LLC. All rights reserved
 ******************************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WonderTitleDescCopier : MonoBehaviour {
	public InputField newWonderTitle;
	public InputField newWonderDesc;

	public Text[] currentWonderTitles = new Text[5];
	public Text[] currentWonderDescs = new Text[5];
	
	public void copyWonderTitleDesc(int index)
	{
		newWonderTitle.text = currentWonderTitles[index-1].text;
		newWonderDesc.text = currentWonderDescs[index-1].text;
	}
}
