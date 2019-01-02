/******************************************************
*Project: Wonderly
*Created by: David Lee Ramirez
*Date: 12/28/18
*Description: Controls rotation and scaling of AR linked object
							via user input (buttons, slider).
*Copyright 2018 LeapWithAlice,LLC. All rights reserved
 ******************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateScaleManager : MonoBehaviour {

	public targetObjectManager tom;
	public FilesManager fm;
	public Slider scaleSlider;

	public void rotateX()
	{
		//determine what target is being viewed
		switch(fm.currentTarget)
		{
			//do nothing if there is no target
			case 0:
				return;
			case 1:
				//determine what is on the target
				switch(fm.targetStatus[fm.currentTarget-1])
				{
					//do nothing if there is no target object
					case "none":
						return;
					case "created":
						return;
					case "model":
						tom.models[0].transform.Rotate(Vector3.right * 45);
						break;
					case "video":
						tom.videoPlayers[0].transform.Rotate(Vector3.up * 45);
						break;
					case "image":
						tom.images[0].transform.Rotate(Vector3.forward * 45);
						break;
				}
				break;
			case 2:
				switch(fm.targetStatus[fm.currentTarget-1])
				{
					//do nothing if there is no target object
					case "none":
						return;
					case "created":
						return;
					case "model":
						tom.models[1].transform.Rotate(Vector3.right * 45);
						break;
					case "video":
						tom.videoPlayers[1].transform.Rotate(Vector3.up * 45);
						break;
					case "image":
						tom.images[1].transform.Rotate(Vector3.forward * 45);
						break;
				}
				break;
			case 3:
				switch(fm.targetStatus[fm.currentTarget-1])
				{
					//do nothing if there is no target object
					case "none":
						return;
					case "created":
						return;
					case "model":
						tom.models[2].transform.Rotate(Vector3.right * 45);
						break;
					case "video":
						tom.videoPlayers[2].transform.Rotate(Vector3.up * 45);
						break;
					case "image":
						tom.images[2].transform.Rotate(Vector3.forward* 45);
						break;
				}
				break;
			case 4:
				switch(fm.targetStatus[fm.currentTarget-1])
				{
					//do nothing if there is no target object
					case "none":
						return;
					case "created":
						return;
					case "model":
						tom.models[3].transform.Rotate(Vector3.right * 45);
						break;
					case "video":
						tom.videoPlayers[3].transform.Rotate(Vector3.up * 45);
						break;
					case "image":
						tom.images[3].transform.Rotate(Vector3.forward * 45);
						break;
				}
				break;
			case 5:
				switch(fm.targetStatus[fm.currentTarget-1])
				{
					//do nothing if there is no target object
					case "none":
						return;
					case "created":
						return;
					case "model":
						tom.models[4].transform.Rotate(Vector3.right * 45);
						break;
					case "video":
						tom.videoPlayers[4].transform.Rotate(Vector3.up * 45);
						break;
					case "image":
						tom.images[4].transform.Rotate(Vector3.forward * 45);
						break;
				}
				break;
		}
	}


	public void rotateY()
	{
		//determine what target is being viewed
		switch(fm.currentTarget)
		{
			//do nothing if there is no target
			case 0:
				return;
			case 1:
				//determine what is on the target
				switch(fm.targetStatus[fm.currentTarget-1])
				{
					//do nothing if there is no target object
					case "none":
						return;
					case "created":
						return;
					case "model":
						tom.models[0].transform.Rotate(Vector3.forward * 45);
						break;
				}
				break;
			case 2:
				switch(fm.targetStatus[fm.currentTarget-1])
				{
					//do nothing if there is no target object
					case "none":
						return;
					case "created":
						return;
					case "model":
						tom.models[1].transform.Rotate(Vector3.forward * 45);
						break;
				}
				break;
			case 3:
				switch(fm.targetStatus[fm.currentTarget-1])
				{
					//do nothing if there is no target object
					case "none":
						return;
					case "created":
						return;
					case "model":
						tom.models[2].transform.Rotate(Vector3.forward * 45);
						break;
				}
				break;
			case 4:
				switch(fm.targetStatus[fm.currentTarget-1])
				{
					//do nothing if there is no target object
					case "none":
						return;
					case "created":
						return;
					case "model":
						tom.models[3].transform.Rotate(Vector3.forward * 45);
						break;
				}
				break;
			case 5:
				switch(fm.targetStatus[fm.currentTarget-1])
				{
					//do nothing if there is no target object
					case "none":
						return;
					case "created":
						return;
					case "model":
						tom.models[4].transform.Rotate(Vector3.forward * 45);
						break;
				}
				break;
		}
	}

	public void scaleTargetObject()
	{
		switch(fm.currentTarget)
		{
			//do nothing if there is no target
			case 0:
				return;
			case 1:
				//determine what is on the target
				switch(fm.targetStatus[0])
				{
					//do nothing if there is no target object
					case "none":
						return;
					case "created":
						return;
					case "model":
						tom.models[0].transform.localScale = new Vector3(scaleSlider.value, scaleSlider.value, scaleSlider.value);
						break;
					case "video":
						tom.videoPlayers[0].transform.localScale = new Vector3(scaleSlider.value, scaleSlider.value, scaleSlider.value);
						break;
					case "image":
						tom.images[0].transform.localScale = new Vector3(scaleSlider.value, scaleSlider.value, scaleSlider.value);
						break;
				}
				break;
			case 2:
				switch(fm.targetStatus[1])
				{
					//do nothing if there is no target object
					case "none":
						return;
					case "created":
						return;
					case "model":
						tom.models[1].transform.localScale = new Vector3(scaleSlider.value, scaleSlider.value, scaleSlider.value);
						break;
					case "video":
						tom.videoPlayers[1].transform.localScale = new Vector3(scaleSlider.value, scaleSlider.value, scaleSlider.value);
						break;
					case "image":
						tom.images[1].transform.localScale = new Vector3(scaleSlider.value, scaleSlider.value, scaleSlider.value);
						break;
				}
				break;
			case 3:
				switch(fm.targetStatus[2])
				{
					case "none":
						return;
					case "created":
						return;
					case "model":
						tom.models[2].transform.localScale = new Vector3(scaleSlider.value, scaleSlider.value, scaleSlider.value);
						break;
					case "video":
						tom.videoPlayers[2].transform.localScale = new Vector3(scaleSlider.value, scaleSlider.value, scaleSlider.value);
						break;
					case "image":
						tom.images[2].transform.localScale = new Vector3(scaleSlider.value, scaleSlider.value, scaleSlider.value);
						break;
				}
				break;
			case 4:
				switch(fm.targetStatus[3])
				{
					//do nothing if there is no target object
					case "none":
						return;
					case "created":
						return;
					case "model":
						tom.models[3].transform.localScale = new Vector3(scaleSlider.value, scaleSlider.value, scaleSlider.value);
						break;
					case "video":
						tom.videoPlayers[3].transform.localScale = new Vector3(scaleSlider.value, scaleSlider.value, scaleSlider.value);
						break;
					case "image":
						tom.images[3].transform.localScale = new Vector3(scaleSlider.value, scaleSlider.value, scaleSlider.value);
						break;
				}
				break;
			case 5:
				switch(fm.targetStatus[4])
				{
					case "none":
						return;
					case "created":
						return;
					case "model":
						tom.models[4].transform.localScale = new Vector3(scaleSlider.value, scaleSlider.value, scaleSlider.value);
						break;
					case "video":
						tom.videoPlayers[4].transform.localScale = new Vector3(scaleSlider.value, scaleSlider.value, scaleSlider.value);
						break;
					case "image":
						tom.images[4].transform.localScale = new Vector3(scaleSlider.value, scaleSlider.value, scaleSlider.value);
						break;
				}
				break;
		}
	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
