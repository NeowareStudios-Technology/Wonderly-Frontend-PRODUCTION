/******************************************************
*Project: Wonderly
*Created by: David Lee Ramirez
*Date: 12/28/18
*Description: Used for response of web call to Google Cloud 
							Endpoints backend for getting user profile info
*Copyright 2018 LeapWithAlice,LLC. All rights reserved
 ******************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ProfileInfoClass {
	public string firstName;
	public string lastName;
	public string email;
	public int createdExp;
}

