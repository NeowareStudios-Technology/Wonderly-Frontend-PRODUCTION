/******************************************************
*Project: Wonderly
*Created by: David Lee Ramirez
*Date: 12/28/18
*Description: Used for JSON response for Cloud Endpoints backend
							web call to check if users propsed email address
							is already signed up with Wonderly.
*Copyright 2018 LeapWithAlice,LLC. All rights reserved
 ******************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct checkEmailResponseClass {
	public string exists;
}
