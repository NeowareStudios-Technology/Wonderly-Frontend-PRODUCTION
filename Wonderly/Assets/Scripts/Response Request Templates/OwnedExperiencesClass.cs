/******************************************************
*Project: Wonderly
*Created by: David Lee Ramirez
*Date: 12/28/18
*Description: Used for response of web call to Google Cloud
							Endpoints Backend to get a list of experiences
							the user has created
*Copyright 2018 LeapWithAlice,LLC. All rights reserved
 ******************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OwnedExperiencesClass {
	public string[] codes = new string[50];
	public string[] titles = new string[50];
	public string[] dates = new string[50];
	public string[] coverImages = new string[50];
}