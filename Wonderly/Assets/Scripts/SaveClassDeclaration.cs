/******************************************************
*Project: Wonderly
*Created by: David Lee Ramirez
*Date: 12/28/18
*Description: Used for creating a JSON save file that represents
							the AR journey. 
*Copyright 2018 LeapWithAlice,LLC. All rights reserved
 ******************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveClassDeclaration {
	public int targetNum;
	public string[] targetStatus = new string[5];
	public string coverImageUrl = "";
	public string title = "";
	public string description = "";
	public string[] wonderTitle = new string[5];
	public string[] wonderDescription = new string[5];
	public string[] vId = new string[5];
	public string[] imageUrl = new string[5];
	public string[] modId = new string[5];
	public float[] rot1 = new float[3];
	public float[] rot2 = new float[3];
	public float[] rot3 = new float[3];
	public float[] rot4 = new float[3];
	public float[] rot5 = new float[3];
	public float[] scale1 = new float[3];
	public float[] scale2 = new float[3];
	public float[] scale3 = new float[3];
	public float[] scale4 = new float[3];
	public float[] scale5 = new float[3];

	public static SaveClassDeclaration CreateFromJSON(string jsonString)
  {
    return JsonUtility.FromJson<SaveClassDeclaration>(jsonString);
  }

}



