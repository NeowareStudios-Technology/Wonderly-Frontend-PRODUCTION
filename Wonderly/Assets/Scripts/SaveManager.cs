using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using Sample;

public class SaveManager : MonoBehaviour {

	public FilesManager fm;
	public targetObjectManager tom;
	public pixabayManager pm;
	public FirebaseStorageManager fsm;
	public CloudEndpointsApiManager ceam;
	public LoadManager lm;

	public InputField title;
	public InputField editTitle;
	public InputField description;
	public InputField titleDisplay;
	///Probably need to make a function that controls which wonder title/description screen to display
	public InputField[] wonderTitles = new InputField[5];
	public InputField[] wonderDescriptions = new InputField[5];

	public Text coverImageUrl;



	public void CreateSaveFile()
	{
		Debug.Log("1. Starting SaveManager.CreateSaveFile()...");
		//delete the previous save
		deleteOldSave();

		//create a new save class instance
		SaveClassDeclaration save = new SaveClassDeclaration();
		save.targetNum = fm.targetCount;
		save.targetStatus = fm.targetStatus;
		Debug.Log("2. number of targets being saved (first element to be saved): " +save.targetNum);

		//save the title and description of the experience
		save.title = title.text;
		save.description = description.text;
		titleDisplay.text = title.text;

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
				//save titles of each wonder
				save.wonderTitle[i] = wonderTitles[i].text;
				//save descriptions of each wonder
				save.wonderDescription[i] = wonderDescriptions[i].text; 

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
						//save rotation of video player
						save.rotationObjectArray[i].rotation[0] = x;
						save.rotationObjectArray[i].rotation[1] = y;
						save.rotationObjectArray[i].rotation[2] = z;
						//save scale of video player
						save.scaleObjectArray[i].scale[0] = scaleX;
						save.scaleObjectArray[i].scale[1] = scaleY;
						save.scaleObjectArray[i].scale[2] = scaleZ;
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
						save.rotationObjectArray[i].rotation[0] = x;
						save.rotationObjectArray[i].rotation[1] = y;
						save.rotationObjectArray[i].rotation[2] = z;
						save.scaleObjectArray[i].scale[0] = scaleX;
						save.scaleObjectArray[i].scale[1] = scaleY;
						save.scaleObjectArray[i].scale[2] = scaleZ;
						break;

					case "image":
						save.imageUrl[i] = pm.chosenUrls[i];
						x = tom.images[i].transform.rotation.eulerAngles.x;
						y = tom.images[i].transform.rotation.eulerAngles.y;
						z = tom.images[i].transform.rotation.eulerAngles.z;
						scaleX = tom.images[i].transform.localScale.x;
						scaleY = tom.images[i].transform.localScale.y;
						scaleZ = tom.images[i].transform.localScale.z;
						save.rotationObjectArray[i].rotation[0] = x;
						save.rotationObjectArray[i].rotation[1] = y;
						save.rotationObjectArray[i].rotation[2] = z;
						save.scaleObjectArray[i].scale[0] = scaleX;
						save.scaleObjectArray[i].scale[1] = scaleY;
						save.scaleObjectArray[i].scale[2] = scaleZ;
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
	}




	public void CreateSaveFileForEdit()
	{
		//delete the previous save
		deleteOldSave();

		//create a new save class instance
		SaveClassDeclaration save = new SaveClassDeclaration();
		save.targetNum = fm.targetCount;
		save.targetStatus = fm.targetStatus;

		//save the title and description of the experience
		save.title = editTitle.text;
		save.description = description.text;

		titleDisplay.text = editTitle.text;



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
				//save titles of each wonder
				save.wonderTitle[i] = wonderTitles[i].text;
				//save descriptions of each wonder
				save.wonderDescription[i] = wonderDescriptions[i].text; 

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
						//save rotation of video player
						save.rotationObjectArray[i].rotation[0] = x;
						save.rotationObjectArray[i].rotation[1] = y;
						save.rotationObjectArray[i].rotation[2] = z;
						//save scale of video player
						save.scaleObjectArray[i].scale[0] = scaleX;
						save.scaleObjectArray[i].scale[1] = scaleY;
						save.scaleObjectArray[i].scale[2] = scaleZ;
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
						save.rotationObjectArray[i].rotation[0] = x;
						save.rotationObjectArray[i].rotation[1] = y;
						save.rotationObjectArray[i].rotation[2] = z;
						save.scaleObjectArray[i].scale[0] = scaleX;
						save.scaleObjectArray[i].scale[1] = scaleY;
						save.scaleObjectArray[i].scale[2] = scaleZ;
						break;

					case "image":
						save.imageUrl[i] = pm.chosenUrls[i];
						x = tom.images[i].transform.rotation.eulerAngles.x;
						y = tom.images[i].transform.rotation.eulerAngles.y;
						z = tom.images[i].transform.rotation.eulerAngles.z;
						scaleX = tom.images[i].transform.localScale.x;
						scaleY = tom.images[i].transform.localScale.y;
						scaleZ = tom.images[i].transform.localScale.z;
						save.rotationObjectArray[i].rotation[0] = x;
						save.rotationObjectArray[i].rotation[1] = y;
						save.rotationObjectArray[i].rotation[2] = z;
						save.scaleObjectArray[i].scale[0] = scaleX;
						save.scaleObjectArray[i].scale[1] = scaleY;
						save.scaleObjectArray[i].scale[2] = scaleZ;
						break;
				}
			}
		}
		string saveFilePath = Path.Combine(fm.SaveDirectory, "aoSave.json");
		string thisSave = JsonUtility.ToJson(save);
		File.WriteAllText(saveFilePath, thisSave);

		Debug.Log("AR Experience Saved");

		ceam.startExperienceEdit2();
	}

	public void deleteOldSave() 
	{
		if (Directory.Exists(fm.SaveDirectory))
			Directory.Delete(fm.SaveDirectory, true);
	}

}