using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sample;

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
						tom.model1.transform.Rotate(Vector3.right * 45);
						break;
					case "video":
						tom.videoPlayer1.transform.Rotate(Vector3.up * 45);
						break;
					case "image":
						tom.image1.transform.Rotate(Vector3.forward * 45);
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
						tom.model2.transform.Rotate(Vector3.right * 45);
						break;
					case "video":
						tom.videoPlayer2.transform.Rotate(Vector3.up * 45);
						break;
					case "image":
						tom.image2.transform.Rotate(Vector3.forward * 45);
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
						tom.model3.transform.Rotate(Vector3.right * 45);
						break;
					case "video":
						tom.videoPlayer3.transform.Rotate(Vector3.up * 45);
						break;
					case "image":
						tom.image3.transform.Rotate(Vector3.forward* 45);
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
						tom.model4.transform.Rotate(Vector3.right * 45);
						break;
					case "video":
						tom.videoPlayer4.transform.Rotate(Vector3.up * 45);
						break;
					case "image":
						tom.image4.transform.Rotate(Vector3.forward * 45);
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
						tom.model5.transform.Rotate(Vector3.right * 45);
						break;
					case "video":
						tom.videoPlayer5.transform.Rotate(Vector3.up * 45);
						break;
					case "image":
						tom.image5.transform.Rotate(Vector3.forward * 45);
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
						tom.model1.transform.Rotate(Vector3.forward * 45);
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
						tom.model2.transform.Rotate(Vector3.forward * 45);
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
						tom.model3.transform.Rotate(Vector3.forward * 45);
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
						tom.model4.transform.Rotate(Vector3.forward * 45);
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
						tom.model5.transform.Rotate(Vector3.forward * 45);
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
						tom.model1.transform.localScale = new Vector3(scaleSlider.value, scaleSlider.value, scaleSlider.value);
						break;
					case "video":
						tom.videoPlayer1.transform.localScale = new Vector3(scaleSlider.value, scaleSlider.value, scaleSlider.value);
						break;
					case "image":
						tom.image1.transform.localScale = new Vector3(scaleSlider.value, scaleSlider.value, scaleSlider.value);
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
						tom.model2.transform.localScale = new Vector3(scaleSlider.value, scaleSlider.value, scaleSlider.value);
						break;
					case "video":
						tom.videoPlayer2.transform.localScale = new Vector3(scaleSlider.value, scaleSlider.value, scaleSlider.value);
						break;
					case "image":
						tom.image2.transform.localScale = new Vector3(scaleSlider.value, scaleSlider.value, scaleSlider.value);
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
						tom.model3.transform.localScale = new Vector3(scaleSlider.value, scaleSlider.value, scaleSlider.value);
						break;
					case "video":
						tom.videoPlayer3.transform.localScale = new Vector3(scaleSlider.value, scaleSlider.value, scaleSlider.value);
						break;
					case "image":
						tom.image3.transform.localScale = new Vector3(scaleSlider.value, scaleSlider.value, scaleSlider.value);
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
						tom.model4.transform.localScale = new Vector3(scaleSlider.value, scaleSlider.value, scaleSlider.value);
						break;
					case "video":
						tom.videoPlayer4.transform.localScale = new Vector3(scaleSlider.value, scaleSlider.value, scaleSlider.value);
						break;
					case "image":
						tom.image4.transform.localScale = new Vector3(scaleSlider.value, scaleSlider.value, scaleSlider.value);
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
						tom.model5.transform.localScale = new Vector3(scaleSlider.value, scaleSlider.value, scaleSlider.value);
						break;
					case "video":
						tom.videoPlayer5.transform.localScale = new Vector3(scaleSlider.value, scaleSlider.value, scaleSlider.value);
						break;
					case "image":
						tom.image5.transform.localScale = new Vector3(scaleSlider.value, scaleSlider.value, scaleSlider.value);
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
