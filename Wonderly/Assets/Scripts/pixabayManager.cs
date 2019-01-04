/******************************************************
*Project: Wonderly
*Created by: David Lee Ramirez
*Date: 12/28/18
*Description: Used for all web calls to Pixabay api. Calls to 
							Pixabay are made in order to retrieve images for 
							AR linked objects or cover images.
*Copyright 2018 LeapWithAlice,LLC. All rights reserved
 ******************************************************/


using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class pixabayManager : MonoBehaviour {

	//script references
	public pixabayClass pxc;
	public FilesManager fm;
	public ArPairDisplayManager apdm;
	public LoadManager lm;
	//url for Pixabay image search
	public string searchUrl = "https://pixabay.com/api/?key=10416046-da227ed77f5d1960a9126dc7c&";
	//reference to UI: term to search pixabay for (linked image, create flow)
	public InputField searchTerm;
	//reference to UI: term to search pixabay for (cover image, create flow)
	public InputField coverImageSearchTerm;
	//reference to UI: term to search pixabay for (cover image, edit flow)
	public InputField coverImageSearchTerm2;
	//below is used for path FLOW2
	public InputField searchTerm2;
	public Text imageTitle2;
	public GameObject thumbNailParentContent2;
	public Animator viewLibraryContentPanel2;
	//array of image thumbnails (linked image)
	public Image[] searchedThumbnails = new Image[50];
	//array of image thumbnails (cover image)
	public Image[] searchedThumbnailsCoverImage = new Image[50];
	//used for blank sprite
	public Image blankImage;
	//references to AR targets
	public GameObject image1;
	public GameObject image2;
	public GameObject image3;
	public GameObject image4;
	public GameObject image5;
	//journey cover image
	public Image coverImage;
	//for displaying after choosing thumbnail from search
	public Text imageTitle;
	//array of all chosen image URLs for AR linked images 
	public string[] chosenUrls = new string[5];
	public string chosenCoverImageUrl;
	//for holding preview Image Urls (lower quality) and image urls (higher quality) for each thumbnail
  public string[] imagePreviewUrl = new string[50];
	public string[] imageUrl = new string[50];
	public string[] coverImagePreviewUrl = new string[50];
	public string[] coverImageUrl = new string[50];
	//for activating/deactivating loading panel
	public GameObject LoadingPanel;
	public int maxThumbResults = 50;
	//prefab for thumbnails
	public GameObject imgThumbPrefab;
	public GameObject thumbNailParentContent;
	public GameObject thumbNailParentContentCoverImage;
	public GameObject thumbNailParentContentCoverImage2;
	//for activating and deactivating cover image choosing
	public GameObject chooseCoverImagePanel;
	public GameObject chooseCoverImagePanel2;
	//for accessing Panel Controller
	public GameObject mainCanvas;
	public Animator viewLibraryContentPanel;
	public GameObject localScriptHolder;
	//Keeps track of instantiated thumbnails
	public GameObject[] thumbnailResults;


	//initialize array of thumbnails (empty game objects)
	void Awake()
	{
			thumbnailResults = new GameObject[maxThumbResults];
	}


	//Destroys thumbnails (create flow)
	public void DestroyChildrenOfCoverImageContent(){
		foreach (Transform child in thumbNailParentContentCoverImage.transform) {
			GameObject.Destroy(child.gameObject);
		}
		Resources.UnloadUnusedAssets();
	}


	//Destroys thumbnails (edit flow)
	public void DestroyChildrenOfCoverImageContent2(){
		foreach (Transform child in thumbNailParentContentCoverImage2.transform) {
			GameObject.Destroy(child.gameObject);
		}
		Resources.UnloadUnusedAssets();
	}


	//clears search text for cover image
	private void ClearSearchTextCoverImage(){
		coverImageSearchTerm.text = "";
	}


	//starts coroutine because coroutine cannot be called by UI
	//-searches for cover image
	public void startSearchCoverImage(int whichParent) {
		StartCoroutine(searchPicCoverImage(whichParent));
	}


	//makes web call for searching for image in pixabay repo
	public IEnumerator searchPicCoverImage(int whichParent) {
		//unload unused textures (memory)
		fm.unloadUnused();
		//holds the search term
		string searchString;
		if (whichParent == 1)
			searchString = coverImageSearchTerm.text;
		else 
			searchString = coverImageSearchTerm2.text;
		Debug.Log(searchString);
		//makes search term url safe
		string urlSafeSearchTerm = searchString.Replace(" ", "+");
		//add delineator to search term for url
		string finalizedSearchTerm = "q=" + urlSafeSearchTerm;
		Debug.Log(finalizedSearchTerm);
		//create full search url
		string per_page = "per_page=" + 50;
		string thisSearchUrl = searchUrl + finalizedSearchTerm + "&" + per_page;
		Debug.Log(thisSearchUrl);

		//create web request
		using (UnityWebRequest imageSearchRequest = UnityWebRequest.Get(thisSearchUrl))
		{
			//set content type
			imageSearchRequest.SetRequestHeader("Content-Type", "application/json");
			
			yield return imageSearchRequest.SendWebRequest();

			//catch errors
			if (imageSearchRequest.isNetworkError || imageSearchRequest.isHttpError)
    	{
			Debug.Log("Error getting image");
			}

			//show previews of each image
			else 
			{
				Debug.Log(imageSearchRequest.responseCode);
				byte[] results = imageSearchRequest.downloadHandler.data;
        string jsonString = Encoding.UTF8.GetString(results);
				Debug.Log(jsonString);
				pxc = JsonUtility.FromJson<pixabayClass>(jsonString);
				
				//get the url for each image returned in the image search request
				int count = 0;
				foreach (pixabayHitClass phc in pxc.hits)
				{
					if (count == 50)
					{
						break;
					}
					Debug.Log("didNotBreakYet");
					coverImagePreviewUrl[count] = phc.previewURL;
					Debug.Log("didNotBreakYet2");
					coverImageUrl[count] = phc.largeImageURL;
					count ++;
					
				}

				//load the image previews to their UI
				for (int j = 0; j < count; j++)
				{
					Debug.Log("download started");
					//depending on where the function is being called from, place thumbnails under different gameobjects
					if (whichParent == 1)
						StartCoroutine(loadPreviewImageCoverImage(j));
					else if (whichParent == 2)
						StartCoroutine(loadPreviewImageCoverImage2(j));
				}
			}
		}
		Debug.Log("finishedSearch WOOO");
	}


	//makes web call to get thumbnail
	private IEnumerator loadPreviewImageCoverImage(int index) {
		using (WWW previewRequest = new WWW(coverImagePreviewUrl[index]))
		{
			yield return previewRequest;
			//catch errors
			if (previewRequest.error != null)
    	{
				Debug.Log("Error getting image");
			}

			else
			{
				string thumbNailName = "imageThumbnail" + index;
       			GameObject newThumbnail = Instantiate(imgThumbPrefab);
				newThumbnail.name = thumbNailName;
				newThumbnail.transform.SetParent(thumbNailParentContentCoverImage.GetComponent<Transform>());
				thumbnailResults[index] = newThumbnail;
				newThumbnail.GetComponent<RectTransform>().localScale = new Vector3(1.0f,1.0f,1.0f);
				newThumbnail.GetComponent<Image>().sprite = Sprite.Create(previewRequest.texture, new Rect(0, 0, previewRequest.texture.width, previewRequest.texture.height), new Vector2(0, 0));
				newThumbnail.GetComponent<Button>().onClick.AddListener(delegate {chooseCoverImageStarter(index, newThumbnail);});
				newThumbnail.GetComponent<Button>().onClick.AddListener(delegate {chooseCoverImagePanel.SetActive(false);});
				//searchedThumbnailsCoverImage[index].sprite = Sprite.Create(previewRequest.texture, new Rect(0, 0, previewRequest.texture.width, previewRequest.texture.height), new Vector2(0, 0));
			}
		}
	}


	//second version putting thumbnails under different gameobject
	private IEnumerator loadPreviewImageCoverImage2(int index) {
		using (WWW previewRequest = new WWW(coverImagePreviewUrl[index]))
		{
			yield return previewRequest;
			//catch errors
			if (previewRequest.error != null)
    	{
				Debug.Log("Error getting image");
			}

			else
			{
				string thumbNailName = "imageThumbnail" + index;
       			GameObject newThumbnail = Instantiate(imgThumbPrefab);
				newThumbnail.name = thumbNailName;
				newThumbnail.transform.SetParent(thumbNailParentContentCoverImage2.GetComponent<Transform>());
				thumbnailResults[index] = newThumbnail;
				newThumbnail.GetComponent<RectTransform>().localScale = new Vector3(1.0f,1.0f,1.0f);
				newThumbnail.GetComponent<Image>().sprite = Sprite.Create(previewRequest.texture, new Rect(0, 0, previewRequest.texture.width, previewRequest.texture.height), new Vector2(0, 0));
				newThumbnail.GetComponent<Button>().onClick.AddListener(delegate {chooseCoverImageStarter(index, newThumbnail);});
				newThumbnail.GetComponent<Button>().onClick.AddListener(delegate {chooseCoverImagePanel2.SetActive(false);});
				//searchedThumbnailsCoverImage[index].sprite = Sprite.Create(previewRequest.texture, new Rect(0, 0, previewRequest.texture.width, previewRequest.texture.height), new Vector2(0, 0));
			}
		}
	}


	//chooses cover image from thumbnail
	public void chooseCoverImageStarter(int index, GameObject newThumbnail) {
			chosenCoverImageUrl = coverImageUrl[index];

			Debug.Log(coverImageUrl[index]);

			StartCoroutine(ChooseCoverImage(index, newThumbnail));
		}


	//makes web call to pixaby for selected image
	public IEnumerator ChooseCoverImage(int index, GameObject newThumbnail) {
		using (WWW imageRequest = new WWW(coverImageUrl[index]))
		{
			yield return imageRequest;
			Debug.Log("request worked");
			//catch errors
			fm.unloadUnused();
			ClearSearchTextCoverImage();
			if (imageRequest.error != null)
			{
				Debug.Log("Error getting image:" + imageRequest.error);
				LoadingPanel.SetActive(false);
			}
			else
			{
				coverImage.sprite = Sprite.Create(imageRequest.texture, new Rect(0, 0, imageRequest.texture.width, imageRequest.texture.height), new Vector2(0, 0));
				string coverImagePath = Path.Combine(fm.SaveDirectory, "coverImage.jpg");
				byte[] coverJpg = ImageConversion.EncodeToJPG(imageRequest.texture, 60); 
				File.WriteAllBytes(coverImagePath, coverJpg);
				
			}
			LoadingPanel.SetActive(false);
			yield return null;
		}
	}


	//logic for searching and applying picture to journey cover (above)

	/***************************************************************************************************************************************************************** */
	
	// logic for searching and applying pictures to targets (below)

	//starts coroutine because coroutine cannot be called by UI
	public void startSearch() {
		StartCoroutine("searchPic");
	}


	//makes web call for searching for image in pixabay repo
	public IEnumerator searchPic() {
		//holds the search term
		fm.unloadUnused();
		string searchString = searchTerm.text;
		imageTitle.text = searchString;
		Debug.Log("search string = " + searchString);
		//makes search term url safe
		string urlSafeSearchTerm = searchString.Replace(" ", "+");
		//add delineator to search term for url
		string finalizedSearchTerm = "q=" + urlSafeSearchTerm;

		string per_page = "per_page=" + 50;
		Debug.Log(finalizedSearchTerm);
		//create full search url
		string thisSearchUrl = searchUrl + finalizedSearchTerm + "&" + per_page;

		Debug.Log(thisSearchUrl);

		//create web request
		using (UnityWebRequest imageSearchRequest = UnityWebRequest.Get(thisSearchUrl))
		{
			//set content type
			imageSearchRequest.SetRequestHeader("Content-Type", "application/json");
			
			yield return imageSearchRequest.SendWebRequest();

			//catch errors
			if (imageSearchRequest.isNetworkError || imageSearchRequest.isHttpError)
    	{
			Debug.Log("Error getting image");
			}

			//show previews of each image
			else 
			{
				Debug.Log(imageSearchRequest.responseCode);
				byte[] results = imageSearchRequest.downloadHandler.data;
        		string jsonString = Encoding.UTF8.GetString(results);
				Debug.Log(jsonString);
				pxc = JsonUtility.FromJson<pixabayClass>(jsonString);

				//get the url for each image returned in the image search request
				int count = 0;
				foreach (pixabayHitClass phc in pxc.hits)
				{
					
					if (count == 50)
					{
						break;
					}
					imagePreviewUrl[count] = phc.previewURL;
					imageUrl[count] = phc.largeImageURL;
					count ++;
				}

				//load the image previews to their UI
				for (int j = 0; j < count; j++)
				{
					Debug.Log("download started");
					StartCoroutine(loadPreviewImage(j));
				}
			}
		}
	}


  //loads thumbnail image
	private IEnumerator loadPreviewImage(int index) {
		using (WWW previewRequest = new WWW(imagePreviewUrl[index]))
		{
			yield return previewRequest;
			//catch errors
			if (previewRequest.error != null)
    	{
				Debug.Log("Error getting image");
			}

			else
			{
				string thumbNailName = "imageThumbnail" + index;
						GameObject newThumbnail = Instantiate(imgThumbPrefab);
				newThumbnail.name = thumbNailName;

				//newThumbnail.GetComponent<Image>().sprite = Sprite.Create(asset.thumbnailTexture, rec, new Vector2(0.5f, 0.5f), 100);

				newThumbnail.transform.SetParent(thumbNailParentContent.GetComponent<Transform>());
				thumbnailResults[index] = newThumbnail;
				
				newThumbnail.GetComponent<RectTransform>().localScale = new Vector3(1.0f,1.0f,1.0f);
				newThumbnail.GetComponent<Image>().sprite = Sprite.Create(previewRequest.texture, new Rect(0, 0, previewRequest.texture.width, previewRequest.texture.height), new Vector2(0, 0));
				newThumbnail.GetComponent<Button>().onClick.AddListener(delegate {mainCanvas.GetComponent<PanelController>().OpenPanel(viewLibraryContentPanel);});
				newThumbnail.GetComponent<Button>().onClick.AddListener(delegate {chooseImageStarter(index, newThumbnail);});
				newThumbnail.GetComponent<Button>().onClick.AddListener(delegate {fm.ModifyTargetStatusArray("image");});
				newThumbnail.GetComponent<Button>().onClick.AddListener(delegate {localScriptHolder.GetComponent<UiManager>().SetLoadingPanelActive(true);});
			}
		}
	}


		//selects image to download from Pixabay
		public void chooseImageStarter(int index, GameObject nThumbnail) {

			//do nothing if current target not valid
			if (fm.currentTarget < 1)
				return;

			chosenUrls[fm.currentTarget -1] = imageUrl[index];
			lm.scd.imageUrl[fm.currentTarget -1] = imageUrl[index];

			StartCoroutine(ChooseImage(index, nThumbnail));
		}


		//makes web call to pixaby for selected image
		public IEnumerator ChooseImage(int index, GameObject newThumbnail) {
			using (WWW imageRequest = new WWW(imageUrl[index]))
			{
				yield return imageRequest;
				Debug.Log("request worked");
				fm.unloadUnused();
				ClearSearchTextImage();
				//catch errors
				
				if (imageRequest.error != null)
				{
					Debug.Log("Error getting image:" + imageRequest.error);
					//LoadingPanel.SetActive(false);
				}
				else
				{   
					Rect rec = new Rect(0, 0, imageRequest.texture.width, imageRequest.texture.height);
					newThumbnail.GetComponent<Image>().sprite = Sprite.Create(imageRequest.texture, rec, new Vector2(0.5f, 0.5f), 100);
					StartCoroutine(SetArPairThumbnail(newThumbnail));

					switch(fm.currentTarget)
					{
						case 1:
							image1.GetComponent<Renderer>().material.mainTexture = imageRequest.texture;
							StartCoroutine(SetArPairThumbnail(newThumbnail));
							string linkedImage1Path = Path.Combine(fm.SaveDirectory, "linkedImage1.jpg");
							byte[] linkedJpg1 = ImageConversion.EncodeToJPG(imageRequest.texture, 60); 
							File.WriteAllBytes(linkedImage1Path, linkedJpg1);
							fm.targetStatus[0] = "image";
							break;
						case 2:
							image2.GetComponent<Renderer>().material.mainTexture = imageRequest.texture;
							StartCoroutine(SetArPairThumbnail(newThumbnail));
							string linkedImage2Path = Path.Combine(fm.SaveDirectory, "linkedImage2.jpg");
							byte[] linkedJpg2 = ImageConversion.EncodeToJPG(imageRequest.texture, 60); 
							File.WriteAllBytes(linkedImage2Path, linkedJpg2);
							fm.targetStatus[1] = "image";
							break;
						case 3:
							image3.GetComponent<Renderer>().material.mainTexture = imageRequest.texture;
							StartCoroutine(SetArPairThumbnail(newThumbnail));
							string linkedImage3Path = Path.Combine(fm.SaveDirectory, "linkedImage3.jpg");
							byte[] linkedJpg3 = ImageConversion.EncodeToJPG(imageRequest.texture, 60); 
							File.WriteAllBytes(linkedImage3Path, linkedJpg3);
							fm.targetStatus[2] = "image";
							break;
						case 4:
							image4.GetComponent<Renderer>().material.mainTexture = imageRequest.texture;
							StartCoroutine(SetArPairThumbnail(newThumbnail));
							string linkedImage4Path = Path.Combine(fm.SaveDirectory, "linkedImage4.jpg");
							byte[] linkedJpg4 = ImageConversion.EncodeToJPG(imageRequest.texture, 60); 
							File.WriteAllBytes(linkedImage4Path, linkedJpg4);
							fm.targetStatus[3] = "image";
							break;
						case 5:
							image5.GetComponent<Renderer>().material.mainTexture = imageRequest.texture;
							StartCoroutine(SetArPairThumbnail(newThumbnail));
							string linkedImage5Path = Path.Combine(fm.SaveDirectory, "linkedImage5.jpg");
							byte[] linkedJpg5 = ImageConversion.EncodeToJPG(imageRequest.texture, 60); 
							File.WriteAllBytes(linkedImage5Path, linkedJpg5);
							fm.targetStatus[4] = "image";
							break;
					}
				}
				localScriptHolder.GetComponent<UiManager>().SetLoadingPanelActive(false);
				//LoadingPanel.SetActive(false);
				
				yield return null;

			}
		}


		//set texture for UI display of pixabay image (not target display)
		public IEnumerator SetArPairThumbnail(GameObject newThumbnail)
		{

			apdm.targetObjectThumbs[fm.currentTarget-1].sprite = newThumbnail.GetComponent<Image>().sprite;
			apdm.chosenThumb.sprite = newThumbnail.GetComponent<Image>().sprite;
			yield return null;
				//clear the url array
		}


		//destroys and releases memory of thumbnails for create flow
		public void DestroyChildrenOfImageContent(){
			foreach (Transform child in thumbNailParentContent.transform) {
				GameObject.Destroy(child.gameObject);
			}
			Resources.UnloadUnusedAssets();
		}
		private void ClearSearchTextImage(){
			searchTerm.text = "";
		}


/************************************************************************************************************************************************************* */
//below is logic for getting image thumbnails and selecting image from the edit path FLOW2


	//starts coroutine because coroutine cannot be called by UI
	public void startSearch2() {
		StartCoroutine("searchPic2");
	}


	//makes web call for searching for image in pixabay repo
	public IEnumerator searchPic2() {
		//holds the search term
		fm.unloadUnused();
		string searchString = searchTerm2.text;
		imageTitle2.text = searchString;
		Debug.Log("search string = " + searchString);
		//makes search term url safe
		string urlSafeSearchTerm = searchString.Replace(" ", "+");
		//add delineator to search term for url
		string finalizedSearchTerm = "q=" + urlSafeSearchTerm;

		string per_page = "per_page=" + 50;
		Debug.Log(finalizedSearchTerm);
		//create full search url
		string thisSearchUrl = searchUrl + finalizedSearchTerm + "&" + per_page;

		Debug.Log(thisSearchUrl);

		//create web request
		using (UnityWebRequest imageSearchRequest = UnityWebRequest.Get(thisSearchUrl))
		{
			//set content type
			imageSearchRequest.SetRequestHeader("Content-Type", "application/json");
			
			yield return imageSearchRequest.SendWebRequest();

			//catch errors
			if (imageSearchRequest.isNetworkError || imageSearchRequest.isHttpError)
    	{
			Debug.Log("Error getting image");
			}

			//show previews of each image
			else 
			{
				Debug.Log(imageSearchRequest.responseCode);
				byte[] results = imageSearchRequest.downloadHandler.data;
        		string jsonString = Encoding.UTF8.GetString(results);
				Debug.Log(jsonString);
				pxc = JsonUtility.FromJson<pixabayClass>(jsonString);

				//get the url for each image returned in the image search request
				int count = 0;
				foreach (pixabayHitClass phc in pxc.hits)
				{
					
					if (count == 50)
					{
						break;
					}
					imagePreviewUrl[count] = phc.previewURL;
					imageUrl[count] = phc.largeImageURL;
					count ++;
				}

				//load the image previews to their UI
				for (int j = 0; j < count; j++)
				{
					Debug.Log("download started");
					StartCoroutine(loadPreviewImage2(j));
				}
			}
		}
	}


	//loads thumbnail image
	private IEnumerator loadPreviewImage2(int index) {
		using (WWW previewRequest = new WWW(imagePreviewUrl[index]))
		{
			yield return previewRequest;
			//catch errors
			if (previewRequest.error != null)
    	{
				Debug.Log("Error getting image");
			}
			else
			{
				string thumbNailName = "imageThumbnail" + index;
						GameObject newThumbnail = Instantiate(imgThumbPrefab);
				newThumbnail.name = thumbNailName;

				//newThumbnail.GetComponent<Image>().sprite = Sprite.Create(asset.thumbnailTexture, rec, new Vector2(0.5f, 0.5f), 100);

				newThumbnail.transform.SetParent(thumbNailParentContent2.GetComponent<Transform>());
				thumbnailResults[index] = newThumbnail;
				
				newThumbnail.GetComponent<RectTransform>().localScale = new Vector3(1.0f,1.0f,1.0f);
				newThumbnail.GetComponent<Image>().sprite = Sprite.Create(previewRequest.texture, new Rect(0, 0, previewRequest.texture.width, previewRequest.texture.height), new Vector2(0, 0));
				newThumbnail.GetComponent<Button>().onClick.AddListener(delegate {mainCanvas.GetComponent<PanelController>().OpenPanel(viewLibraryContentPanel2);});
				newThumbnail.GetComponent<Button>().onClick.AddListener(delegate {chooseImageStarter2(index, newThumbnail);});
				newThumbnail.GetComponent<Button>().onClick.AddListener(delegate {fm.ModifyTargetStatusArray("image");});
				newThumbnail.GetComponent<Button>().onClick.AddListener(delegate {localScriptHolder.GetComponent<UiManager>().SetLoadingPanelActive(true);});
			}
		}
	}


	//selects image to download from Pixabay
	public void chooseImageStarter2(int index, GameObject nThumbnail) {

			//do nothing if current target not valid
			if (fm.currentTarget < 1)
				return;

			chosenUrls[fm.currentTarget -1] = imageUrl[index];
			lm.scd.imageUrl[fm.currentTarget -1] = imageUrl[index];

			StartCoroutine(ChooseImage2(index, nThumbnail));
		}

	
		//makes web call to pixaby for selected image
		public IEnumerator ChooseImage2(int index, GameObject newThumbnail) {
			using (WWW imageRequest = new WWW(imageUrl[index]))
			{
				yield return imageRequest;
				Debug.Log("request worked");
				fm.unloadUnused();
				ClearSearchTextImage2();
				//catch errors
				
				if (imageRequest.error != null)
				{
					Debug.Log("Error getting image:" + imageRequest.error);
					LoadingPanel.SetActive(false);
				}
				else
				{   
					Rect rec = new Rect(0, 0, imageRequest.texture.width, imageRequest.texture.height);
					newThumbnail.GetComponent<Image>().sprite = Sprite.Create(imageRequest.texture, rec, new Vector2(0.5f, 0.5f), 100);
					StartCoroutine(SetArPairThumbnail2(newThumbnail));
					
					switch(fm.currentTarget)
					{
						case 1:
							image1.GetComponent<Renderer>().material.mainTexture = imageRequest.texture;
							StartCoroutine(SetArPairThumbnail2(newThumbnail));
							string linkedImage1Path = Path.Combine(fm.SaveDirectory, "linkedImage1.jpg");
							byte[] linkedJpg1 = ImageConversion.EncodeToJPG(imageRequest.texture, 60); 
							File.WriteAllBytes(linkedImage1Path, linkedJpg1);
							fm.targetStatus[0] = "image";
							break;
						case 2:
							image2.GetComponent<Renderer>().material.mainTexture = imageRequest.texture;
							StartCoroutine(SetArPairThumbnail2(newThumbnail));
							string linkedImage2Path = Path.Combine(fm.SaveDirectory, "linkedImage2.jpg");
							byte[] linkedJpg2 = ImageConversion.EncodeToJPG(imageRequest.texture, 60); 
							File.WriteAllBytes(linkedImage2Path, linkedJpg2);
							fm.targetStatus[1] = "image";
							break;
						case 3:
							image3.GetComponent<Renderer>().material.mainTexture = imageRequest.texture;
							StartCoroutine(SetArPairThumbnail2(newThumbnail));
							string linkedImage3Path = Path.Combine(fm.SaveDirectory, "linkedImage3.jpg");
							byte[] linkedJpg3 = ImageConversion.EncodeToJPG(imageRequest.texture, 60); 
							File.WriteAllBytes(linkedImage3Path, linkedJpg3);
							fm.targetStatus[2] = "image";
							break;
						case 4:
							image4.GetComponent<Renderer>().material.mainTexture = imageRequest.texture;
							StartCoroutine(SetArPairThumbnail2(newThumbnail));
							string linkedImage4Path = Path.Combine(fm.SaveDirectory, "linkedImage4.jpg");
							byte[] linkedJpg4 = ImageConversion.EncodeToJPG(imageRequest.texture, 60); 
							File.WriteAllBytes(linkedImage4Path, linkedJpg4);
							fm.targetStatus[3] = "image";
							break;
						case 5:
							image5.GetComponent<Renderer>().material.mainTexture = imageRequest.texture;
							StartCoroutine(SetArPairThumbnail2(newThumbnail));
							string linkedImage5Path = Path.Combine(fm.SaveDirectory, "linkedImage5.jpg");
							byte[] linkedJpg5 = ImageConversion.EncodeToJPG(imageRequest.texture, 60); 
							File.WriteAllBytes(linkedImage5Path, linkedJpg5);
							fm.targetStatus[4] = "image";
							break;
					}
				}
				LoadingPanel.SetActive(false);
				yield return null;
			}
		}


		//set texture for UI display of pixabay image (not target display)
		public IEnumerator SetArPairThumbnail2(GameObject newThumbnail)
		{

			apdm.targetObjectThumbs[fm.currentTarget-1].sprite = newThumbnail.GetComponent<Image>().sprite;
			apdm.chosenThumb2.sprite = newThumbnail.GetComponent<Image>().sprite;


				yield return null;
				//clear the url array
		}


		//destroys thumbnails and releases memory for edit flow
		public void DestroyChildrenOfImageContent2(){
			foreach (Transform child in thumbNailParentContent2.transform) {
				GameObject.Destroy(child.gameObject);
			}
			Resources.UnloadUnusedAssets();
		}
		private void ClearSearchTextImage2(){
			searchTerm.text = "";
		}
}
