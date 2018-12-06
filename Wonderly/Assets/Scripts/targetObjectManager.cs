using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Sample;

public class targetObjectManager : MonoBehaviour {

	public GameObject[] videoPlayers = new GameObject[5];
	public GameObject[] targetMarkers = new GameObject[5];
	public GameObject[] models = new GameObject[5];
	public GameObject[] images = new GameObject[5];
	public string[] modelIds = new string[5];
	public Image[] targetObjectThumbnails = new Image[5];

	public Image blankSprite;

	public List<string> attribs = new List<string>();

	public FilesManager fm;
	public ImageTargetManager itm;

	// Use this for initialization
	void Start () {
		
	}
	
	// THIS NEEDS TO BE FIXED, AS IT IS NOW THERE IS NO SOUND
	// WHEN THE UPDATE() FUNCTION IS REMOVED, SOUND COMES BACK ON BUT THE VIDEO PLAYS WITHOUT THE ACTIVE TARGET UPON SELECTION
	//decides whether to display a targets video player depending on if a video was assigned or not
	//video player will also appear as placeholder for target on a "created" target 
	void Update () {

		
		GameObject videoPlayer = videoPlayers[0];
		GameObject targetMarker = targetMarkers[0];
		GameObject image = images[0];
		for (int i = 0; i < 5; i++)
		{
			switch(i)
			{
				case 0:
					videoPlayer = videoPlayers[0];
					targetMarker = targetMarkers[0];
					image = images[0];
					break;
				case 1:
					videoPlayer = videoPlayers[1];
					targetMarker = targetMarkers[1];
					image = images[1];
					break;
				case 2:
					videoPlayer = videoPlayers[2];
					targetMarker = targetMarkers[2];
					image = images[2];
					break;
				case 3:
					videoPlayer = videoPlayers[3];
					targetMarker = targetMarkers[3];
					image = images[3];
					break;
				case 4:
					videoPlayer = videoPlayers[4];
					targetMarker = targetMarkers[4];
					image = images[4];
					break;
			}
			//manages objects for each target
			if (fm.targetStatus[i] == "none")
			{
				//videoPlayer.SetActive(false);
				targetMarker.SetActive(false);
				image.SetActive(false);
				switch(i)
				{
					case 0:
						resetTargetModel(1);
						videoPlayers[0].gameObject.transform.position = new Vector3(90f, 0f, 0f);
						break;
					case 1:
						resetTargetModel(2);
						videoPlayers[1].gameObject.transform.position = new Vector3(90f, 0f, 0f);
						break;
					case 2:
						resetTargetModel(3);
						videoPlayers[2].gameObject.transform.position = new Vector3(90f, 0f, 0f);
						break;
					case 3:
						resetTargetModel(4);
						videoPlayers[3].gameObject.transform.position = new Vector3(90f, 0f, 0f);
						break;
					case 4:
						resetTargetModel(5);
						videoPlayers[4].gameObject.transform.position = new Vector3(90f, 0f, 0f);
						break;
				}
			}
			else if (fm.targetStatus[i] == "image")
			{
				switch(i)
				{
					case 0:
						resetTargetModel(1);
						break;
					case 1:
						resetTargetModel(2);
						break;
					case 2:
						resetTargetModel(3);
						break;
					case 3:
						resetTargetModel(4);
						break;
					case 4:
						resetTargetModel(5);
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
						resetTargetModel(1);
						videoPlayers[0].gameObject.transform.position = new Vector3(90f, 0f, 0f);
						break;
					case 1:
						resetTargetModel(2);
						videoPlayers[1].gameObject.transform.position = new Vector3(90f, 0f, 0f);
						break;
					case 2:
						resetTargetModel(3);
						videoPlayers[2].gameObject.transform.position = new Vector3(90f, 0f, 0f);
						break;
					case 3:
						resetTargetModel(4);
						videoPlayers[3].gameObject.transform.position = new Vector3(90f, 0f, 0f);
						break;
					case 4:
						resetTargetModel(5);
						videoPlayers[4].gameObject.transform.position = new Vector3(90f, 0f, 0f);
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
						resetTargetModel(1);
						if (itm.activeTarget1 == false && itm.target1.activeSelf == true)
						{
							videoPlayers[0].GetComponent<SimplePlayback>().unityVideoPlayer.Pause();
							videoPlayers[0].gameObject.transform.position = new Vector3(90f, 0f, 0f);
						}
						break;
					case 1:
						resetTargetModel(2);
						if (itm.activeTarget2 == false && itm.target2.activeSelf == true)
						{
							videoPlayers[1].GetComponent<SimplePlayback>().unityVideoPlayer.Pause();
							videoPlayers[1].gameObject.transform.position = new Vector3(90f, 0f, 0f);
						}
						break;
					case 2:
						resetTargetModel(3);
						if (itm.activeTarget3 == false && itm.target3.activeSelf == true)
						{
							videoPlayers[2].GetComponent<SimplePlayback>().unityVideoPlayer.Pause();
							videoPlayers[2].gameObject.transform.position = new Vector3(90f, 0f, 0f);
						}
						break;
					case 3:
						resetTargetModel(4);
						if (itm.activeTarget4 == false && itm.target4.activeSelf == true)
						{
							videoPlayers[3].GetComponent<SimplePlayback>().unityVideoPlayer.Pause();
							videoPlayers[3].gameObject.transform.position = new Vector3(90f, 0f, 0f);
						}
						break;
					case 4:
						resetTargetModel(5);
						if (itm.activeTarget5 == false && itm.target5.activeSelf == true)
						{
							videoPlayers[4].GetComponent<SimplePlayback>().unityVideoPlayer.Pause();
							videoPlayers[4].gameObject.transform.position = new Vector3(90f, 0f, 0f);
						}
						break;
				}
				videoPlayer.SetActive(true);
				targetMarker.SetActive(false);
				image.SetActive(false);
				
			}
		}
	}

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

