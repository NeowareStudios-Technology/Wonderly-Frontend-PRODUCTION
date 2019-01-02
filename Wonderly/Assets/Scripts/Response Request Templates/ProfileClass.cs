/******************************************************
*Project: Wonderly
*Created by: David Lee Ramirez
*Date: 12/28/18
*Description: Used for response/request of web call to 
							Google Cloud Endpoints to set users first 
							and last name.
*Copyright 2018 LeapWithAlice,LLC. All rights reserved
 ******************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ProfileClass {
	public string firstName;
	public string lastName;
}

