/******************************************************
*Project: Wonderly
*Created by: David Lee Ramirez
*Date: 12/28/18
*Description: Used for response for web call to Pixabay api 
							to get search results. This class represents one
							search result.
*Copyright 2018 LeapWithAlice,LLC. All rights reserved
 ******************************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct pixabayHitClass {
	public int id;
	public string pageURL;
	public string type;
	public string tags;
	public string previewURL;
	public int previewWidth;
	public int previewHeight;
	public string webformatURL;
	public int webformatWidth;
	public int webformatHeight;
	public string largeImageURL;
	public string fullHDURL;
	public string imageURL;
	public int imageWidth;
	public int imageHeight;
	public int imageSize;
	public int views;
	public int downloads;
	public int favorites;
	public int likes;
	public int comments;
	public int user_id;
	public string user;
	public string userImageURL;
}
