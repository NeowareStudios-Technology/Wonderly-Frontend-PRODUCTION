/******************************************************
*Project: Wonderly
*Created by: David Lee Ramirez
*Date: 12/28/18
*Description: Used for JSON request body to Google Cloud
							Endpoints web call to edit an existing 
							journey.
*Copyright 2018 LeapWithAlice,LLC. All rights reserved
 ******************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class editExperienceClass {
	public string title = "";
	public int video;
	public int model;
	public int image;
	public bool text;
	public bool weblink;
	public string code;
	public string coverImage;
}
