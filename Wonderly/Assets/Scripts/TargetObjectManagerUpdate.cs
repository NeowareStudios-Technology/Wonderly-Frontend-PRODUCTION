/******************************************************
*Project: Wonderly
*Created by: David Lee Ramirez
*Date: 12/28/18
*Description: Handles the displaying of AR 
							linked objects on each target. 
*Copyright 2018 LeapWithAlice,LLC. All rights reserved
 ******************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetObjectManagerUpdate : MonoBehaviour {

	//script references
	public targetObjectManager tom;
	public FilesManager fm;
	public ImageTargetManager itm;
	
	// Update is called once per frame
	void Update () {
		GameObject videoPlayer = tom.videoPlayers[0];
		GameObject targetMarker = tom.targetMarkers[0];
		GameObject image = tom.images[0];
		//for each possible target
		for (int i = 0; i < 3; i++)
		{
			switch(i)
			{
				case 0:
					videoPlayer = tom.videoPlayers[0];
					targetMarker = tom.targetMarkers[0];
					image = tom.images[0];
					break;
				case 1:
					videoPlayer = tom.videoPlayers[1];
					targetMarker = tom.targetMarkers[1];
					image = tom.images[1];
					break;
				case 2:
					videoPlayer = tom.videoPlayers[2];
					targetMarker = tom.targetMarkers[2];
					image = tom.images[2];
					break;
				case 3:
					videoPlayer = tom.videoPlayers[3];
					targetMarker = tom.targetMarkers[3];
					image = tom.images[3];
					break;
				case 4:
					videoPlayer = tom.videoPlayers[4];
					targetMarker = tom.targetMarkers[4];
					image = tom.images[4];
					break;
			}
			//if there is no linked AR object, reset all models and set videoplayer off screen, and deactivate image
			if (fm.targetStatus[i] == "none")
			{
				//videoPlayer.SetActive(false);
				targetMarker.SetActive(false);
				image.SetActive(false);
				switch(i)
				{
					case 0:
						tom.resetTargetModel(1);
						tom.videoPlayers[0].gameObject.transform.position = new Vector3(90f, 0f, 0f);
						break;
					case 1:
						tom.resetTargetModel(2);
						tom.videoPlayers[1].gameObject.transform.position = new Vector3(90f, 0f, 0f);
						break;
					case 2:
						tom.resetTargetModel(3);
						tom.videoPlayers[2].gameObject.transform.position = new Vector3(90f, 0f, 0f);
						break;
					case 3:
						tom.resetTargetModel(4);
						tom.videoPlayers[3].gameObject.transform.position = new Vector3(90f, 0f, 0f);
						break;
					case 4:
						tom.resetTargetModel(5);
						tom.videoPlayers[4].gameObject.transform.position = new Vector3(90f, 0f, 0f);
						break;
				}
			}
			//if the linked AR object is an image, reset model if any, and only activate image
			else if (fm.targetStatus[i] == "image")
			{
				switch(i)
				{
					case 0:
						tom.resetTargetModel(1);
						break;
					case 1:
						tom.resetTargetModel(2);
						break;
					case 2:
						tom.resetTargetModel(3);
						break;
					case 3:
						tom.resetTargetModel(4);
						break;
					case 4:
						tom.resetTargetModel(5);
						break;
				}
				videoPlayer.SetActive(false);
				targetMarker.SetActive(false);
				image.SetActive(true);
			}

			else if (fm.targetStatus[i] == "created")
			{
				//videoPlayer.SetActive(false);
				targetMarker.SetActive(true);
				image.SetActive(false);
				switch(i)
				{
					case 0:
						tom.resetTargetModel(1);
						tom.videoPlayers[0].gameObject.transform.position = new Vector3(90f, 0f, 0f);
						break;
					case 1:
						tom.resetTargetModel(2);
						tom.videoPlayers[1].gameObject.transform.position = new Vector3(90f, 0f, 0f);
						break;
					case 2:
						tom.resetTargetModel(3);
						tom.videoPlayers[2].gameObject.transform.position = new Vector3(90f, 0f, 0f);
						break;
					case 3:
						tom.resetTargetModel(4);
						tom.videoPlayers[3].gameObject.transform.position = new Vector3(90f, 0f, 0f);
						break;
					case 4:
						tom.resetTargetModel(5);
						tom.videoPlayers[4].gameObject.transform.position = new Vector3(90f, 0f, 0f);
						break;
				}
			}
			else if (fm.targetStatus[i] == "model")
			{
				videoPlayer.SetActive(false);
				targetMarker.SetActive(false);
				image.SetActive(false);
			}
			
			else if (fm.targetStatus[i] == "video")
			{
				switch(i)
				{
					case 0:	
						tom.resetTargetModel(1);
						if (itm.activeTarget1 == false && itm.target1.activeSelf == true)
						{
							tom.videoPlayers[0].GetComponent<SimplePlayback>().unityVideoPlayer.Pause();
							tom.videoPlayers[0].gameObject.transform.position = new Vector3(90f, 0f, 0f);
						}
						break;
					case 1:
						tom.resetTargetModel(2);
						if (itm.activeTarget2 == false && itm.target2.activeSelf == true)
						{
							tom.videoPlayers[1].GetComponent<SimplePlayback>().unityVideoPlayer.Pause();
							tom.videoPlayers[1].gameObject.transform.position = new Vector3(90f, 0f, 0f);
						}
						break;
					case 2:
						tom.resetTargetModel(3);
						if (itm.activeTarget3 == false && itm.target3.activeSelf == true)
						{
							tom.videoPlayers[2].GetComponent<SimplePlayback>().unityVideoPlayer.Pause();
							tom.videoPlayers[2].gameObject.transform.position = new Vector3(90f, 0f, 0f);
						}
						break;
					case 3:
						tom.resetTargetModel(4);
						if (itm.activeTarget4 == false && itm.target4.activeSelf == true)
						{
							tom.videoPlayers[3].GetComponent<SimplePlayback>().unityVideoPlayer.Pause();
							tom.videoPlayers[3].gameObject.transform.position = new Vector3(90f, 0f, 0f);
						}
						break;
					case 4:
						tom.resetTargetModel(5);
						if (itm.activeTarget5 == false && itm.target5.activeSelf == true)
						{
							tom.videoPlayers[4].GetComponent<SimplePlayback>().unityVideoPlayer.Pause();
							tom.videoPlayers[4].gameObject.transform.position = new Vector3(90f, 0f, 0f);
						}
						break;
				}
				videoPlayer.SetActive(true);
				targetMarker.SetActive(false);
				image.SetActive(false);
				
			}
		}
	}
}
