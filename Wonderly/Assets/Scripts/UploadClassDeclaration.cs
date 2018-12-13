﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UploadClassDeclaration {
	public string title = "";
	public int model;
	public int video;
	public int image;
	public bool t1;
	public bool t2;
	public bool t3;
	public bool t4;
	public bool t5;
	public string coverImage = "";

	

	public static UploadClassDeclaration CreateFromJSON(string jsonString)
  {
    return JsonUtility.FromJson<UploadClassDeclaration>(jsonString);
  }

}

