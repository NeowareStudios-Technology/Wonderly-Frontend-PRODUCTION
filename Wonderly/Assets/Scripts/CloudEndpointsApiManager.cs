using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using UnityEngine.Networking;
using System.IO;
using UnityEngine.SceneManagement;
using Sample;
using System.Threading.Tasks;


public class CloudEndpointsApiManager : MonoBehaviour {

	public GameObject lsh;
	public FirebaseManager fbm;
	public FirebaseStorageManager fsm;
	public SaveManager sm;
	public FilesManager fm;
	public ErrorMessageFlowManager emfm;
	public OwnedExperiencesClass oec;
	public TargetIndicesClass tic;
	public ProfileInfoClass pic;
	public checkEmailClass cec;
	public checkEmailResponseClass cerc;
	public CreateOrEditController coec;
	public UiManager um;
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

	public Animator journeySummaryAnimator;
	public Animator ViewScreenAnimator;
	public Animator PreviewScreenAnimator;

	public PanelController mainCanvasPanelController;

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

	public string code;

	public string deleteCode;
	public string editCode;
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

			loadingPanel.SetActive(false);

			Debug.Log(newProfileRequest.responseCode);

			//load in profile info to ui (called here because need to wait for cookie and profile creation)
			StartCoroutine("getProfileInfo");
		}

	}

	public void startProfileEdit()
	{
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

		using (UnityWebRequest editProfileRequest = UnityWebRequest.Put(editProfileUrl,editProfJson))
		{
			//set content type
			editProfileRequest.SetRequestHeader("Content-Type", "application/json");
			//set auth header
			editProfileRequest.SetRequestHeader("Authorization", "Bearer " + fbm.token);

			yield return editProfileRequest.SendWebRequest();
			Debug.Log(editProfileRequest.responseCode);

			if ((fbm.editPassword.text != "") && (fbm.editPassword2.text != ""))
			{
				fbm.changeUserPassword();
			}

			//call this to fill the profile page with the changed content (lets user know edit worked)
			//getProfileInfo will disable loading screen on completion
			StartCoroutine("getProfileInfo");
		}
	}


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

	//called by ui button to delete experience (journey)
	public void startExperienceDelete(int index)
	{
		deleteCode = libraryCodes[index-1];
		//delete experience from google datastore noSQL database
		StartCoroutine("deleteExperienceFromDataStore");

		//delete experience from Firebase storage file store
		fsm.DeleteExperience(deleteCode);

		//get rid of the library stub that was holding the journey info
		Destroy(libraryStubs[index-1]);
	}

	public IEnumerator deleteExperienceFromDataStore() 
	{
		ExperienceCodeClass expCode = new ExperienceCodeClass();
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
			byte[] results = deleteExperienceRequest.downloadHandler.data;
			string jsonString = Encoding.UTF8.GetString(results);
			Debug.Log(jsonString);
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



		using (UnityWebRequest editExperienceRequest = UnityWebRequest.Put(editExpUrl,editExperienceJson))
		{
			//set content type
			editExperienceRequest.SetRequestHeader("Content-Type", "application/json");
			//set auth header
			editExperienceRequest.SetRequestHeader("Authorization", "Bearer " + fbm.token);

			yield return editExperienceRequest.SendWebRequest();

			Debug.Log(editExperienceRequest.responseCode);
		}
	}


public void startGetProfileInfo()
	{
		StartCoroutine("getProfileInfo");
	}



	public IEnumerator getProfileInfo() 
	{
        //lsh.GetComponent<UiManager>().SetLoadingPanelActive(true);
        using (UnityWebRequest newProfileInfoRequest = UnityWebRequest.Get(getProfileUrl))
        {
            //set content type
            newProfileInfoRequest.SetRequestHeader("Content-Type", "application/json");
            //set auth header
            newProfileInfoRequest.SetRequestHeader("Authorization", "Bearer " + fbm.token);

            yield return newProfileInfoRequest.SendWebRequest();

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
		}
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
			
			libraryStubs[i].transform.GetChild(1).gameObject.GetComponent<Text>().text = oec.titles[i];
			libraryStubs[i].transform.GetChild(2).gameObject.GetComponent<Text>().text = oec.codes[i];
			// Get Child 5 = JourneyCoverPhoto
			libraryStubs[i].transform.GetChild(5).gameObject.GetComponent<Button>().onClick.AddListener(delegate {fsm.startDownloadExperienceFilesDirect(index+1); });
			libraryStubs[i].transform.GetChild(5).gameObject.GetComponent<Button>().onClick.AddListener(delegate {mainCanvasPanelController.OpenPanel(PreviewScreenAnimator); });
			libraryStubs[i].transform.GetChild(5).gameObject.GetComponent<Button>().onClick.AddListener(delegate {iconPanel.SetActive(false); });
			libraryStubs[i].transform.GetChild(5).gameObject.GetComponent<Button>().onClick.AddListener(delegate {lsh.GetComponent<UiManager>().SetLoadingPanelActive(true); });
			libraryStubs[i].transform.GetChild(7).gameObject.GetComponent<Button>().onClick.AddListener(delegate {createLibraryPopup(index); });
			libraryStubs[i].transform.GetChild(8).gameObject.GetComponent<Text>().text = oec.dates[i];
			libraryCodes[i] = oec.codes[i];

		//	StartCoroutine(loadJourneyCoverImage(libraryStubs[i], oec.coverImages[i]));

	}
	lsh.GetComponent<UiManager>().SetLoadingPanelActive(false);
	}
		
