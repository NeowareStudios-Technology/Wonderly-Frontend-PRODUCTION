/******************************************************
*Project: Wonderly
*Created by: David Lee Ramirez
*Date: 12/28/18
*Description: handles nearly all calls to Google Cloud Endpoints Backend
							and initializes/destroys library stubs
*Copyright 2018 LeapWithAlice,LLC. All rights reserved
 ******************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using UnityEngine.Networking;
using System.IO;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;


public class CloudEndpointsApiManager : MonoBehaviour {

	//script references
	public GameObject lsh;
	public FirebaseManager fbm;
	public FirebaseStorageManager fsm;
	public SaveManager sm;
	public FilesManager fm;
	public ErrorMessageFlowManager emfm;
	public OwnedExperiencesClass oec;
	public ProfileInfoClass pic;
	public checkEmailClass cec;
	public checkEmailResponseClass cerc;
	public CreateOrEditController coec;
	public UiManager um;
	//UI elements for getting values to send to backend via JSON web calls
	public Text editFirstName;
	public Text editLastName;
	public InputField editFirstNameInput;
	public InputField editLastNameInput;
	public Text displayFirstNameHome;
	public Text displayFirstName;
	public Text displayLastName;
	public Text displayName;
	public Text displayEmail;
	public Text displayExpNum;
	public Text firstNamePlaceHolder;
	public Text lastNamePlaceHolder;
	public Text emailPlaceHolder;
	public Text password;
	public Text email;
	public Text UiCode;
	public Text displayCode;
	//for switching screens
	public PanelController mainCanvasPanelController;
	public Animator journeySummaryAnimator;
	public Animator ViewScreenAnimator;
	public Animator PreviewScreenAnimator;

	public GameObject loadingPanel;
	public GameObject libraryPanel;
	public GameObject inFrontOfBottomPanel;
	public GameObject frontCanvas;

	public string[] libraryCodes = new string[50];
	public GameObject[] libraryStubs = new GameObject[50];
	public GameObject libraryStubPrefab;
	public GameObject libraryScrollContent;
	public GameObject libraryPopupMenuPrefab;
	public GameObject libraryPopupMenuInstantiated;
	public GameObject iconPanel;
	public GameObject editPasswordLengthError;
	public GameObject editPasswordMatcherror;
	public GameObject editPasswordValidError;
	public GameObject deleteJourneyPrefab;
	public string code;
	public string deleteCode;
	public string editCode;

	//urls for backend calls
	private string getProfileUrl = "https://wonderly-225214.appspot.com/_ah/api/wonderly/v1/profile";
	private string createProfileUrl = "https://wonderly-225214.appspot.com/_ah/api/wonderly/v1/profile";
	private string getOwnedCodesUrl = "https://wonderly-225214.appspot.com/_ah/api/wonderly/v1/profile/codes";
	private string deleteExpUrl = "https://wonderly-225214.appspot.com/_ah/api/wonderly/v1/exp/delete";
	private string editExpUrl = "https://wonderly-225214.appspot.com/_ah/api/wonderly/v1/exp/edit";
	private string emailCheckUrl = "https://wonderly-225214.appspot.com/_ah/api/wonderly/v1/profile/check";
	private string editProfileUrl = "https://wonderly-225214.appspot.com/_ah/api/wonderly/v1/profile/edit";

	public bool checkEmail;
	int maxAllowedSize = 2000*2000;
	public byte[] fileContents;
	public Image testImage;
  public GameObject noJourneyPanel;
	private int deleteJourneyIndex;
	

	//for counting number of web call retries (max = 3)
	private int profileCreateCallCount = 0;
	private int profileEditCallCount = 0;
	private int emailCheckCallCount = 0;
	private int deleteExpCallCount = 0;
	private int editExpCallCount = 0;
	private int profileInfoCallCount = 0;
	private int ownedCodesCallCount = 0;
	private int MAX_RETRY = 3;

	public void startProfileCreate()
	{
		StartCoroutine("profileCreate");
	}

	public IEnumerator profileCreate () 
	{
		ProfileClass newProfile = new ProfileClass();
		newProfile.firstName = PlayerPrefs.GetString("fName");
		newProfile.lastName = PlayerPrefs.GetString("lName");

		//convert profile clas instance into json string
		string newProfJson = JsonUtility.ToJson(newProfile);

		using (UnityWebRequest newProfileRequest = UnityWebRequest.Put(createProfileUrl,newProfJson))
		{
			//set content type
			newProfileRequest.SetRequestHeader("Content-Type", "application/json");
			//set auth header
			newProfileRequest.SetRequestHeader("Authorization", "Bearer " + fbm.token);

			yield return newProfileRequest.SendWebRequest();
			Debug.Log(newProfileRequest.responseCode);

			//retry call up to MAX_RETRY times if error
			if (newProfileRequest.responseCode != 200 && profileCreateCallCount < MAX_RETRY)
			{
				profileCreateCallCount++;
				StartCoroutine("profileCreate");
			}
			else if (newProfileRequest.responseCode != 200 && profileCreateCallCount >= MAX_RETRY)
			{
				loadingPanel.SetActive(false);
				Debug.Log("call to backend for profileCreate retries failed");
				profileCreateCallCount = 0;
			}
			else
			{
				loadingPanel.SetActive(false);
				profileCreateCallCount = 0;
				//load in profile info to ui (called here because need to wait for cookie and profile creation)
				StartCoroutine("getProfileInfo");
			}
		}

	}

	public void startProfileEdit()
	{
		lsh.GetComponent<UiManager>().SetLoadingPanelActive(true);
		Debug.Log("0");

		//if any inputs in password edit
		if (fbm.editPassword.text != "" || fbm.editPassword2.text != "" || fbm.editPasswordCurrent.text != "")
		{
				//if current password incorrect
				if (fbm.editPasswordCurrent.text != PlayerPrefs.GetString("password"))
				{
					Debug.Log("1");
					//activate need correct password error message
					lsh.GetComponent<UiManager>().SetLoadingPanelActive(false);
					editPasswordValidError.SetActive(true);
					return;
				}

				//if passwords dont match
				else if (fbm.editPassword2.text != fbm.editPassword.text)
				{
					Debug.Log("2");
					//activate need matching passwords error message
					lsh.GetComponent<UiManager>().SetLoadingPanelActive(false);
					editPasswordMatcherror.SetActive(true);
					return;
				}

				//if current password incorrect
				else if (fbm.editPassword2.text.Length < 6)
				{
					Debug.Log("MAX_RETRY");
					//activate need correct password length error message
					lsh.GetComponent<UiManager>().SetLoadingPanelActive(false);
					editPasswordLengthError.SetActive(true);
					return;
				}

				//if new passwords blank
				else if (fbm.editPassword.text =="" || fbm.editPassword2.text =="")
				{
					Debug.Log("4");
					//activate need correct password error message
					lsh.GetComponent<UiManager>().SetLoadingPanelActive(false);
					editPasswordLengthError.SetActive(true);
					return;
				}

				else
				{
						Debug.Log("5");
						Debug.Log("changing password and starting profile edit");
						fbm.changeUserPassword();
				}
		}

		StartCoroutine("profileEdit");
	}

	public IEnumerator profileEdit() 
	{
		ProfileClass editProfile = new ProfileClass();
		editProfile.firstName = editFirstName.text;
		editProfile.lastName = editLastName.text;

		Debug.Log(editFirstNameInput.text);
		editFirstNameInput.text = "";
		editLastNameInput.text = "";
		Debug.Log(editFirstNameInput.text);
		//convert profile clas instance into json string
		string editProfJson = JsonUtility.ToJson(editProfile);

			if (editProfile.firstName != "" || editProfile.lastName != "")
			{
				//retry call up to MAX_RETRY times (if succsessful gets set to MAX_RETRY)
				while (profileEditCallCount < MAX_RETRY)
				{
					using (UnityWebRequest editProfileRequest = UnityWebRequest.Put(editProfileUrl,editProfJson))
					{
						//set content type
						editProfileRequest.SetRequestHeader("Content-Type", "application/json");
						//set auth header
						editProfileRequest.SetRequestHeader("Authorization", "Bearer " + fbm.token);

						yield return editProfileRequest.SendWebRequest();
						Debug.Log(editProfileRequest.responseCode);

						//retry call up to MAX_RETRY times if error
						if (editProfileRequest.responseCode != 200 && profileEditCallCount < MAX_RETRY)
						{
							profileEditCallCount++;
						}
						else if (editProfileRequest.responseCode != 200 && profileEditCallCount >= MAX_RETRY)
						{
							profileEditCallCount = MAX_RETRY;
							loadingPanel.SetActive(false);
							Debug.Log("call to backend for profileEdit retries failed");
						}
						else
						{
							profileEditCallCount = MAX_RETRY;
							//call this to fill the profile page with the changed content (lets user know edit worked)
							//getProfileInfo will disable loading screen on completion
							StartCoroutine("getProfileInfo");
						}
					}
				}
			}

		profileEditCallCount = 0;
		um.clearUserSettingsInputFields();
	}


	//checks if proposed user email is already in use
	public void startCheckEmail()
	{
		StartCoroutine("emailCheck");
	}

	public IEnumerator emailCheck() 
	{
		checkEmailClass emailCheck = new checkEmailClass();
		emailCheck.email = email.text.ToLower();

		//convert profile clas instance into json string
		string emailCheckJson = JsonUtility.ToJson(emailCheck);

		using (UnityWebRequest emailCheckRequest = UnityWebRequest.Put(emailCheckUrl,emailCheckJson))
		{
			//set content type
			emailCheckRequest.SetRequestHeader("Content-Type", "application/json");

			yield return emailCheckRequest.SendWebRequest();

			Debug.Log(emailCheckRequest.responseCode);
			//retry call up to MAX_RETRY times if error
			if (emailCheckRequest.responseCode != 200 && emailCheckCallCount < MAX_RETRY)
			{
				emailCheckCallCount++;
				StartCoroutine("emailCheck");
			}
			else if (emailCheckRequest.responseCode != 200 && emailCheckCallCount >= MAX_RETRY)
			{
				loadingPanel.SetActive(false);
				Debug.Log("call to backend for emailCheck retries failed");
				emailCheckCallCount = 0;
			}
			else
			{
				emailCheckCallCount = 0;

				byte[] results = emailCheckRequest.downloadHandler.data;
				string jsonString = Encoding.UTF8.GetString(results);
				cerc = JsonUtility.FromJson<checkEmailResponseClass>(jsonString);
				Debug.Log(cerc.exists);
				if (cerc.exists == "n")
				{
					Debug.Log("The email is free to use");
					emfm.activateSignUpPanel2();
					emfm.signUpIndex++;
				}
				else if (cerc.exists == "y")
				{
					Debug.Log("The email is already in use");
					emfm.displayEmailError();
				}
			}
		}
	}

	//called by ui button to delete experience (journey)
	public void startExperienceDelete(int index)
	{
		lsh.GetComponent<UiManager>().SetLoadingPanelActive(true);
		//delete experience from google datastore noSQL database
		StartCoroutine(deleteExperienceFromDataStore(index));
	}

	public IEnumerator deleteExperienceFromDataStore(int index) 
	{
		ExperienceCodeClass expCode = new ExperienceCodeClass();
		deleteCode = libraryCodes[index];
		expCode.code = deleteCode;

		//convert profile clas instance into json string
		string delExpJson = JsonUtility.ToJson(expCode);

		using (UnityWebRequest deleteExperienceRequest = UnityWebRequest.Put(deleteExpUrl,delExpJson))
		{
			//set content type
			deleteExperienceRequest.SetRequestHeader("Content-Type", "application/json");
			//set auth header
			deleteExperienceRequest.SetRequestHeader("Authorization", "Bearer " + fbm.token);

			yield return deleteExperienceRequest.SendWebRequest();
			//retry call up to MAX_RETRY times if error
			if (deleteExperienceRequest.responseCode != 200 && deleteExpCallCount < MAX_RETRY)
			{
				deleteExpCallCount++;
				StartCoroutine(deleteExperienceFromDataStore(index));
			}
			else if (deleteExperienceRequest.responseCode != 200 && deleteExpCallCount >= MAX_RETRY)
			{
				lsh.GetComponent<UiManager>().SetLoadingPanelActive(false);
				Debug.Log("call to backend for deleteExperience retries failed");
				deleteExpCallCount = 0;
			}
			else
			{
				lsh.GetComponent<UiManager>().SetLoadingPanelActive(false);
				deleteExpCallCount = 0;
				byte[] results = deleteExperienceRequest.downloadHandler.data;
				string jsonString = Encoding.UTF8.GetString(results);
				Debug.Log(jsonString);

				//delete experience from Firebase storage file store
				fsm.DeleteExperience(deleteCode);

				//get rid of the library stub that was holding the journey info
				Destroy(libraryStubs[index]);
			}
		}
	}


	public void startExperienceEdit(int index)
	{
		lsh.GetComponent<UiManager>().SetLoadingPanelActive(true);
		editCode = libraryCodes[index-1];
		
		fsm.ecc.code = editCode;
		UiCode.text = editCode;
		code = editCode;
		fsm.startDownloadExperienceFilesForEdit(editCode);
	}

	public void startExperienceEdit2()
	{
		StartCoroutine("experienceEdit");
		StartCoroutine("deleteAndUploadExperienceFiles");
	}

	private IEnumerator deleteAndUploadExperienceFiles()
	{
		fsm.DeleteExperience(code);
		yield return new WaitForSeconds(2);
		fsm.uploadExperienceFiles();
	}

	private IEnumerator experienceEdit() 
	{
		editExperienceClass editExperience = new editExperienceClass();
		editExperience.title = sm.editTitle.text;
		editExperience.code = code;
		displayCode.text = code;
		Debug.Log("In experienceEdit: cover image url from sm is "+sm.save.coverImageUrl);
		editExperience.coverImage = sm.save.coverImageUrl;
		if (editExperience.coverImage == "" || editExperience.coverImage == null)
			editExperience.coverImage = "none";

		//get target object counts
		int modelCount = 0;
		int videoCount = 0;
		int imageCount = 0;
		for(int i= 0; i < 5; i++)
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
		}
		Debug.Log(modelCount);
		editExperience.model = modelCount;
		editExperience.video = videoCount;
		editExperience.image = imageCount;
		if (sm.description.text == "")
			editExperience.text = false;
		else
			editExperience.text = true;

		//convert editExperience clas instance into json string
		string editExperienceJson = JsonUtility.ToJson(editExperience);

		//retry call up to MAX_RETRY times if error
		while (editExpCallCount < MAX_RETRY)
		{
			using (UnityWebRequest editExperienceRequest = UnityWebRequest.Put(editExpUrl,editExperienceJson))
			{
				//set content type
				editExperienceRequest.SetRequestHeader("Content-Type", "application/json");
				//set auth header
				editExperienceRequest.SetRequestHeader("Authorization", "Bearer " + fbm.token);

				yield return editExperienceRequest.SendWebRequest();
				//retry call up to MAX_RETRY times if error
				if (editExperienceRequest.responseCode != 200 && editExpCallCount < MAX_RETRY)
				{
					Debug.Log(editExperienceRequest.responseCode);
					editExpCallCount++;
				}
				else if (editExperienceRequest.responseCode != 200 && editExpCallCount >= MAX_RETRY)
				{
					editExpCallCount = MAX_RETRY;
					Debug.Log("call to backend for editExp retries failed");
				}
				else
				{
					editExpCallCount = MAX_RETRY;
				}
				Debug.Log(editExperienceRequest.responseCode);
			}
		}
		editExpCallCount = 0;
	}


public void startGetProfileInfo()
	{
		StartCoroutine("getProfileInfo");
	}



	public IEnumerator getProfileInfo() 
	{
				um.clearUserSettingsInputFields();

        using (UnityWebRequest newProfileInfoRequest = UnityWebRequest.Get(getProfileUrl))
        {
            //set content type
            newProfileInfoRequest.SetRequestHeader("Content-Type", "application/json");
            //set auth header
            newProfileInfoRequest.SetRequestHeader("Authorization", "Bearer " + fbm.token);

            yield return newProfileInfoRequest.SendWebRequest();

						//retry call up to MAX_RETRY times if error
						if (newProfileInfoRequest.responseCode != 200 && profileInfoCallCount < MAX_RETRY)
						{
							profileInfoCallCount++;
							StartCoroutine("getProfileInfo");
						}
						else if (newProfileInfoRequest.responseCode != 200 && profileInfoCallCount >= MAX_RETRY)
						{
							loadingPanel.SetActive(false);
							Debug.Log("call to backend for getProfileInfo retries failed");
							profileInfoCallCount = 0;
						}
						else
						{
							loadingPanel.SetActive(false);
							profileInfoCallCount = 0;
					
							Debug.Log(newProfileInfoRequest.responseCode);
							byte[] results = newProfileInfoRequest.downloadHandler.data;
							string jsonString = Encoding.UTF8.GetString(results);
							Debug.Log(jsonString);
							pic = JsonUtility.FromJson<ProfileInfoClass>(jsonString);

							//for home screen
							displayFirstNameHome.text = "Hi," + " " + pic.firstName + "!";

							//for profile info screen
							string fullName = pic.firstName + " " + pic.lastName;
							displayName.text = fullName;
							//add "" to make the int into a string without c# complaining
							displayExpNum.text = pic.createdExp + "";

							//for profile edit screen
							firstNamePlaceHolder.text = pic.firstName;
							lastNamePlaceHolder.text = pic.lastName;
							emailPlaceHolder.text = pic.email;

							lsh.GetComponent<UiManager>().SetLoadingPanelActive(false);
						}
        }	
	}




	//called when user opens library to populate library stubs with saved info on server
	public void startGetOwnedCodes()
	{
		StartCoroutine(getOwnedCodes());
		Debug.Log("start get ownded codes called this mayn");
	}

	public IEnumerator getOwnedCodes() 
	{
		lsh.GetComponent<UiManager>().SetLoadingPanelActive(true);
		int numExperiences = 0;
		using (UnityWebRequest getOwnedCodesRequest = UnityWebRequest.Get(getOwnedCodesUrl))
		{
			//set content type
			getOwnedCodesRequest.SetRequestHeader("Content-Type", "application/json");
			//set auth header
			getOwnedCodesRequest.SetRequestHeader("Authorization", "Bearer " + fbm.token);

			yield return getOwnedCodesRequest.SendWebRequest();

			//retry call up to MAX_RETRY times if error
			if (getOwnedCodesRequest.responseCode != 200 && ownedCodesCallCount < MAX_RETRY)
			{
				ownedCodesCallCount++;
				StartCoroutine("getOwnedCodes");
			}
			else if (getOwnedCodesRequest.responseCode != 200 && ownedCodesCallCount >= MAX_RETRY)
			{
				loadingPanel.SetActive(false);
				Debug.Log("call to backend for profileCreate retries failed");
			}
			else
			{
				ownedCodesCallCount = 0;

			Debug.Log(getOwnedCodesRequest.responseCode);

			byte[] results = getOwnedCodesRequest.downloadHandler.data;
			string jsonString = Encoding.UTF8.GetString(results);
			Debug.Log(jsonString);
			oec = JsonUtility.FromJson<OwnedExperiencesClass>(jsonString);

			//count how many experiences the user has 
			foreach(string code in oec.codes)
			{
				if (code == null)
					break;

				numExperiences++;
			}

			if (numExperiences == 0 && getOwnedCodesRequest.responseCode == 200)
			{
					noJourneyPanel.SetActive(true);
			}
			else
			{
					noJourneyPanel.SetActive(false);
			}

			Debug.Log("Number of codes: "+numExperiences);

			foreach (GameObject x in libraryStubs){
			Destroy(x);
		}
		//this for loop creates the library stub GameObjects from a prefab (up to 50)
		//for dynamic spawning way
		for (int i = 0; i < numExperiences; i++)
		{
			Texture2D tex = new Texture2D(2000, 2000);
			//make sure there arent more than 50 experiences (journeys)
			if (i == 49)
				break;

			//get cover image from our firebase storage project
			if (oec.coverImages[i] != "none")
			{
				Firebase.Storage.StorageReference coverImageRef = fbm.fbStorage.GetReference(oec.codes[i] + "/" + "coverImage.jpg");

				coverImageRef.GetBytesAsync(maxAllowedSize).ContinueWith((Task<byte[]> task1) => {
			if (task1.IsFaulted || task1.IsCanceled) {
				Debug.Log(task1.Exception.ToString());
				// Uh-oh, an error occurred!
			} else {
				byte[] fileContents = task1.Result;
				Debug.Log("cover image finished downloading!");
				
				tex.LoadImage(fileContents);
				testImage.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0));
				
				//NEED TO ASSIGN IMAGE TO STUB HERE
			}
			});
			//if a cover image was selected by a user create a new sprite! 
			libraryStubs[i] = Instantiate(libraryStubPrefab,libraryScrollContent.transform);
			libraryStubs[i].transform.GetChild(5).GetComponent<Image>().sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0));
		}
		else{
			//if a cover image was NOT selected by a user do not create a new sprite! 
			libraryStubs[i] = Instantiate(libraryStubPrefab,libraryScrollContent.transform);
		}


			int index = i;

			//spawn and fill out library stub
			
			//title
			libraryStubs[i].transform.GetChild(1).gameObject.GetComponent<Text>().text = oec.titles[i];
			//code
			libraryStubs[i].transform.GetChild(2).gameObject.GetComponent<Text>().text = oec.codes[i];
			//JourneyCoverPhoto
			libraryStubs[i].transform.GetChild(5).gameObject.GetComponent<Button>().onClick.AddListener(delegate {fsm.startDownloadExperienceFilesDirect(index+1); });
			libraryStubs[i].transform.GetChild(5).gameObject.GetComponent<Button>().onClick.AddListener(delegate {mainCanvasPanelController.OpenPanel(PreviewScreenAnimator); });
			libraryStubs[i].transform.GetChild(5).gameObject.GetComponent<Button>().onClick.AddListener(delegate {iconPanel.SetActive(false); });
			libraryStubs[i].transform.GetChild(5).gameObject.GetComponent<Button>().onClick.AddListener(delegate {lsh.GetComponent<UiManager>().SetLoadingPanelActive(true); });
			//more info
			libraryStubs[i].transform.GetChild(7).gameObject.GetComponent<Button>().onClick.AddListener(delegate {createLibraryPopup(index); });
			//date
			libraryStubs[i].transform.GetChild(8).gameObject.GetComponent<Text>().text = oec.dates[i];
			libraryCodes[i] = oec.codes[i];

		//	StartCoroutine(loadJourneyCoverImage(libraryStubs[i], oec.coverImages[i]));

	}
	lsh.GetComponent<UiManager>().SetLoadingPanelActive(false);
			}
		}
	
	}
		

	//creates library menu UI
	void createLibraryPopup(int index)
	{
		frontCanvas.SetActive(true);
		libraryPopupMenuInstantiated = Instantiate(libraryPopupMenuPrefab, inFrontOfBottomPanel.transform);
		//delete button
		libraryPopupMenuInstantiated.transform.GetChild(2).GetChild(2).gameObject.GetComponent<Button>().onClick.AddListener(delegate {createDeletePopup(index); });
		//edit button
		libraryPopupMenuInstantiated.transform.GetChild(2).GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(delegate {um.InstantResetSummaryScreen(); });
		libraryPopupMenuInstantiated.transform.GetChild(2).GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(delegate {startExperienceEdit(index+1); });
		libraryPopupMenuInstantiated.transform.GetChild(2).GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(delegate {lsh.GetComponent<UiManager>().SetLoadingPanelActive(true); });
		libraryPopupMenuInstantiated.transform.GetChild(2).GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(delegate {mainCanvasPanelController.OpenPanel(journeySummaryAnimator); });
		libraryPopupMenuInstantiated.transform.GetChild(2).GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(delegate {iconPanel.SetActive(false); });
		libraryPopupMenuInstantiated.transform.GetChild(2).GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(delegate {coec.setCreateOrEdit("edit"); });
		//libraryPopupMenuInstantiated.transform.GetChild(2).GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(delegate {frontCanvas.SetActive(false); });
		libraryPopupMenuInstantiated.transform.GetChild(2).GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(delegate {Destroy(libraryPopupMenuInstantiated); });
		//cancel button
		libraryPopupMenuInstantiated.transform.GetChild(4).GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(delegate {frontCanvas.SetActive(false); });
		libraryPopupMenuInstantiated.transform.GetChild(4).GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(delegate {Destroy(libraryPopupMenuInstantiated); });
		//share
		libraryPopupMenuInstantiated.transform.GetChild(2).GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(delegate {lsh.GetComponent<Sharing>().SharingCodeJourneyContextMenu(index); });
		libraryPopupMenuInstantiated.transform.GetChild(2).GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(delegate {Destroy(libraryPopupMenuInstantiated); });
		libraryPopupMenuInstantiated.transform.GetChild(2).GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(delegate {frontCanvas.SetActive(false); });
	}


	//creates library menu UI
	void createDeletePopup(int index)
	{
		Debug.Log("in create delete popup");
		frontCanvas.SetActive(true);
		deleteJourneyIndex = index;
		GameObject deleteJourneyInstantiated = Instantiate(deleteJourneyPrefab, inFrontOfBottomPanel.transform);
		//delete button
		deleteJourneyInstantiated.transform.GetChild(0).GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(delegate {startExperienceDelete(deleteJourneyIndex); });
		deleteJourneyInstantiated.transform.GetChild(0).GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(delegate {Destroy(libraryPopupMenuInstantiated); });
		deleteJourneyInstantiated.transform.GetChild(0).GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(delegate {Destroy(deleteJourneyInstantiated); });
		//cancel button
		deleteJourneyInstantiated.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(delegate {Destroy(deleteJourneyInstantiated); });
	}
	

	public void deactivateLibraryStubs()
	{
		//destroy each spanwed in library stub
		foreach (GameObject libStub in libraryStubs)
		{
			Destroy(libStub);
		}
		Resources.UnloadUnusedAssets();
	}
}

