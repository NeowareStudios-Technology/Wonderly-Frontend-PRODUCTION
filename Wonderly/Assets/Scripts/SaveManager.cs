/******************************************************
*Project: Wonderly
*Created by: David Lee Ramirez
*Date: 12/28/18
*Description: Handles creation of save files of journeys
							for eventual saving in Firebase Storage.
*Copyright 2018 LeapWithAlice,LLC. All rights reserved
 ******************************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class SaveManager : MonoBehaviour {
	//script references 
	public FilesManager fm;
	public targetObjectManager tom;
	public pixabayManager pm;
	public FirebaseStorageManager fsm;
	public CloudEndpointsApiManager ceam;
	public LoadManager lm;
	public ArPairDisplayManager apdm;
	//reference to localScriptHolder gameObject
	public GameObject lsh;
	//template class for save file (gets converted to JSON)
	public SaveClassDeclaration save;
	//UI refrences to journey titles and description 
	public InputField title;
	public Text editTitle;
	public InputField description;
	//the below 2 are for the edit title/description screen marked SINGLE
	public InputField editTitle2;
	public InputField editDescription2;
	//display for journey title and description 
	public Text titleDisplay;
	public Text descriptionDisplay;
	public InputField wonderTitle;
	public InputField wonderDescription;
	//the below 2 are for the edit title/description screen marked SINGLE
	public InputField wonderTitle2;
	public InputField wonderDescription2;
	//holds all wonder titles and descriptions to be saved
	public string[] wonderTitles = new string[5];
	public string[] wonderDescriptions = new string[5];


	//add this function to button that saves/sets wonders
	public void LocalSaveWonderTitleDesc()
	{

			if (fm.currentTarget < 0)
			{
				Debug.Log("current target index out of range");
				return;
			}
		
			//locally save Wonder titles and descriptions
			wonderTitles[fm.currentTarget-1] = wonderTitle.text;
			wonderDescriptions[fm.currentTarget-1] = wonderDescription.text;

			//display Wonder titles and descriptions
			apdm.wonderTitles[fm.currentTarget-1].text = wonderTitle.text;
			apdm.wonderDescriptions[fm.currentTarget-1].text = wonderDescription.text;

			wonderTitle.text = "";
			wonderDescription.text = "";
	}


	//add this function to button that saves/sets wonders
	public void LocalSaveWonderTitleDescManualIndex(int whichTarget)
	{

			//locally save Wonder titles and descriptions
			wonderTitles[whichTarget-1] = wonderTitle2.text;
			wonderDescriptions[whichTarget-1] = wonderDescription2.text;

			//display Wonder titles and descriptions
			apdm.wonderTitles[whichTarget-1].text = wonderTitle2.text;
			apdm.wonderDescriptions[whichTarget-1].text = wonderDescription2.text;

			wonderTitle2.text = "";
			wonderDescription2.text = "";
	}


	public void SetJourneyTitleDescription()
	{
		titleDisplay.text = title.text;
		descriptionDisplay.text = description.text;
	}


	public void SetEditedJourneyTitleDescription()
	{
		titleDisplay.text = editTitle2.text;
		descriptionDisplay.text = editDescription2.text;
	}


	//creates save file JSON from save file class
	public void CreateSaveFile()
	{
		lsh.GetComponent<UiManager>().SetLoadingPanelActive(true);
		Debug.Log("creating normal save file");

		Debug.Log("1. Starting SaveManager.CreateSaveFile()...");
		//delete the previous save
		//deleteOldSave();

		//create a new save class instance
		save = new SaveClassDeclaration();
		save.targetNum = fm.targetCount;
		save.targetStatus = fm.targetStatus;
		Debug.Log("2. number of targets being saved (first element to be saved): " +save.targetNum);

		//save the title and description of the experience
		save.title = title.text;
		save.description = description.text;

		//save cover photo url
		save.coverImageUrl = pm.chosenCoverImageUrl;
		if (save.coverImageUrl == "")
			save.coverImageUrl = "none";

		//save each wonder title and description
		for (int k = 0; k< 5; k++)
		{
			save.wonderTitle[k] = wonderTitles[k];
			save.wonderDescription[k] = wonderDescriptions[k];
		}

		//create save directory
		Directory.CreateDirectory(fm.SaveDirectory);
		Debug.Log("3. Save directory being created: " + fm.SaveDirectory);

		//copy working directory target photos to save directory
		string targetPath1 = Path.Combine(fm.MarksDirectory, "targetPhoto1.jpg");
		string targetPath2 = Path.Combine(fm.MarksDirectory, "targetPhoto2.jpg");
		string targetPath3 = Path.Combine(fm.MarksDirectory, "targetPhoto3.jpg");
		string targetPath4 = Path.Combine(fm.MarksDirectory, "targetPhoto4.jpg");
		string targetPath5 = Path.Combine(fm.MarksDirectory, "targetPhoto5.jpg");
		string destPath1 = Path.Combine(fm.SaveDirectory, "targetPhoto1.jpg");
		string destPath2 = Path.Combine(fm.SaveDirectory, "targetPhoto2.jpg");
		string destPath3 = Path.Combine(fm.SaveDirectory, "targetPhoto3.jpg");
		string destPath4 = Path.Combine(fm.SaveDirectory, "targetPhoto4.jpg");
		string destPath5 = Path.Combine(fm.SaveDirectory, "targetPhoto5.jpg");
		
		if (File.Exists(targetPath1))
			System.IO.File.Copy(targetPath1, destPath1, true);
		if (File.Exists(targetPath2))
			System.IO.File.Copy(targetPath2, destPath2, true);
		if (File.Exists(targetPath3))
			System.IO.File.Copy(targetPath3, destPath3, true);
		if (File.Exists(targetPath4))
			System.IO.File.Copy(targetPath4, destPath4, true);
		if (File.Exists(targetPath5))
			System.IO.File.Copy(targetPath5, destPath5, true);


		//make sure to iterate over all 5 possible targets to make sure we get all targets if one has been deleted
		for (int i = 0; i < 5; i++)
		{
			//only save this target if the it has objects save to it
			if (fm.targetStatus[i] != "none" || fm.targetStatus[i] != "created")
			{
				//declare here so that duplicate naming errors dont occur
				float x = 0.0f;
				float y = 0.0f;
				float z = 0.0f;
				float scaleX = 0.0f;
				float scaleY = 0.0f;
				float scaleZ = 0.0f;

				//check whether a video, model, or image is set to the target
				switch(fm.targetStatus[i])
				{
					//save all video url info and position info to save class
					case "video":
						//declare here so that duplicate naming errors dont occur
						string videoUrl = "";
						string audioUrl = "";
						//save video id
						save.vId[i] = tom.videoPlayers[i].GetComponent<SimplePlayback>().videoId;
						//get rotation of video player
						x = tom.videoPlayers[i].transform.rotation.eulerAngles.x;
						y = tom.videoPlayers[i].transform.rotation.eulerAngles.y;
						z = tom.videoPlayers[i].transform.rotation.eulerAngles.z;
						//get scale of video playei
						scaleX = tom.videoPlayers[i].transform.localScale.x;
						scaleY = tom.videoPlayers[i].transform.localScale.y;
						scaleZ = tom.videoPlayers[i].transform.localScale.z;
						switch(i)
						{
							case 0:
								//save rotation of video player
								save.rot1[0] = x;
								save.rot1[1] = y;
								save.rot1[2] = z;
								//save scale of video player
								save.scale1[0]= scaleX;
								save.scale1[1] = scaleY;
								save.scale1[2] = scaleZ;
								break;

							case 1:
								//save rotation of video player
								save.rot2[0] = x;
								save.rot2[1] = y;
								save.rot2[2] = z;
								//save scale of video player
								save.scale2[0]= scaleX;
								save.scale2[1] = scaleY;
								save.scale2[2] = scaleZ;
								break;

							case 2:
								//save rotation of video player
								save.rot3[0] = x;
								save.rot3[1] = y;
								save.rot3[2] = z;
								//save scale of video player
								save.scale3[0]= scaleX;
								save.scale3[1] = scaleY;
								save.scale3[2] = scaleZ;
								break;

							case 3:
								//save rotation of video player
								save.rot4[0] = x;
								save.rot4[1] = y;
								save.rot4[2] = z;
								//save scale of video player
								save.scale4[0]= scaleX;
								save.scale4[1] = scaleY;
								save.scale4[2] = scaleZ;
								break;
							
						case 4:
								//save rotation of video player
								save.rot5[0] = x;
								save.rot5[1] = y;
								save.rot5[2] = z;
								//save scale of video player
								save.scale5[0]= scaleX;
								save.scale5[1] = scaleY;
								save.scale5[2] = scaleZ;
								break;

						}
						break;

					//save model ID info and model rotation info to save class
					case "model":
						string modelId = "";
						save.modId[i] = tom.modelIds[i];
						x = tom.models[i].transform.rotation.eulerAngles.x;
						y = tom.models[i].transform.rotation.eulerAngles.y;
						z = tom.models[i].transform.rotation.eulerAngles.z;
						scaleX = tom.models[i].transform.localScale.x;
						scaleY = tom.models[i].transform.localScale.y;
						scaleZ = tom.models[i].transform.localScale.z;
						switch(i)
						{
							case 0:
								//save rotation of video player
								save.rot1[0] = x;
								save.rot1[1] = y;
								save.rot1[2] = z;
								//save scale of video player
								save.scale1[0]= scaleX;
								save.scale1[1] = scaleY;
								save.scale1[2] = scaleZ;
								break;

							case 1:
								//save rotation of video player
								save.rot2[0] = x;
								save.rot2[1] = y;
								save.rot2[2] = z;
								//save scale of video player
								save.scale2[0]= scaleX;
								save.scale2[1] = scaleY;
								save.scale2[2] = scaleZ;
								break;

							case 2:
								//save rotation of video player
								save.rot3[0] = x;
								save.rot3[1] = y;
								save.rot3[2] = z;
								//save scale of video player
								save.scale3[0]= scaleX;
								save.scale3[1] = scaleY;
								save.scale3[2] = scaleZ;
								break;

							case 3:
								//save rotation of video player
								save.rot4[0] = x;
								save.rot4[1] = y;
								save.rot4[2] = z;
								//save scale of video player
								save.scale4[0]= scaleX;
								save.scale4[1] = scaleY;
								save.scale4[2] = scaleZ;
								break;
							
						case 4:
								//save rotation of video player
								save.rot5[0] = x;
								save.rot5[1] = y;
								save.rot5[2] = z;
								//save scale of video player
								save.scale5[0]= scaleX;
								save.scale5[1] = scaleY;
								save.scale5[2] = scaleZ;
								break;

						}
						break;

					case "image":
						save.imageUrl[i] = pm.chosenUrls[i];
						x = tom.images[i].transform.rotation.eulerAngles.x;
						y = tom.images[i].transform.rotation.eulerAngles.y;
						z = tom.images[i].transform.rotation.eulerAngles.z;
						scaleX = tom.images[i].transform.localScale.x;
						scaleY = tom.images[i].transform.localScale.y;
						scaleZ = tom.images[i].transform.localScale.z;
						switch(i)
						{
							case 0:
								//save rotation of video player
								save.rot1[0] = x;
								save.rot1[1] = y;
								save.rot1[2] = z;
								//save scale of video player
								save.scale1[0]= scaleX;
								save.scale1[1] = scaleY;
								save.scale1[2] = scaleZ;
								break;

							case 1:
								//save rotation of video player
								save.rot2[0] = x;
								save.rot2[1] = y;
								save.rot2[2] = z;
								//save scale of video player
								save.scale2[0]= scaleX;
								save.scale2[1] = scaleY;
								save.scale2[2] = scaleZ;
								break;

							case 2:
								//save rotation of video player
								save.rot3[0] = x;
								save.rot3[1] = y;
								save.rot3[2] = z;
								//save scale of video player
								save.scale3[0]= scaleX;
								save.scale3[1] = scaleY;
								save.scale3[2] = scaleZ;
								break;

							case 3:
								//save rotation of video player
								save.rot4[0] = x;
								save.rot4[1] = y;
								save.rot4[2] = z;
								//save scale of video player
								save.scale4[0]= scaleX;
								save.scale4[1] = scaleY;
								save.scale4[2] = scaleZ;
								break;
							
						case 4:
								//save rotation of video player
								save.rot5[0] = x;
								save.rot5[1] = y;
								save.rot5[2] = z;
								//save scale of video player
								save.scale5[0]= scaleX;
								save.scale5[1] = scaleY;
								save.scale5[2] = scaleZ;
								break;

						}
						break;
				}
			}
		}
		string saveFilePath = Path.Combine(fm.SaveDirectory, "aoSave.json");
		string thisSave = JsonUtility.ToJson(save);
		File.WriteAllText(saveFilePath, thisSave);

		Debug.Log("4. AR Experience Being Saved...");
		if (File.Exists(saveFilePath))
		{
			Debug.Log("5. Save file created!");
		}
		else
		{
			Debug.Log("5. Save file could not be created.");
		}

		if (File.Exists(saveFilePath))
			Debug.Log("**0** sm 266, Save file exists: "+saveFilePath);
		else	
			Debug.Log("**0** sm 266, Save file missing: "+saveFilePath);

		fsm.startExperienceUpload();

		//clear out local temp save of wonder titles/descriptions to be ready for next Journey creation
		for (int m = 0; m < 5; m++)
		{
			wonderTitles[m] = "";
			wonderDescriptions[m] = "";
		}

		pm.coverImage.sprite = pm.blankImage.sprite;
	}




	public void CreateSaveFileForEdit()
	{
		lsh.GetComponent<UiManager>().SetLoadingPanelActive(true);
		Debug.Log("creating save file for edit");

		//create a new save class instance
		save = new SaveClassDeclaration();
		save.targetNum = fm.targetCount;
		save.targetStatus = fm.targetStatus;

		//save the title and description of the experience
		save.title = editTitle.text;
		save.description = description.text;

		//save cover photo url
		save.coverImageUrl = pm.chosenCoverImageUrl;
		if (save.coverImageUrl == "")
			save.coverImageUrl = "none";

		for (int k = 0; k< 5; k++)
		{
			save.wonderTitle[k] = wonderTitles[k];
			save.wonderDescription[k] = wonderDescriptions[k];
		}


		//create save directory
		Directory.CreateDirectory(fm.SaveDirectory);

		//copy working directory target photos to save directory
		string targetPath1 = Path.Combine(fm.MarksDirectory, "targetPhoto1.jpg");
		string targetPath2 = Path.Combine(fm.MarksDirectory, "targetPhoto2.jpg");
		string targetPath3 = Path.Combine(fm.MarksDirectory, "targetPhoto3.jpg");
		string targetPath4 = Path.Combine(fm.MarksDirectory, "targetPhoto4.jpg");
		string targetPath5 = Path.Combine(fm.MarksDirectory, "targetPhoto5.jpg");
		string destPath1 = Path.Combine(fm.SaveDirectory, "targetPhoto1.jpg");
		string destPath2 = Path.Combine(fm.SaveDirectory, "targetPhoto2.jpg");
		string destPath3 = Path.Combine(fm.SaveDirectory, "targetPhoto3.jpg");
		string destPath4 = Path.Combine(fm.SaveDirectory, "targetPhoto4.jpg");
		string destPath5 = Path.Combine(fm.SaveDirectory, "targetPhoto5.jpg");
		
		if (File.Exists(targetPath1))
			System.IO.File.Copy(targetPath1, destPath1, true);
		if (File.Exists(targetPath2))
			System.IO.File.Copy(targetPath2, destPath2, true);
		if (File.Exists(targetPath3))
			System.IO.File.Copy(targetPath3, destPath3, true);
		if (File.Exists(targetPath4))
			System.IO.File.Copy(targetPath4, destPath4, true);
		if (File.Exists(targetPath5))
			System.IO.File.Copy(targetPath5, destPath5, true);


		//make sure to iterate over all 5 possible targets to make sure we get all targets if one has been deleted
		for (int i = 0; i < 5; i++)
		{

			//only save this target if the it has objects save to it
			if (fm.targetStatus[i] != "none" || fm.targetStatus[i] != "created")
			{
				//declare here so that duplicate naming errors dont occur
				float x = 0.0f;
				float y = 0.0f;
				float z = 0.0f;
				float scaleX = 0.0f;
				float scaleY = 0.0f;
				float scaleZ = 0.0f;

				//check whether a video, model, or image is set to the target
				switch(fm.targetStatus[i])
				{
					//save all video url info and position info to save class
					case "video":
						//declare here so that duplicate naming errors dont occur
						string videoUrl = "";
						string audioUrl = "";
						//save video id
						save.vId[i] = tom.videoPlayers[i].GetComponent<SimplePlayback>().videoId;
						//get rotation of video player
						x = tom.videoPlayers[i].transform.rotation.eulerAngles.x;
						y = tom.videoPlayers[i].transform.rotation.eulerAngles.y;
						z = tom.videoPlayers[i].transform.rotation.eulerAngles.z;
						//get scale of video playei
						scaleX = tom.videoPlayers[i].transform.localScale.x;
						scaleY = tom.videoPlayers[i].transform.localScale.y;
						scaleZ = tom.videoPlayers[i].transform.localScale.z;
						switch(i)
						{
							case 0:
								//save rotation of video player
								save.rot1[0] = x;
								save.rot1[1] = y;
								save.rot1[2] = z;
								//save scale of video player
								save.scale1[0]= scaleX;
								save.scale1[1] = scaleY;
								save.scale1[2] = scaleZ;
								break;

							case 1:
								//save rotation of video player
								save.rot2[0] = x;
								save.rot2[1] = y;
								save.rot2[2] = z;
								//save scale of video player
								save.scale2[0]= scaleX;
								save.scale2[1] = scaleY;
								save.scale2[2] = scaleZ;
								break;

							case 2:
								//save rotation of video player
								save.rot3[0] = x;
								save.rot3[1] = y;
								save.rot3[2] = z;
								//save scale of video player
								save.scale3[0]= scaleX;
								save.scale3[1] = scaleY;
								save.scale3[2] = scaleZ;
								break;

							case 3:
								//save rotation of video player
								save.rot4[0] = x;
								save.rot4[1] = y;
								save.rot4[2] = z;
								//save scale of video player
								save.scale4[0]= scaleX;
								save.scale4[1] = scaleY;
								save.scale4[2] = scaleZ;
								break;
							
						case 4:
								//save rotation of video player
								save.rot5[0] = x;
								save.rot5[1] = y;
								save.rot5[2] = z;
								//save scale of video player
								save.scale5[0]= scaleX;
								save.scale5[1] = scaleY;
								save.scale5[2] = scaleZ;
								break;

						}
						break;

					//save model ID info and model rotation info to save class
					case "model":
						string modelId = "";
						save.modId[i] = tom.modelIds[i];
						x = tom.models[i].transform.rotation.eulerAngles.x;
						y = tom.models[i].transform.rotation.eulerAngles.y;
						z = tom.models[i].transform.rotation.eulerAngles.z;
						scaleX = tom.models[i].transform.localScale.x;
						scaleY = tom.models[i].transform.localScale.y;
						scaleZ = tom.models[i].transform.localScale.z;
						switch(i)
						{
							case 0:
								//save rotation of video player
								save.rot1[0] = x;
								save.rot1[1] = y;
								save.rot1[2] = z;
								//save scale of video player
								save.scale1[0]= scaleX;
								save.scale1[1] = scaleY;
								save.scale1[2] = scaleZ;
								break;

							case 1:
								//save rotation of video player
								save.rot2[0] = x;
								save.rot2[1] = y;
								save.rot2[2] = z;
								//save scale of video player
								save.scale2[0]= scaleX;
								save.scale2[1] = scaleY;
								save.scale2[2] = scaleZ;
								break;

							case 2:
								//save rotation of video player
								save.rot3[0] = x;
								save.rot3[1] = y;
								save.rot3[2] = z;
								//save scale of video player
								save.scale3[0]= scaleX;
								save.scale3[1] = scaleY;
								save.scale3[2] = scaleZ;
								break;

							case 3:
								//save rotation of video player
								save.rot4[0] = x;
								save.rot4[1] = y;
								save.rot4[2] = z;
								//save scale of video player
								save.scale4[0]= scaleX;
								save.scale4[1] = scaleY;
								save.scale4[2] = scaleZ;
								break;
							
						case 4:
								//save rotation of video player
								save.rot5[0] = x;
								save.rot5[1] = y;
								save.rot5[2] = z;
								//save scale of video player
								save.scale5[0]= scaleX;
								save.scale5[1] = scaleY;
								save.scale5[2] = scaleZ;
								break;

						}
						break;

					case "image":
						save.imageUrl[i] = pm.chosenUrls[i];
						x = tom.images[i].transform.rotation.eulerAngles.x;
						y = tom.images[i].transform.rotation.eulerAngles.y;
						z = tom.images[i].transform.rotation.eulerAngles.z;
						scaleX = tom.images[i].transform.localScale.x;
						scaleY = tom.images[i].transform.localScale.y;
						scaleZ = tom.images[i].transform.localScale.z;
						switch(i)
						{
							case 0:
								//save rotation of video player
								save.rot1[0] = x;
								save.rot1[1] = y;
								save.rot1[2] = z;
								//save scale of video player
								save.scale1[0]= scaleX;
								save.scale1[1] = scaleY;
								save.scale1[2] = scaleZ;
								break;

							case 1:
								//save rotation of video player
								save.rot2[0] = x;
								save.rot2[1] = y;
								save.rot2[2] = z;
								//save scale of video player
								save.scale2[0]= scaleX;
								save.scale2[1] = scaleY;
								save.scale2[2] = scaleZ;
								break;

							case 2:
								//save rotation of video player
								save.rot3[0] = x;
								save.rot3[1] = y;
								save.rot3[2] = z;
								//save scale of video player
								save.scale3[0]= scaleX;
								save.scale3[1] = scaleY;
								save.scale3[2] = scaleZ;
								break;

							case 3:
								//save rotation of video player
								save.rot4[0] = x;
								save.rot4[1] = y;
								save.rot4[2] = z;
								//save scale of video player
								save.scale4[0]= scaleX;
								save.scale4[1] = scaleY;
								save.scale4[2] = scaleZ;
								break;
							
						case 4:
								//save rotation of video player
								save.rot5[0] = x;
								save.rot5[1] = y;
								save.rot5[2] = z;
								//save scale of video player
								save.scale5[0]= scaleX;
								save.scale5[1] = scaleY;
								save.scale5[2] = scaleZ;
								break;

						}
						break;
				}
			}
		}
		string saveFilePath = Path.Combine(fm.SaveDirectory, "aoSave.json");
		string thisSave = JsonUtility.ToJson(save);
		File.WriteAllText(saveFilePath, thisSave);

		Debug.Log("AR Experience Saved");

		ceam.startExperienceEdit2();

		//clear out local temp save of wonder titles/descriptions to be ready for next Journey creation
		for (int m = 0; m < 5; m++)
		{
			wonderTitles[m] = "";
			wonderDescriptions[m] = "";
		}
		pm.coverImage.sprite = pm.blankImage.sprite;
	}


	//deletes old save file
	public void deleteOldSave() 
	{
		if (Directory.Exists(fm.SaveDirectory))
			Directory.Delete(fm.SaveDirectory, true);
	}

}