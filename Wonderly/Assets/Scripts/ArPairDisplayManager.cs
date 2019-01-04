﻿/******************************************************
*Project: Wonderly
*Created by: David Lee Ramirez
*Date: 12/28/18
*Description: Sets thumbnails for preview and summary screens
*Copyright 2018 LeapWithAlice,LLC. All rights reserved
 ******************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ArPairDisplayManager : MonoBehaviour {
	public FilesManager fm;
	public VideoSearchManager vsm;
	public ModelInitializer mi;
	public UiManager um;
	public YoutubeVideoUi[] yvuArray = new YoutubeVideoUi[18];

	public Image chosenThumb;
	public Image chosenThumb2;
	public Image[] targetThumbs = new Image[5];
	public Image[] targetObjectThumbs = new Image[5];
	public Image blankImage;
	public Image[] videoThumbs = new Image[18];
	public Image[] modelThumbs = new Image[18];
	public int[] targetThumbCheck = {0,0,0,0,0};

	//for summary screen
	public Text[] wonderTitles = new Text[5];
	public Text[] wonderDescriptions = new Text[5];

	//for preview screen
	public Text[] previewWonderDescriptions = new Text[5];
	public Text[] previewWonderTitles = new Text[5];



	// IMAGE THUMBNAIL HANDLING FOR AR PAIRS HANDLED IN IMAGE SCRIPTS
/* 
	// Update is called once per frame
	void Update () {

		//go through each possible ar target pair and set up with proper target photo and manage all target object photos
		for (int i=0; i<5; i++)
		{
			switch(i)
			{
				case 0:
					string thisPath1 = Path.Combine(fm.MarksDirectory, "targetPhoto1.jpg");
					//if the target thumb has not yet been set and the target photo exists, set the target thumb
					if (targetThumbCheck[0] == 0 && File.Exists(thisPath1))
					{
						targetThumbs[0].sprite = IMG2Sprite.LoadNewSprite(thisPath1);
						targetThumbCheck[0] = 1;
					}
					//else if the target thumb has been set, but the target photo has been deleted, blank the target thumb
					else if(targetThumbCheck[0] == 1 && !File.Exists(thisPath1))
					{
						targetThumbs[0].sprite = blankImage.sprite;
						targetThumbCheck[0] = 0;
					}
					break;

				case 1:
					string thisPath2 = Path.Combine(fm.MarksDirectory, "targetPhoto2.jpg");
					if (targetThumbCheck[1] == 0 && File.Exists(thisPath2))
					{
						targetThumbs[1].sprite = IMG2Sprite.LoadNewSprite(thisPath2);
						targetThumbCheck[1] = 1;
					}
					else if(targetThumbCheck[1] == 1 && !File.Exists(thisPath2))
					{
						targetThumbs[1].sprite = blankImage.sprite;
						targetThumbCheck[1] = 0;
					}
					break;
				case 2:
					string thisPath3 = Path.Combine(fm.MarksDirectory, "targetPhoto3.jpg");
					if (targetThumbCheck[2] == 0 && File.Exists(thisPath3))
					{
						targetThumbs[2].sprite = IMG2Sprite.LoadNewSprite(thisPath3);
						targetThumbCheck[2] = 1;
					}
					else if(targetThumbCheck[2] == 1 && !File.Exists(thisPath3))
					{
						targetThumbs[2].sprite = blankImage.sprite;
						targetThumbCheck[2] = 0;
					}
					break;
				case 3:
					string thisPath4 = Path.Combine(fm.MarksDirectory, "targetPhoto4.jpg");
					if (targetThumbCheck[3] == 0 && File.Exists(thisPath4))
					{
						targetThumbs[3].sprite = IMG2Sprite.LoadNewSprite(thisPath4);
						targetThumbCheck[3] = 1;
					}
					else if(targetThumbCheck[3] == 1 && !File.Exists(thisPath4))
					{
						targetThumbs[3].sprite = blankImage.sprite;
						targetThumbCheck[3] = 0;
					}
					break;
				case 4:
					string thisPath5 = Path.Combine(fm.MarksDirectory, "targetPhoto5.jpg");
					if (targetThumbCheck[4] == 0 && File.Exists(thisPath5))
					{
						targetThumbs[4].sprite = IMG2Sprite.LoadNewSprite(thisPath5);
						targetThumbCheck[4] = 1;
					}
					else if(targetThumbCheck[4] == 1 && !File.Exists(thisPath5))
					{
						targetThumbs[4].sprite = blankImage.sprite;
						targetThumbCheck[4] = 0;
					}
					break;
			}
		}
	}
*/

	public void blankTargetObjectThumb()
	{
		targetObjectThumbs[fm.currentTarget-1].sprite = blankImage.sprite;
	}

	public void setYoutubeThumbnailArPair(GameObject thumbNail)
  {
		

		if (thumbNail.GetComponent<YoutubeVideoUi>().videoId =="")
			return;

		chosenThumb.sprite = thumbNail.GetComponent<Image>().sprite;
		chosenThumb2.sprite = thumbNail.GetComponent<Image>().sprite;
		targetObjectThumbs[fm.currentTarget-1].sprite = thumbNail.GetComponent<Image>().sprite;

		vsm.ClearSearchField();
		vsm.DeleteThumbnails();

	}


		public void setModelThumbnailArPair(GameObject index)
  {
		//do nothing if target num not valid
		if (fm.currentTarget < 1)
			return;
		//set object thubnail in Journey summary
		targetObjectThumbs[fm.currentTarget-1].sprite = index.GetComponent<Image>().sprite;

		//set object thumbnail in select object
		chosenThumb.sprite = index.GetComponent<Image>().sprite;
		chosenThumb2.sprite = index.GetComponent<Image>().sprite;

		mi.DeleteThumbnails();
		mi.ClearSearchText();
	}
}