/* 
	private IEnumerator loadJourneyCoverImage(GameObject libStub, string coverImageUrl) {
		using (WWW imageRequest = new WWW(coverImageUrl))
		{
			yield return imageRequest;
			//catch errors
			if (imageRequest.error != null)
    	{
				Debug.Log("Error getting image");
			}

			else
			{	
				libStub.transform.GetChild(5).gameObject.GetComponent<Image>().sprite = Sprite.Create(imageRequest.texture, new Rect(0, 0, imageRequest.texture.width, imageRequest.texture.height), new Vector2(0, 0));
			}
		}
	}*/

	//creates library menu UI
	void createLibraryPopup(int index)
	{
		frontCanvas.SetActive(true);
		libraryPopupMenuInstantiated = Instantiate(libraryPopupMenuPrefab, inFrontOfBottomPanel.transform);
		//delete button
		libraryPopupMenuInstantiated.transform.GetChild(2).GetChild(3).gameObject.GetComponent<Button>().onClick.AddListener(delegate {startExperienceDelete(index+1); });
		libraryPopupMenuInstantiated.transform.GetChild(2).GetChild(3).gameObject.GetComponent<Button>().onClick.AddListener(delegate {frontCanvas.SetActive(false); });
		libraryPopupMenuInstantiated.transform.GetChild(2).GetChild(3).gameObject.GetComponent<Button>().onClick.AddListener(delegate {Destroy(libraryPopupMenuInstantiated); });
		//edit button
		libraryPopupMenuInstantiated.transform.GetChild(2).GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(delegate {um.InstantResetSummaryScreen(); });
		libraryPopupMenuInstantiated.transform.GetChild(2).GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(delegate {startExperienceEdit(index+1); });
		libraryPopupMenuInstantiated.transform.GetChild(2).GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(delegate {lsh.GetComponent<UiManager>().SetLoadingPanelActive(true); });
		libraryPopupMenuInstantiated.transform.GetChild(2).GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(delegate {mainCanvasPanelController.OpenPanel(journeySummaryAnimator); });
		libraryPopupMenuInstantiated.transform.GetChild(2).GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(delegate {iconPanel.SetActive(false); });
		libraryPopupMenuInstantiated.transform.GetChild(2).GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(delegate {coec.setCreateOrEdit("edit"); });
		//libraryPopupMenuInstantiated.transform.GetChild(2).GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(delegate {frontCanvas.SetActive(false); });
		libraryPopupMenuInstantiated.transform.GetChild(2).GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(delegate {Destroy(libraryPopupMenuInstantiated); });
		//cancel button
		libraryPopupMenuInstantiated.transform.GetChild(4).GetChild(2).gameObject.GetComponent<Button>().onClick.AddListener(delegate {frontCanvas.SetActive(false); });
		libraryPopupMenuInstantiated.transform.GetChild(4).GetChild(2).gameObject.GetComponent<Button>().onClick.AddListener(delegate {Destroy(libraryPopupMenuInstantiated); });
		//share
		libraryPopupMenuInstantiated.transform.GetChild(2).GetChild(2).gameObject.GetComponent<Button>().onClick.AddListener(delegate {lsh.GetComponent<Sharing>().SharingCodeJourneyContextMenu(index); });
		libraryPopupMenuInstantiated.transform.GetChild(2).GetChild(2).gameObject.GetComponent<Button>().onClick.AddListener(delegate {Destroy(libraryPopupMenuInstantiated); });
		libraryPopupMenuInstantiated.transform.GetChild(2).GetChild(2).gameObject.GetComponent<Button>().onClick.AddListener(delegate {frontCanvas.SetActive(false); });
	}
	

	public void deactivateLibraryStubs()
	{
		//destroy each spanwed in library stub
		foreach (GameObject libStub in libraryStubs)
		{
			Destroy(libStub);
		}
	}
}