	public void rotateTargetObject()
	{
		//do nothing if there is no target object 
		if(fm.targetStatus[fm.currentTarget-1] == "none" || fm.targetStatus[fm.currentTarget-1] == "created")
			return;

		switch(fm.currentTarget)
		{
			case 1:
				if (fm.targetStatus[fm.currentTarget-1] == "video")
				{
					videoPlayers[0].transform.Rotate(0,0,30);
				}
				if (fm.targetStatus[fm.currentTarget-1] == "model")
				{
					models[0].transform.Rotate(30,0,0);
				}
				if (fm.targetStatus[fm.currentTarget-1] == "image")
				{
					images[0].transform.Rotate(0,0,30);
				}
				break;
			case 2:
				if (fm.targetStatus[fm.currentTarget-1] == "video")
				{
					videoPlayers[1].transform.Rotate(0,0,30);
				}
				if (fm.targetStatus[fm.currentTarget-1] == "model")
				{
					models[1].transform.Rotate(30,0,0);
				}
				if (fm.targetStatus[fm.currentTarget-1] == "image")
				{
					images[1].transform.Rotate(0,0,30);
				}
				break;
			case 3:
				if (fm.targetStatus[fm.currentTarget-1] == "video")
				{
					videoPlayers[2].transform.Rotate(0,0,30);
				}
				if (fm.targetStatus[fm.currentTarget-1] == "model")
				{
					models[2].transform.Rotate(30,0,0);
				}
				if (fm.targetStatus[fm.currentTarget-1] == "image")
				{
					images[2].transform.Rotate(0,0,30);
				}
				break;
			case 4:
				if (fm.targetStatus[fm.currentTarget-1] == "video")
				{
					videoPlayers[3].transform.Rotate(0,0,30);
				}
				if (fm.targetStatus[fm.currentTarget-1] == "model")
				{
					models[3].transform.Rotate(30,0,0);
				}
				if (fm.targetStatus[fm.currentTarget-1] == "image")
				{
					images[3].transform.Rotate(0,0,30);
				}
				break;
			case 5:
				if (fm.targetStatus[fm.currentTarget-1] == "video")
				{
					videoPlayers[4].transform.Rotate(0,0,30);
				}
				if (fm.targetStatus[fm.currentTarget-1] == "model")
				{
					models[4].transform.Rotate(30,0,0);
				}
				if (fm.targetStatus[fm.currentTarget-1] == "image")
				{
					images[4].transform.Rotate(0,0,30);
				}
				break;
		}
	}

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

	///	targetObjectThumbnails[0].sprite = blankSprite.sprite;
	///	targetObjectThumbnails[1].sprite = blankSprite.sprite;
	///	targetObjectThumbnails[2].sprite = blankSprite.sprite;
	///	targetObjectThumbnails[3].sprite = blankSprite.sprite;
	///	targetObjectThumbnails[4].sprite = blankSprite.sprite;

		removeTargetObject(1);
		removeTargetObject(2);
		removeTargetObject(3);
		removeTargetObject(4);
		removeTargetObject(5);
      
	}
}
