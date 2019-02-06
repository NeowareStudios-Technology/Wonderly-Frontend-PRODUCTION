/******************************************************
*Project: Wonderly
*Created by: David Lee Ramirez
*Date: 12/28/18
*Description: Used for handling Firebase Storage file transfers.
*Copyright 2018 LeapWithAlice,LLC. All rights reserved
 ******************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using UnityEngine.Networking;
using System.IO;
using Firebase;
using Firebase.Auth;
using Firebase.Unity.Editor;
using Firebase.Storage;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class FirebaseStorageManager : MonoBehaviour {
	//script references
	public FirebaseManager fbm;
	public FilesManager fm;
	public SaveManager sm;
	public ImageTargetManager itm;
	public CloudEndpointsApiManager ceam;
	public LoadManager lm;
	public ExperienceCodeClass ecc;
	//for accessing local script holder GameObject
	public GameObject lsh;
	//UI element
	public Text codeDisplay;
	//stores code of journey that user decides to edit
	public string editCode;
	//save directory/ save file paths
	public string saveFolderPath;
	public string saveFilePath;
	//all possible target paths 
	private string targetPath1;
	private string targetPath2;
	private string targetPath3;
	private string targetPath4;
	private string targetPath5;
	//path for cover Image
	private string coverImage;
	//paths for any linked AR images (from pixabay)
	private string linkedImage1;
	private string linkedImage2;
	private string linkedImage3;
	private string linkedImage4;
	private string linkedImage5;
	//notification if code search invalid
	public GameObject wrongCodeNotification;
	//UI elements for view screen buttons and coachmarks
	public GameObject loadPanel;
	private int whichIndex;
	public string expCode;
	//url for saving journey in google datastore db
	private string saveApiUrl = "https://wonderly-225214.appspot.com/_ah/api/wonderly/v1/exp";
	//keeps track of web call retries
	private int experienceUploadCallCount = 0;


	//initializationof path strings
	void Start () {
		saveFolderPath = Path.Combine(fm.MarksDirectory, "SaveFolder");
		
		saveFilePath = Path.Combine(saveFolderPath, "aoSave.json");

		targetPath1 = Path.Combine(saveFolderPath, "targetPhoto1.jpg");
		targetPath2 = Path.Combine(saveFolderPath, "targetPhoto2.jpg");
		//targetPath3 = Path.Combine(saveFolderPath, "targetPhoto3.jpg");
		//targetPath4 = Path.Combine(saveFolderPath, "targetPhoto4.jpg");
		//targetPath5 = Path.Combine(saveFolderPath, "targetPhoto5.jpg");

		coverImage = Path.Combine(fm.SaveDirectory, "coverImage.jpg");
		linkedImage1 = Path.Combine(fm.SaveDirectory, "linkedImage1.jpg");
		linkedImage2 = Path.Combine(fm.SaveDirectory, "linkedImage2.jpg");
		//linkedImage3 = Path.Combine(fm.SaveDirectory, "linkedImage3.jpg");
		//linkedImage4 = Path.Combine(fm.SaveDirectory, "linkedImage4.jpg");
		//linkedImage5 = Path.Combine(fm.SaveDirectory, "linkedImage5.jpg");
	}

	//starts experience upload to Google Cloud Datastore via IEnumerator (startExperienceUpload->experienceUpload->UploadExperienceFiles)
	public void startExperienceUpload()
	{
		if (File.Exists(saveFilePath))
			Debug.Log("**1** fsm 65, Save file exists: "+saveFilePath);
		else	
			Debug.Log("**1** fsm 67, Save file missing: "+saveFilePath);
		StartCoroutine("experienceUpload");
	}

	//uploads experience meta data to Cloud Datastore using Cloud Endpoints AliceOne API
	public IEnumerator experienceUpload () {
		if (File.Exists(saveFilePath))
			Debug.Log("**2** fsm 74, Save file exists: "+saveFilePath);
		else	
			Debug.Log("**2** fsm 76, Save file missing: "+saveFilePath);

		//Debug.Log("1. Starting fsm.experienceUpload()...");
		//reset code display
		//codeDisplay.text = "Loading...";
		//create a new upload class instance
		UploadClassDeclaration upload = new UploadClassDeclaration();
		upload.title = sm.title.text;
		upload.coverImage = sm.save.coverImageUrl;

		//get target object counts
		int modelCount = 0;
		int videoCount = 0;
		int imageCount = 0;
		for(int i= 0; i < 2; i++)
		{
			switch(fm.targetStatus[i])
			{
				case "model":
					modelCount++;
					break;
				case "video":
					videoCount++;
					break;
				case "image":
					imageCount++;
					break;
			}

			if (fm.targetStatus[i] != "none")
			{
				switch(i)
				{
					case 0:
						upload.t1 = true;
						break;
					case 1:
						upload.t2 = true;
						break;
					case 2:
						upload.t3 = true;
						break;
					case 3:
						upload.t4 = true;
						break;
					case 4:
						upload.t5 = true;
						break;
				}
			}
			else
			{
				switch(i)
				{
					case 0:
						upload.t1 = false;
						break;
					case 1:
						upload.t2 = false;
						break;
					case 2:
						upload.t3 = false;
						break;
					case 3:
						upload.t4 = false;
						break;
					case 4:
						upload.t5 = false;
						break;
				}
			}
		}
		//Debug.Log(modelCount);
		upload.model = modelCount;
		upload.video = videoCount;
		upload.image = imageCount;

		//convert upload clas instance into json string
		string uploadJson = JsonUtility.ToJson(upload);
		if (File.Exists(saveFilePath))
			Debug.Log("**3** fsm 164, Save file exists: "+saveFilePath);
		else	
			Debug.Log("**3** fsm 165, Save file missing: "+saveFilePath);

		//Debug.Log("2. Experience info being saved to Goodle Datastore: " + uploadJson);

		while (experienceUploadCallCount < 3)
		{
		//create web request
			using (UnityWebRequest experienceUploadRequest = UnityWebRequest.Put(saveApiUrl,uploadJson))
			{
				//set content type
				experienceUploadRequest.SetRequestHeader("Content-Type", "application/json");
				//set auth header
				experienceUploadRequest.SetRequestHeader("Authorization", "Bearer " + fbm.token);
				
				yield return experienceUploadRequest.SendWebRequest();

				//retry call up to 3 times if error
				if (experienceUploadRequest.responseCode != 200 && experienceUploadCallCount < 3)
				{
					experienceUploadCallCount++;
				}
				else if (experienceUploadRequest.responseCode != 200 && experienceUploadCallCount >= 3)
				{
					//Debug.Log("call to backend for profileCreate retries failed");
					experienceUploadCallCount = 3;
				}
				else
				{
					experienceUploadCallCount = 3;
					//load in profile info to ui (called here because need to wait for cookie and profile creation)
					byte[] results = experienceUploadRequest.downloadHandler.data;
					string jsonResponse = Encoding.UTF8.GetString(results);
					//Debug.Log(jsonResponse);
					//Debug.Log("3. Response from cloud endpoints after creating experience data entry: " +jsonResponse);
					ecc = JsonUtility.FromJson<ExperienceCodeClass>(jsonResponse);

					if (File.Exists(saveFilePath))
						Debug.Log("**4** fsm 189, Save file exists: "+saveFilePath);
					else	
						Debug.Log("**4** fsm 191, Save file missing: "+saveFilePath);

					if (ecc.code != "")
					{
						//display code
						codeDisplay.text = ecc.code;
						uploadExperienceFiles();
					}
				}
			}
		}
		experienceUploadCallCount = 0;
	}

	//uploads experience files (target jpg files and save json) to firebase storage (filestore)
	public void uploadExperienceFiles()
	{
		//Debug.Log("1. Starting fsm.uploadExperienceFiles");
		byte[] target1 = new byte[0];
		byte[] target2 = new byte[0];
		byte[] target3 = new byte[0];
		byte[] target4 = new byte[0];
		byte[] target5 = new byte[0];
		byte[] cover = new byte[0];
		byte[] linked1 = new byte[0];
		byte[] linked2 = new byte[0];
		byte[] linked3 = new byte[0];
		byte[] linked4 = new byte[0];
		byte[] linked5 = new byte[0];
		byte[] saveFile = new byte[0];

		if (File.Exists(saveFilePath))
		{	
			//Debug.Log("2. save file exists: "+ saveFilePath);
			saveFile = System.IO.File.ReadAllBytes(saveFilePath);
		}
		else
		{
			//Debug.Log("2. save file not found: "+ saveFilePath);
		}

		Firebase.Storage.StorageReference expRef = fbm.fbStorageRef.Child(ecc.code + "/" + "aoSave.json");
		expRef.PutBytesAsync(saveFile).ContinueWith ((Task<StorageMetadata> task) => {
    if (task.IsFaulted || task.IsCanceled) {
				//Debug.Log("3. Could not upload the save file..");
        //Debug.Log(task.Exception.ToString());
        // Uh-oh, an error occurred!
    } else {
        // Metadata contains file metadata such as size, content-type, and download URL.
        Firebase.Storage.StorageMetadata metadata = task.Result;
        //Debug.Log("3. Finished uploading save file...");
    }
		});

		//convert any target jpegs that exist in save folder to byte arrays
		if (File.Exists(targetPath1))
		{
			target1 = System.IO.File.ReadAllBytes(targetPath1);
			Firebase.Storage.StorageReference target1Ref = fbm.fbStorageRef.Child(ecc.code + "/" + "targetPhoto1.jpg");
			target1Ref.PutBytesAsync(target1).ContinueWith ((Task<StorageMetadata> task) => {
			if (task.IsFaulted || task.IsCanceled) {
					//Debug.Log(task.Exception.ToString());
					// Uh-oh, an error occurred!
			} else {
					// Metadata contains file metadata such as size, content-type, and download URL.
					Firebase.Storage.StorageMetadata metadata = task.Result;
					//Debug.Log("Finished uploading target 1...");
    		}		
			});
		}
		if (File.Exists(targetPath2))
		{
			target2 = System.IO.File.ReadAllBytes(targetPath2);
			Firebase.Storage.StorageReference target2Ref = fbm.fbStorageRef.Child(ecc.code + "/" + "targetPhoto2.jpg");
			target2Ref.PutBytesAsync(target2).ContinueWith ((Task<StorageMetadata> task) => {
			if (task.IsFaulted || task.IsCanceled) {
					//Debug.Log(task.Exception.ToString());
					// Uh-oh, an error occurred!
			} else {
					// Metadata contains file metadata such as size, content-type, and download URL.
					Firebase.Storage.StorageMetadata metadata = task.Result;
					//Debug.Log("Finished uploading target 2...");
    		}		
			});
		}
		/* 
		if (File.Exists(targetPath3))
		{
			target3 = System.IO.File.ReadAllBytes(targetPath3);
			Firebase.Storage.StorageReference target3Ref = fbm.fbStorageRef.Child(ecc.code + "/" + "targetPhoto3.jpg");
			target3Ref.PutBytesAsync(target3).ContinueWith ((Task<StorageMetadata> task) => {
			if (task.IsFaulted || task.IsCanceled) {
					//Debug.Log(task.Exception.ToString());
					// Uh-oh, an error occurred!
			} else {
					// Metadata contains file metadata such as size, content-type, and download URL.
					Firebase.Storage.StorageMetadata metadata = task.Result;
					//Debug.Log("Finished uploading target 3...");
    		}		
			});
		}
		if (File.Exists(targetPath4))
		{
			target4 = System.IO.File.ReadAllBytes(targetPath4);
			Firebase.Storage.StorageReference target4Ref = fbm.fbStorageRef.Child(ecc.code + "/" + "targetPhoto4.jpg");
			target4Ref.PutBytesAsync(target4).ContinueWith ((Task<StorageMetadata> task) => {
			if (task.IsFaulted || task.IsCanceled) {
					//Debug.Log(task.Exception.ToString());
					// Uh-oh, an error occurred!
			} else {
					// Metadata contains file metadata such as size, content-type, and download URL.
					Firebase.Storage.StorageMetadata metadata = task.Result;
					//Debug.Log("Finished uploading target 4...");
    		}		
			});
		}
		if (File.Exists(targetPath5))
		{
			target5 = System.IO.File.ReadAllBytes(targetPath5);
			Firebase.Storage.StorageReference target5Ref = fbm.fbStorageRef.Child(ecc.code + "/" + "targetPhoto5.jpg");
			target5Ref.PutBytesAsync(target5).ContinueWith ((Task<StorageMetadata> task) => {
			if (task.IsFaulted || task.IsCanceled) {
					//Debug.Log(task.Exception.ToString());
					// Uh-oh, an error occurred!
			} else {
					// Metadata contains file metadata such as size, content-type, and download URL.
					Firebase.Storage.StorageMetadata metadata = task.Result;
					//Debug.Log("Finished uploading target 5...");
    		}		
			});
		}
		*/
		if (File.Exists(coverImage))
		{
			cover = System.IO.File.ReadAllBytes(coverImage);
			Firebase.Storage.StorageReference coverRef = fbm.fbStorageRef.Child(ecc.code + "/" + "coverImage.jpg");
			coverRef.PutBytesAsync(cover).ContinueWith ((Task<StorageMetadata> task) => {
			if (task.IsFaulted || task.IsCanceled) {
					//Debug.Log(task.Exception.ToString());
					// Uh-oh, an error occurred!
			} else {
					// Metadata contains file metadata such as size, content-type, and download URL.
					Firebase.Storage.StorageMetadata metadata = task.Result;
					//Debug.Log("Finished uploading cover image...");
    		}		
			});
		}
		
		if (File.Exists(linkedImage1))
		{
			linked1 = System.IO.File.ReadAllBytes(linkedImage1);
			Firebase.Storage.StorageReference linked1Ref = fbm.fbStorageRef.Child(ecc.code + "/" + "linkedImage1.jpg");
			linked1Ref.PutBytesAsync(linked1).ContinueWith ((Task<StorageMetadata> task) => {
			if (task.IsFaulted || task.IsCanceled) {
					//Debug.Log(task.Exception.ToString());
					// Uh-oh, an error occurred!
			} else {
					// Metadata contains file metadata such as size, content-type, and download URL.
					Firebase.Storage.StorageMetadata metadata = task.Result;
					//Debug.Log("Finished uploading linked image 1...");
    		}		
			});
		}
		if (File.Exists(linkedImage2))
		{
			linked2 = System.IO.File.ReadAllBytes(linkedImage2);
			Firebase.Storage.StorageReference linked2Ref = fbm.fbStorageRef.Child(ecc.code + "/" + "linkedImage2.jpg");
			linked2Ref.PutBytesAsync(linked2).ContinueWith ((Task<StorageMetadata> task) => {
			if (task.IsFaulted || task.IsCanceled) {
					//Debug.Log(task.Exception.ToString());
					// Uh-oh, an error occurred!
			} else {
					// Metadata contains file metadata such as size, content-type, and download URL.
					Firebase.Storage.StorageMetadata metadata = task.Result;
					//Debug.Log("Finished uploading linked image 2...");
    		}		
			});
		}
		/*
		if (File.Exists(linkedImage3))
		{
			linked3 = System.IO.File.ReadAllBytes(linkedImage3);
			Firebase.Storage.StorageReference linked3Ref = fbm.fbStorageRef.Child(ecc.code + "/" + "linkedImage3.jpg");
			linked3Ref.PutBytesAsync(linked3).ContinueWith ((Task<StorageMetadata> task) => {
			if (task.IsFaulted || task.IsCanceled) {
					//Debug.Log(task.Exception.ToString());
					// Uh-oh, an error occurred!
			} else {
					// Metadata contains file metadata such as size, content-type, and download URL.
					Firebase.Storage.StorageMetadata metadata = task.Result;
					//Debug.Log("Finished uploading linked image 3...");
    		}		
			});
		}
		if (File.Exists(linkedImage4))
		{
			linked4 = System.IO.File.ReadAllBytes(linkedImage4);
			Firebase.Storage.StorageReference linked4Ref = fbm.fbStorageRef.Child(ecc.code + "/" + "linkedImage4.jpg");
			linked4Ref.PutBytesAsync(linked4).ContinueWith ((Task<StorageMetadata> task) => {
			if (task.IsFaulted || task.IsCanceled) {
					//Debug.Log(task.Exception.ToString());
					// Uh-oh, an error occurred!
			} else {
					// Metadata contains file metadata such as size, content-type, and download URL.
					Firebase.Storage.StorageMetadata metadata = task.Result;
					//Debug.Log("Finished uploading linked image 4...");
    		}		
			});
		}
		if (File.Exists(linkedImage5))
		{
			linked5 = System.IO.File.ReadAllBytes(linkedImage5);
			Firebase.Storage.StorageReference linked5Ref = fbm.fbStorageRef.Child(ecc.code + "/" + "linkedImage5.jpg");
			linked5Ref.PutBytesAsync(linked5).ContinueWith ((Task<StorageMetadata> task) => {
			if (task.IsFaulted || task.IsCanceled) {
					//Debug.Log(task.Exception.ToString());
					// Uh-oh, an error occurred!
			} else {
					// Metadata contains file metadata such as size, content-type, and download URL.
					Firebase.Storage.StorageMetadata metadata = task.Result;
					//Debug.Log("Finished uploading linked image 5...");
    		}		
			});
		}
		*/
		lsh.GetComponent<UiManager>().SetLoadingPanelActive(false);

		itm.DeleteAllTargetsAndText();
		sm.deleteOldSave();
		
	}

	//starts downloading experience files (used for "open experience by code")
	//-called by Search button on Code Search screen
	//(startDownloadExperienceFiles->downloadExperienceFiles->lm.LoadFile)
	public void startDownloadExperienceFiles()
	{
		fm.arCamera.SetActive(true);
		StartCoroutine("downloadExperienceFiles");
	}

	private IEnumerator downloadExperienceFiles()
	{

		if (Directory.Exists(fm.SaveDirectory))
			Directory.Delete(fm.SaveDirectory, true);
		
		//Debug.Log("in downloadExperienceFiles");
		
		Directory.CreateDirectory(fm.SaveDirectory);

		//references to local paths
		string saveFilePath = Path.Combine(fm.SaveDirectory, "aoSave.json");
		string targetPath1 = Path.Combine(fm.SaveDirectory, "targetPhoto1.jpg");
		string targetPath2 = Path.Combine(fm.SaveDirectory, "targetPhoto2.jpg");
		//string targetPath3 = Path.Combine(fm.SaveDirectory, "targetPhoto3.jpg");
		//string targetPath4 = Path.Combine(fm.SaveDirectory, "targetPhoto4.jpg");
		//string targetPath5 = Path.Combine(fm.SaveDirectory, "targetPhoto5.jpg");
		string coverPath = Path.Combine(fm.SaveDirectory, "coverImage.jpg");
		string linkedPath1 = Path.Combine(fm.SaveDirectory, "linkedImage1.jpg");
		string linkedPath2 = Path.Combine(fm.SaveDirectory, "linkedImage2.jpg");
		//string linkedPath3 = Path.Combine(fm.SaveDirectory, "linkedImage3.jpg");
		//string linkedPath4 = Path.Combine(fm.SaveDirectory, "linkedImage4.jpg");
		//string linkedPath5 = Path.Combine(fm.SaveDirectory, "linkedImage5.jpg");

		string lowerCaseCode = ceam.UiCode.text.ToLower();
		//references to cloud filstore paths
		Firebase.Storage.StorageReference saveFileRef = fbm.fbStorage.GetReference(lowerCaseCode + "/" + "aoSave.json");
		Firebase.Storage.StorageReference targetRef1 = fbm.fbStorage.GetReference(lowerCaseCode + "/" + "targetPhoto1.jpg");
		Firebase.Storage.StorageReference targetRef2 = fbm.fbStorage.GetReference(lowerCaseCode + "/" + "targetPhoto2.jpg");
		//Firebase.Storage.StorageReference targetRef3 = fbm.fbStorage.GetReference(lowerCaseCode + "/" + "targetPhoto3.jpg");
		//Firebase.Storage.StorageReference targetRef4 = fbm.fbStorage.GetReference(lowerCaseCode + "/" + "targetPhoto4.jpg");
		//Firebase.Storage.StorageReference targetRef5 = fbm.fbStorage.GetReference(lowerCaseCode + "/" + "targetPhoto5.jpg");
		Firebase.Storage.StorageReference coverRef = fbm.fbStorage.GetReference(lowerCaseCode + "/" + "coverImage.jpg");
		Firebase.Storage.StorageReference linkedRef1 = fbm.fbStorage.GetReference(lowerCaseCode + "/" + "linkedImage1.jpg");
		Firebase.Storage.StorageReference linkedRef2 = fbm.fbStorage.GetReference(lowerCaseCode + "/" + "linkedImage2.jpg");
		//Firebase.Storage.StorageReference linkedRef3 = fbm.fbStorage.GetReference(lowerCaseCode + "/" + "linkedImage3.jpg");
		//Firebase.Storage.StorageReference linkedRef4 = fbm.fbStorage.GetReference(lowerCaseCode + "/" + "linkedImage4.jpg");
		//Firebase.Storage.StorageReference linkedRef5 = fbm.fbStorage.GetReference(lowerCaseCode + "/" + "linkedImage5.jpg");

		int maxAllowedSize = 2000*2000;

		targetRef1.GetBytesAsync(maxAllowedSize).ContinueWith((Task<byte[]> task1) => {
			if (task1.IsFaulted || task1.IsCanceled) {
				//Debug.Log(task1.Exception.ToString());
				// Uh-oh, an error occurred!
			} else {
				byte[] fileContents = task1.Result;
					//Debug.Log("target1 finished downloading!");
					File.WriteAllBytes(targetPath1, fileContents);

			}
});

targetRef2.GetBytesAsync(maxAllowedSize).ContinueWith((Task<byte[]> task1) => {
			if (task1.IsFaulted || task1.IsCanceled) {
				//Debug.Log(task1.Exception.ToString());
				// Uh-oh, an error occurred!
			} else {
				byte[] fileContents = task1.Result;
					//Debug.Log("target2 finished downloading!");
					File.WriteAllBytes(targetPath2, fileContents);

			}
});
/* 
targetRef3.GetBytesAsync(maxAllowedSize).ContinueWith((Task<byte[]> task1) => {
			if (task1.IsFaulted || task1.IsCanceled) {
				//Debug.Log(task1.Exception.ToString());
				// Uh-oh, an error occurred!
			} else {
				byte[] fileContents = task1.Result;
					//Debug.Log("target3 finished downloading!");
					File.WriteAllBytes(targetPath3, fileContents);

			}
});

targetRef4.GetBytesAsync(maxAllowedSize).ContinueWith((Task<byte[]> task1) => {
			if (task1.IsFaulted || task1.IsCanceled) {
				//Debug.Log(task1.Exception.ToString());
				// Uh-oh, an error occurred!
			} else {
				byte[] fileContents = task1.Result;
					//Debug.Log("target4 finished downloading!");
					File.WriteAllBytes(targetPath4, fileContents);

			}
});

targetRef5.GetBytesAsync(maxAllowedSize).ContinueWith((Task<byte[]> task1) => {
			if (task1.IsFaulted || task1.IsCanceled) {
				//Debug.Log(task1.Exception.ToString());
				// Uh-oh, an error occurred!
			} else {
				byte[] fileContents = task1.Result;
					//Debug.Log("target5 finished downloading!");
					File.WriteAllBytes(targetPath5, fileContents);

			}
});
*/
coverRef.GetBytesAsync(maxAllowedSize).ContinueWith((Task<byte[]> task1) => {
			if (task1.IsFaulted || task1.IsCanceled) {
				//Debug.Log(task1.Exception.ToString());
				// Uh-oh, an error occurred!
			} else {
				byte[] fileContents = task1.Result;
					//Debug.Log("cover image finished downloading!");
					File.WriteAllBytes(coverPath, fileContents);

			}
});

linkedRef1.GetBytesAsync(maxAllowedSize).ContinueWith((Task<byte[]> task1) => {
			if (task1.IsFaulted || task1.IsCanceled) {
				//Debug.Log(task1.Exception.ToString());
				// Uh-oh, an error occurred!
			} else {
				byte[] fileContents = task1.Result;
					//Debug.Log("linked image 1 finished downloading!");
					File.WriteAllBytes(linkedPath1, fileContents);

			}
});

linkedRef2.GetBytesAsync(maxAllowedSize).ContinueWith((Task<byte[]> task1) => {
			if (task1.IsFaulted || task1.IsCanceled) {
				//Debug.Log(task1.Exception.ToString());
				// Uh-oh, an error occurred!
			} else {
				byte[] fileContents = task1.Result;
					//Debug.Log("linked image 2 finished downloading!");
					File.WriteAllBytes(linkedPath2, fileContents);

			}
});

/*
linkedRef3.GetBytesAsync(maxAllowedSize).ContinueWith((Task<byte[]> task1) => {
			if (task1.IsFaulted || task1.IsCanceled) {
				//Debug.Log(task1.Exception.ToString());
				// Uh-oh, an error occurred!
			} else {
				byte[] fileContents = task1.Result;
					//Debug.Log("linked image 3 finished downloading!");
					File.WriteAllBytes(linkedPath3, fileContents);

			}
});

linkedRef4.GetBytesAsync(maxAllowedSize).ContinueWith((Task<byte[]> task1) => {
			if (task1.IsFaulted || task1.IsCanceled) {
				//Debug.Log(task1.Exception.ToString());
				// Uh-oh, an error occurred!
			} else {
				byte[] fileContents = task1.Result;
					//Debug.Log("linked image 4 finished downloading!");
					File.WriteAllBytes(linkedPath4, fileContents);

			}
});

linkedRef5.GetBytesAsync(maxAllowedSize).ContinueWith((Task<byte[]> task1) => {
			if (task1.IsFaulted || task1.IsCanceled) {
				//Debug.Log(task1.Exception.ToString());
				// Uh-oh, an error occurred!
			} else {
				byte[] fileContents = task1.Result;
					//Debug.Log("linked image 5 finished downloading!");
					File.WriteAllBytes(linkedPath5, fileContents);

			}
});
*/

yield return new WaitForSeconds(5);

saveFileRef.GetBytesAsync(maxAllowedSize).ContinueWith((Task<byte[]> task1) => {
			if (task1.IsFaulted || task1.IsCanceled) {
				//Debug.Log(task1.Exception.ToString());
				wrongCodeNotification.SetActive(true);
				loadPanel.SetActive(false);
				// Uh-oh, an error occurred!
			} else {
				byte[] fileContents = task1.Result;
				string saveContent = System.Text.Encoding.UTF8.GetString(fileContents, 0, fileContents.Length);
				//Debug.Log("Save File finished downloading!");
				File.WriteAllText(saveFilePath, saveContent);
				lm.LoadFile();
			}
});
	itm.gameObject.GetComponent<UiManager>().SetLoadingPanelActive(false);
	}



	//starts downloading experience files (used when user eecides to edit experience)
	//-called by clicking Edit Journey button on journey more info popup menu
	//(startDownloadExperienceFilesForEdit->downloadExperienceFilesForEdit->lm.LoadFile)
	public void startDownloadExperienceFilesForEdit(string codeToEdit)
	{
		editCode = codeToEdit;
		fm.arCamera.SetActive(true);
		StartCoroutine("downloadExperienceFilesForEdit");
	}

	private IEnumerator downloadExperienceFilesForEdit()
	{

		if (Directory.Exists(fm.SaveDirectory))
			Directory.Delete(fm.SaveDirectory, true);
		
		//Debug.Log("in downloadExperienceFiles");
		
		Directory.CreateDirectory(fm.SaveDirectory);

		//references to local paths
		string saveFilePath = Path.Combine(fm.SaveDirectory, "aoSave.json");
		string targetPath1 = Path.Combine(fm.SaveDirectory, "targetPhoto1.jpg");
		string targetPath2 = Path.Combine(fm.SaveDirectory, "targetPhoto2.jpg");
		//string targetPath3 = Path.Combine(fm.SaveDirectory, "targetPhoto3.jpg");
		//string targetPath4 = Path.Combine(fm.SaveDirectory, "targetPhoto4.jpg");
		//string targetPath5 = Path.Combine(fm.SaveDirectory, "targetPhoto5.jpg");
		string coverPath = Path.Combine(fm.SaveDirectory, "coverImage.jpg");
		string linkedPath1 = Path.Combine(fm.SaveDirectory, "linkedImage1.jpg");
		string linkedPath2 = Path.Combine(fm.SaveDirectory, "linkedImage2.jpg");
		//string linkedPath3 = Path.Combine(fm.SaveDirectory, "linkedImage3.jpg");
		//string linkedPath4 = Path.Combine(fm.SaveDirectory, "linkedImage4.jpg");
		//string linkedPath5 = Path.Combine(fm.SaveDirectory, "linkedImage5.jpg");

		//references to cloud filstore paths
		Firebase.Storage.StorageReference saveFileRef = fbm.fbStorage.GetReference(editCode + "/" + "aoSave.json");
		Firebase.Storage.StorageReference targetRef1 = fbm.fbStorage.GetReference(editCode + "/" + "targetPhoto1.jpg");
		Firebase.Storage.StorageReference targetRef2 = fbm.fbStorage.GetReference(editCode + "/" + "targetPhoto2.jpg");
		//Firebase.Storage.StorageReference targetRef3 = fbm.fbStorage.GetReference(editCode + "/" + "targetPhoto3.jpg");
		//Firebase.Storage.StorageReference targetRef4 = fbm.fbStorage.GetReference(editCode + "/" + "targetPhoto4.jpg");
		//Firebase.Storage.StorageReference targetRef5 = fbm.fbStorage.GetReference(editCode + "/" + "targetPhoto5.jpg");
		Firebase.Storage.StorageReference coverRef = fbm.fbStorage.GetReference(editCode + "/" + "coverImage.jpg");
		Firebase.Storage.StorageReference linkedRef1 = fbm.fbStorage.GetReference(editCode + "/" + "linkedImage1.jpg");
		Firebase.Storage.StorageReference linkedRef2 = fbm.fbStorage.GetReference(editCode + "/" + "linkedImage2.jpg");
		//Firebase.Storage.StorageReference linkedRef3 = fbm.fbStorage.GetReference(editCode + "/" + "linkedImage3.jpg");
		//Firebase.Storage.StorageReference linkedRef4 = fbm.fbStorage.GetReference(editCode + "/" + "linkedImage4.jpg");
		//Firebase.Storage.StorageReference linkedRef5 = fbm.fbStorage.GetReference(editCode + "/" + "linkedImage5.jpg");

		//Debug.Log(editCode);
		//codeDisplay.text = editCode;

		int maxAllowedSize = 2000*2000;

		targetRef1.GetBytesAsync(maxAllowedSize).ContinueWith((Task<byte[]> task1) => {
			if (task1.IsFaulted || task1.IsCanceled) {
				//Debug.Log(task1.Exception.ToString());
				// Uh-oh, an error occurred!
			} else {
				byte[] fileContents = task1.Result;
					//Debug.Log("target1 finished downloading!");
					File.WriteAllBytes(targetPath1, fileContents);

			}
});

targetRef2.GetBytesAsync(maxAllowedSize).ContinueWith((Task<byte[]> task1) => {
			if (task1.IsFaulted || task1.IsCanceled) {
				//Debug.Log(task1.Exception.ToString());
				// Uh-oh, an error occurred!
			} else {
				byte[] fileContents = task1.Result;
					//Debug.Log("target2 finished downloading!");
					File.WriteAllBytes(targetPath2, fileContents);

			}
});
/*
targetRef3.GetBytesAsync(maxAllowedSize).ContinueWith((Task<byte[]> task1) => {
			if (task1.IsFaulted || task1.IsCanceled) {
				//Debug.Log(task1.Exception.ToString());
				// Uh-oh, an error occurred!
			} else {
				byte[] fileContents = task1.Result;
					//Debug.Log("target3 finished downloading!");
					File.WriteAllBytes(targetPath3, fileContents);

			}
});

targetRef4.GetBytesAsync(maxAllowedSize).ContinueWith((Task<byte[]> task1) => {
			if (task1.IsFaulted || task1.IsCanceled) {
				//Debug.Log(task1.Exception.ToString());
				// Uh-oh, an error occurred!
			} else {
				byte[] fileContents = task1.Result;
					//Debug.Log("target4 finished downloading!");
					File.WriteAllBytes(targetPath4, fileContents);

			}
});

targetRef5.GetBytesAsync(maxAllowedSize).ContinueWith((Task<byte[]> task1) => {
			if (task1.IsFaulted || task1.IsCanceled) {
				//Debug.Log(task1.Exception.ToString());
				// Uh-oh, an error occurred!
			} else {
				byte[] fileContents = task1.Result;
					//Debug.Log("target5 finished downloading!");
					File.WriteAllBytes(targetPath5, fileContents);

			}
});
*/
coverRef.GetBytesAsync(maxAllowedSize).ContinueWith((Task<byte[]> task1) => {
			if (task1.IsFaulted || task1.IsCanceled) {
				//Debug.Log(task1.Exception.ToString());
				// Uh-oh, an error occurred!
			} else {
				byte[] fileContents = task1.Result;
					//Debug.Log("cover image finished downloading!");
					File.WriteAllBytes(coverPath, fileContents);

			}
});

linkedRef1.GetBytesAsync(maxAllowedSize).ContinueWith((Task<byte[]> task1) => {
			if (task1.IsFaulted || task1.IsCanceled) {
				//Debug.Log(task1.Exception.ToString());
				// Uh-oh, an error occurred!
			} else {
				byte[] fileContents = task1.Result;
					//Debug.Log("linked image 1 finished downloading!");
					File.WriteAllBytes(linkedPath1, fileContents);

			}
});

linkedRef2.GetBytesAsync(maxAllowedSize).ContinueWith((Task<byte[]> task1) => {
			if (task1.IsFaulted || task1.IsCanceled) {
				//Debug.Log(task1.Exception.ToString());
				// Uh-oh, an error occurred!
			} else {
				byte[] fileContents = task1.Result;
					//Debug.Log("linked image 2 finished downloading!");
					File.WriteAllBytes(linkedPath2, fileContents);

			}
});
/*
linkedRef3.GetBytesAsync(maxAllowedSize).ContinueWith((Task<byte[]> task1) => {
			if (task1.IsFaulted || task1.IsCanceled) {
				//Debug.Log(task1.Exception.ToString());
				// Uh-oh, an error occurred!
			} else {
				byte[] fileContents = task1.Result;
					//Debug.Log("linked image 3 finished downloading!");
					File.WriteAllBytes(linkedPath3, fileContents);

			}
});

linkedRef4.GetBytesAsync(maxAllowedSize).ContinueWith((Task<byte[]> task1) => {
			if (task1.IsFaulted || task1.IsCanceled) {
				//Debug.Log(task1.Exception.ToString());
				// Uh-oh, an error occurred!
			} else {
				byte[] fileContents = task1.Result;
					//Debug.Log("linked image 4 finished downloading!");
					File.WriteAllBytes(linkedPath4, fileContents);

			}
});

linkedRef5.GetBytesAsync(maxAllowedSize).ContinueWith((Task<byte[]> task1) => {
			if (task1.IsFaulted || task1.IsCanceled) {
				//Debug.Log(task1.Exception.ToString());
				// Uh-oh, an error occurred!
			} else {
				byte[] fileContents = task1.Result;
					//Debug.Log("linked image 5 finished downloading!");
					File.WriteAllBytes(linkedPath5, fileContents);

			}
});
*/
yield return new WaitForSeconds(5);

saveFileRef.GetBytesAsync(maxAllowedSize).ContinueWith((Task<byte[]> task1) => {
			if (task1.IsFaulted || task1.IsCanceled) {
				//Debug.Log(task1.Exception.ToString());
				// Uh-oh, an error occurred!
			} else {
				byte[] fileContents = task1.Result;
				string saveContent = System.Text.Encoding.UTF8.GetString(fileContents, 0, fileContents.Length);
				//Debug.Log("Save File finished downloading!");
				File.WriteAllText(saveFilePath, saveContent);
				lm.LoadFile();
			}
});
	itm.gameObject.GetComponent<UiManager>().SetLoadingPanelActive(false);
	}

	
	//starts downloading experience files (used when user opens an owned experience from library)
	//-called by clicking Edit Journey button on journey more info popup menu
	//(startDownloadExperienceFilesDirect->downloadExperienceFilesDirect->lm.LoadFile)
	public void startDownloadExperienceFilesDirect(int index)
	{
		whichIndex = index;
		fm.arCamera.SetActive(true);
		//Debug.Log("this is the library index:"+whichIndex);
		StartCoroutine("downloadExperienceFilesDirect");
	}

	private IEnumerator downloadExperienceFilesDirect()
	{
		//for debugging iOS download problem
		//Debug.Log("1. fsm484, Starting downloadExperienceFilesDirect()");

		if (Directory.Exists(fm.SaveDirectory))
			Directory.Delete(fm.SaveDirectory, true);
		
		Directory.CreateDirectory(fm.SaveDirectory);

		//references to local paths
		string saveFilePath = Path.Combine(fm.SaveDirectory, "aoSave.json");
		string targetPath1 = Path.Combine(fm.SaveDirectory, "targetPhoto1.jpg");
		string targetPath2 = Path.Combine(fm.SaveDirectory, "targetPhoto2.jpg");
		//string targetPath3 = Path.Combine(fm.SaveDirectory, "targetPhoto3.jpg");
		//string targetPath4 = Path.Combine(fm.SaveDirectory, "targetPhoto4.jpg");
		//string targetPath5 = Path.Combine(fm.SaveDirectory, "targetPhoto5.jpg");
		string coverPath = Path.Combine(fm.SaveDirectory, "coverImage.jpg");
		string linkedPath1 = Path.Combine(fm.SaveDirectory, "linkedImage1.jpg");
		string linkedPath2 = Path.Combine(fm.SaveDirectory, "linkedImage2.jpg");
		//string linkedPath3 = Path.Combine(fm.SaveDirectory, "linkedImage3.jpg");
		//string linkedPath4 = Path.Combine(fm.SaveDirectory, "linkedImage4.jpg");
		//string linkedPath5 = Path.Combine(fm.SaveDirectory, "linkedImage5.jpg");

		//for debugging iOS download problem
		//Debug.Log("2a. fsm500, Save file path = "+saveFilePath);
		//for debugging iOS download problem
		//Debug.Log("2b. fsm502, Target 1 path = "+targetPath1);
		//for debugging iOS download problem
		//Debug.Log("2c. fsm504, Target 2 path = "+targetPath2);
		//for debugging iOS download problem
		//Debug.Log("2d. fsm506, Target 3 path = "+targetPath3);
		//for debugging iOS download problem
		//Debug.Log("2e. fsm508, Target 4 path = "+targetPath4);
		//for debugging iOS download problem
		//Debug.Log("2f. fsm510, Target 5 path = "+targetPath5);

		expCode = ceam.libraryCodes[whichIndex-1];
		

		//for debugging iOS download problem
		//Debug.Log("3. fsm668, Code from library = "+expCode);


		//references to cloud filstore paths
		Firebase.Storage.StorageReference saveFileRef = fbm.fbStorage.GetReference(expCode + "/" + "aoSave.json");
		Firebase.Storage.StorageReference targetRef1 = fbm.fbStorage.GetReference(expCode + "/" + "targetPhoto1.jpg");
		Firebase.Storage.StorageReference targetRef2 = fbm.fbStorage.GetReference(expCode + "/" + "targetPhoto2.jpg");
		//Firebase.Storage.StorageReference targetRef3 = fbm.fbStorage.GetReference(expCode + "/" + "targetPhoto3.jpg");
		//Firebase.Storage.StorageReference targetRef4 = fbm.fbStorage.GetReference(expCode + "/" + "targetPhoto4.jpg");
		//Firebase.Storage.StorageReference targetRef5 = fbm.fbStorage.GetReference(expCode + "/" + "targetPhoto5.jpg");
		Firebase.Storage.StorageReference coverRef = fbm.fbStorage.GetReference(expCode + "/" + "coverImage.jpg");
		Firebase.Storage.StorageReference linkedRef1 = fbm.fbStorage.GetReference(expCode + "/" + "linkedImage1.jpg");
		Firebase.Storage.StorageReference linkedRef2 = fbm.fbStorage.GetReference(expCode + "/" + "linkedImage2.jpg");
		//Firebase.Storage.StorageReference linkedRef3 = fbm.fbStorage.GetReference(expCode + "/" + "linkedImage3.jpg");
		//Firebase.Storage.StorageReference linkedRef4 = fbm.fbStorage.GetReference(expCode + "/" + "linkedImage4.jpg");
		//Firebase.Storage.StorageReference linkedRef5 = fbm.fbStorage.GetReference(expCode + "/" + "linkedImage5.jpg");

		//for debugging iOS download problem
		//Debug.Log("4a. fsm682, Save file download path = "+expCode + "/" + "aoSave.json");
		//for debugging iOS download problem
		//Debug.Log("4b. fsm684, Target 1 download path = "+expCode + "/" + "targetPhoto1.jpg");
		//for debugging iOS download problem
		//Debug.Log("4c. fsm686, Target 2 download path = "+expCode + "/" + "targetPhoto2.jpg");
		//for debugging iOS download problem
		//Debug.Log("4d. fsm688, Target 3 download path = "+expCode + "/" + "targetPhoto3.jpg");
		//for debugging iOS download problem
		//Debug.Log("4e. fsm690, Target 4 download path = "+expCode + "/" + "targetPhoto4.jpg");
		//for debugging iOS download problem
		//Debug.Log("4f. fsm692, Target 5 download path = "+expCode + "/" + "targetPhoto5.jpg");

		int maxAllowedSize = 2000*2000;


targetRef1.GetBytesAsync(maxAllowedSize).ContinueWith((Task<byte[]> task1) => {
			if (task1.IsFaulted || task1.IsCanceled) {
				//Debug.Log(task1.Exception.ToString());
				// Uh-oh, an error occurred!
			} else {
				byte[] fileContents = task1.Result;
					//Debug.Log("target1 finished downloading!");
					File.WriteAllBytes(targetPath1, fileContents);

			}
});

targetRef2.GetBytesAsync(maxAllowedSize).ContinueWith((Task<byte[]> task1) => {
			if (task1.IsFaulted || task1.IsCanceled) {
				//Debug.Log(task1.Exception.ToString());
				// Uh-oh, an error occurred!
			} else {
				byte[] fileContents = task1.Result;
					//Debug.Log("target2 finished downloading!");
					File.WriteAllBytes(targetPath2, fileContents);

			}
});
/* 
targetRef3.GetBytesAsync(maxAllowedSize).ContinueWith((Task<byte[]> task1) => {
			if (task1.IsFaulted || task1.IsCanceled) {
				//Debug.Log(task1.Exception.ToString());
				// Uh-oh, an error occurred!
			} else {
				byte[] fileContents = task1.Result;
					//Debug.Log("target3 finished downloading!");
					File.WriteAllBytes(targetPath3, fileContents);

			}
});

targetRef4.GetBytesAsync(maxAllowedSize).ContinueWith((Task<byte[]> task1) => {
			if (task1.IsFaulted || task1.IsCanceled) {
				//Debug.Log(task1.Exception.ToString());
				// Uh-oh, an error occurred!
			} else {
				byte[] fileContents = task1.Result;
					//Debug.Log("target4 finished downloading!");
					File.WriteAllBytes(targetPath4, fileContents);

			}
});

targetRef5.GetBytesAsync(maxAllowedSize).ContinueWith((Task<byte[]> task1) => {
			if (task1.IsFaulted || task1.IsCanceled) {
				//Debug.Log(task1.Exception.ToString());
				// Uh-oh, an error occurred!
			} else {
				byte[] fileContents = task1.Result;
					//Debug.Log("target5 finished downloading!");
					File.WriteAllBytes(targetPath5, fileContents);

			}
});
*/

coverRef.GetBytesAsync(maxAllowedSize).ContinueWith((Task<byte[]> task1) => {
			if (task1.IsFaulted || task1.IsCanceled) {
				//Debug.Log(task1.Exception.ToString());
				// Uh-oh, an error occurred!
			} else {
				byte[] fileContents = task1.Result;
					//Debug.Log("cover image finished downloading!");
					File.WriteAllBytes(coverPath, fileContents);

			}
});

linkedRef1.GetBytesAsync(maxAllowedSize).ContinueWith((Task<byte[]> task1) => {
			if (task1.IsFaulted || task1.IsCanceled) {
				//Debug.Log(task1.Exception.ToString());
				// Uh-oh, an error occurred!
			} else {
				byte[] fileContents = task1.Result;
					//Debug.Log("linked image 1 finished downloading!");
					File.WriteAllBytes(linkedPath1, fileContents);

			}
});

linkedRef2.GetBytesAsync(maxAllowedSize).ContinueWith((Task<byte[]> task1) => {
			if (task1.IsFaulted || task1.IsCanceled) {
				//Debug.Log(task1.Exception.ToString());
				// Uh-oh, an error occurred!
			} else {
				byte[] fileContents = task1.Result;
					//Debug.Log("linked image 2 finished downloading!");
					File.WriteAllBytes(linkedPath2, fileContents);

			}
});
/* 
linkedRef3.GetBytesAsync(maxAllowedSize).ContinueWith((Task<byte[]> task1) => {
			if (task1.IsFaulted || task1.IsCanceled) {
				//Debug.Log(task1.Exception.ToString());
				// Uh-oh, an error occurred!
			} else {
				byte[] fileContents = task1.Result;
					//Debug.Log("linked image 3 finished downloading!");
					File.WriteAllBytes(linkedPath3, fileContents);

			}
});


linkedRef4.GetBytesAsync(maxAllowedSize).ContinueWith((Task<byte[]> task1) => {
			if (task1.IsFaulted || task1.IsCanceled) {
				//Debug.Log(task1.Exception.ToString());
				// Uh-oh, an error occurred!
			} else {
				byte[] fileContents = task1.Result;
					//Debug.Log("linked image 4 finished downloading!");
					File.WriteAllBytes(linkedPath4, fileContents);

			}
});

linkedRef5.GetBytesAsync(maxAllowedSize).ContinueWith((Task<byte[]> task1) => {
			if (task1.IsFaulted || task1.IsCanceled) {
				//Debug.Log(task1.Exception.ToString());
				// Uh-oh, an error occurred!
			} else {
				byte[] fileContents = task1.Result;
					//Debug.Log("linked image 5 finished downloading!");
					File.WriteAllBytes(linkedPath5, fileContents);

			}
});
*/
yield return new WaitForSeconds(5);

saveFileRef.GetBytesAsync(maxAllowedSize).ContinueWith((Task<byte[]> task1) => {
			if (task1.IsFaulted || task1.IsCanceled) {
				//Debug.Log(task1.Exception.ToString());
				// Uh-oh, an error occurred!
			} else {
				byte[] fileContents = task1.Result;
				string saveContent = System.Text.Encoding.UTF8.GetString(fileContents, 0, fileContents.Length);
				//Debug.Log("Save File finished downloading!");
				File.WriteAllText(saveFilePath, saveContent);
				//Debug.Log("5. fsm1061 Does save file exist at correct path? "+ File.Exists(saveFilePath));
				//Debug.Log("5.a. fsm1062 Contents of save file: "+File.ReadAllText(saveFilePath));
				lm.LoadFile();

			}
});
		whichIndex = 0;
		itm.gameObject.GetComponent<UiManager>().SetLoadingPanelActive(false);
	}

	//deletes experience from UI, Google Cloud Datastore, and Firebase Storage
	public void DeleteExperience(string code)
	{
		Firebase.Storage.StorageReference saveRef = fbm.fbStorageRef.Child(code + "/" + "aoSave.json");
		Firebase.Storage.StorageReference photoRef1 = fbm.fbStorageRef.Child(code + "/" + "targetPhoto1.jpg");
		Firebase.Storage.StorageReference photoRef2 = fbm.fbStorageRef.Child(code + "/" + "targetPhoto2.jpg");
		Firebase.Storage.StorageReference linkedRef1 = fbm.fbStorageRef.Child(code + "/" + "linkedImage1.jpg");
		Firebase.Storage.StorageReference linkedRef2 = fbm.fbStorageRef.Child(code + "/" + "linkedImage2.jpg");
		Firebase.Storage.StorageReference coverRef = fbm.fbStorageRef.Child(code + "/" + "coverImage.jpg");
		//Firebase.Storage.StorageReference photoRef3 = fbm.fbStorageRef.Child(code + "/" + "targetPhoto3.jpg");
		//Firebase.Storage.StorageReference photoRef4 = fbm.fbStorageRef.Child(code + "/" + "targetPhoto4.jpg");
		//Firebase.Storage.StorageReference photoRef5 = fbm.fbStorageRef.Child(code + "/" + "targetPhoto5.jpg");


		// Delete the file
		saveRef.DeleteAsync().ContinueWith(task => {
    if (task.IsCompleted) {
        //Debug.Log("Edit Flow: Save file deleted successfully.");
    } else {
        // Uh-oh, an error occurred!
    }
		});	
		// Delete the file
		photoRef1.DeleteAsync().ContinueWith(task => {
    if (task.IsCompleted) {
        //Debug.Log("Edit Flow: Target 1 image File deleted successfully.");
    } else {
        // Uh-oh, an error occurred!
    }
		});	
		// Delete the file
		photoRef2.DeleteAsync().ContinueWith(task => {
    if (task.IsCompleted) {
        //Debug.Log("Edit Flow: Target 2 image File deleted successfully.");
    } else {
        // Uh-oh, an error occurred!
    }
		});	
		linkedRef1.DeleteAsync().ContinueWith(task => {
    if (task.IsCompleted) {
        //Debug.Log("Edit Flow: linked 1 image File deleted successfully.");
    } else {
        // Uh-oh, an error occurred!
    }
		});	
		linkedRef2.DeleteAsync().ContinueWith(task => {
    if (task.IsCompleted) {
        //Debug.Log("Edit Flow: linked 2 image File deleted successfully.");
    } else {
        // Uh-oh, an error occurred!
    }
		});	
		coverRef.DeleteAsync().ContinueWith(task => {
    if (task.IsCompleted) {
        //Debug.Log("Edit Flow: cover image File deleted successfully.");
    } else {
        // Uh-oh, an error occurred!
    }
		});	
		/* 
		// Delete the file
		photoRef3.DeleteAsync().ContinueWith(task => {
    if (task.IsCompleted) {
        //Debug.Log("File deleted successfully.");
    } else {
        // Uh-oh, an error occurred!
    }
		});	
		
		// Delete the file
		photoRef4.DeleteAsync().ContinueWith(task => {
    if (task.IsCompleted) {
        //Debug.Log("File deleted successfully.");
    } else {
        // Uh-oh, an error occurred!
    }
		});	
		// Delete the file
		photoRef5.DeleteAsync().ContinueWith(task => {
    if (task.IsCompleted) {
        //Debug.Log("File deleted successfully.");
    } else {
        // Uh-oh, an error occurred!
    }
		});	
		 */
	}
}
