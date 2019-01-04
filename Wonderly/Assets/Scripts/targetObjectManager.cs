/******************************************************
*Project: Wonderly
*Created by: David Lee Ramirez
*Date: 12/28/18
*Description: Handles the displaying and removal of AR 
							linked objects on each target. 
*Copyright 2018 LeapWithAlice,LLC. All rights reserved
 ******************************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class targetObjectManager : MonoBehaviour {
	//script reference
	public FilesManager fm;
	public ImageTargetManager itm;
	//holds all video players for targets
	public GameObject[] videoPlayers = new GameObject[5];
	//holds all markers (white ball) for targets
	public GameObject[] targetMarkers = new GameObject[5];
	//holds all possible models for targets
	public GameObject[] models = new GameObject[5];
	//holds all images for AR targets
	public GameObject[] images = new GameObject[5];
	//holds all IDs for imported Poly models
	public string[] modelIds = new string[5];
	//reference to linked AR object thumbnails
	public Image[] targetObjectThumbnails = new Image[5];
	//used to blank out sprites
	public Image blankSprite;
	
	public List<string> attribs = new List<string>();


	//resets model: destroys gameObject and sets id to ""
	public void resetTargetModel(int whichModel)
	{
		switch(whichModel)
		{
			case 1:
				Destroy(models[0]);
				modelIds[0] = "";
				break;
			case 2:
				Destroy(models[1]);
				modelIds[1] = "";
				break;
			case 3:
				Destroy(models[2]);
				modelIds[2] = "";
				break;
			case 4:
				Destroy(models[3]);
				modelIds[3] = "";
				break;
			case 5:
				Destroy(models[4]);
				modelIds[4] = "";
				break;
		}
	}


	public void removeTargetObject(int whichTarget)
	{
		//do nothing if there is no target object 
		if(fm.targetStatus[whichTarget-1] == "none" || fm.targetStatus[whichTarget-1] == "created")
			return;

		switch(whichTarget)
		{
			case 1:
				//set to created, since we know that the image was not "none" or "created" before, but set to a model, vid, or pic
				fm.targetStatus[0] = "created";
				if (models[0] != null)
				{
					Destroy(models[0]);
					modelIds[0] = null;
					models[0] = null;
				}
				videoPlayers[0].SetActive(false);
				targetObjectThumbnails[0].sprite = blankSprite.sprite;
				break;
			case 2:
				fm.targetStatus[1] = "created";
				if (models[1] != null)
				{
					Destroy(models[1]);
					modelIds[1] = null;
					models[1] = null;
				}
				videoPlayers[1].SetActive(false);
				targetObjectThumbnails[1].sprite = blankSprite.sprite;
				break;
			case 3:
				fm.targetStatus[2] = "created";
				if (models[2] != null)
				{
					Destroy(models[2]);
					modelIds[2] = null;
					models[2] = null;
				}
				videoPlayers[2].SetActive(false);
				targetObjectThumbnails[2].sprite = blankSprite.sprite;
				break;
			case 4:
				fm.targetStatus[3] = "created";
				if (models[3] != null)
				{
					Destroy(models[3]);
					modelIds[3] = null;
					models[3] = null;
				}
				videoPlayers[3].SetActive(false);
				targetObjectThumbnails[3].sprite = blankSprite.sprite;
				break;
			case 5:
				fm.targetStatus[4] = "created";
				if (models[4] != null)
				{
					Destroy(models[4]);
					modelIds[4] = null;
					models[4] = null;
				}
				videoPlayers[0].SetActive(false);
				targetObjectThumbnails[4].sprite = blankSprite.sprite;
				break;
		}
	}

	public void manualRemoveTargetObject(int whichTarget)
	{
		//do nothing if there is no target object 
		if(fm.targetStatus[whichTarget-1] == "none" || fm.targetStatus[whichTarget-1] == "created")
			return;

		switch(whichTarget)
		{
			case 1:
				fm.targetStatus[0] = "created";
				if (models[0] != null)
				{
					Destroy(models[0]);
					modelIds[0] = null;
					models[0] = null;
				}
				videoPlayers[0].SetActive(false);
				
				break;
			case 2:
				fm.targetStatus[1] = "created";
				if (models[1] != null)
				{
					Destroy(models[1]);
					modelIds[1] = null;
					models[1] = null;
				}
				videoPlayers[1].SetActive(false);
				break;
			case 3:
				fm.targetStatus[2] = "created";
				if (models[2] != null)
				{
					Destroy(models[2]);
					modelIds[2] = null;
					models[2] = null;
				}
				videoPlayers[2].SetActive(false);
				break;
			case 4:
				fm.targetStatus[3] = "created";
				if (models[3] != null)
				{
					Destroy(models[3]);
					modelIds[3] = null;
					models[3] = null;
				}
				videoPlayers[3].SetActive(false);
				break;
			case 5:
				fm.targetStatus[4] = "created";
				if (models[4] != null)
				{
					Destroy(models[4]);
					modelIds[4] = null;
					models[4] = null;
				}
				videoPlayers[0].SetActive(false);
				break;
		}
	}


	//clear all gameobjects from scene
	public void clearScene() {
		if(itm.target1.transform.childCount == 4)
      Destroy(itm.target1.transform.GetChild(3).gameObject);
		if(itm.target2.transform.childCount == 4)
      Destroy(itm.target2.transform.GetChild(3).gameObject);
		if(itm.target3.transform.childCount == 4)
      Destroy(itm.target3.transform.GetChild(3).gameObject);
		if(itm.target4.transform.childCount == 4)
      Destroy(itm.target4.transform.GetChild(3).gameObject);
		if(itm.target5.transform.childCount == 4)
      Destroy(itm.target5.transform.GetChild(3).gameObject);

		for (int i = 0; i < 5; i++)
		{
			fm.targetStatus[i] = "none";
		}
    
		fm.targetCount = 0;
		fm.currentTarget = 0;

		models[0] = null;
		models[1] = null;
		models[2] = null;
		models[3] = null;
		models[4] = null;

		modelIds[0] = "";
		modelIds[1] = "";
		modelIds[2] = "";
		modelIds[3] = "";
		modelIds[4] = "";

		removeTargetObject(1);
		removeTargetObject(2);
		removeTargetObject(3);
		removeTargetObject(4);
		removeTargetObject(5);
      
	}
}
