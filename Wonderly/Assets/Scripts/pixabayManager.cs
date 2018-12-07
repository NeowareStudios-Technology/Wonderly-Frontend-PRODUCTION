using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Sample;

public class pixabayManager : MonoBehaviour {
	public string searchUrl = "https://pixabay.com/api/?key=10416046-da227ed77f5d1960a9126dc7c&";
	public InputField searchTerm;
	public InputField coverImageSearchTerm;
	public Image[] searchedThumbnails = new Image[18];
	public Image[] searchedThumbnailsCoverImage = new Image[18];
	public Image blankImage;
	public GameObject image1;
	public GameObject image2;
	public GameObject image3;
	public GameObject image4;
	public GameObject image5;
	public Image coverImage;
	//public Text imageTitle1;
	//public Text imageTitle2;
	//public Text imageTitle3;
	//public Text imageTitle4;
	//public Text imageTitle5;
	public string[] chosenUrls = new string[5];
	public string chosenCoverImageUrl;
  public string[] imagePreviewUrl = new string[18];
	public string[] imageUrl = new string[18];
	public string[] coverImagePreviewUrl = new string[18];
	public string[] coverImageUrl = new string[18];
	public pixabayClass pxc;
	public FilesManager fm;
	public ArPairDisplayManager apdm;
	public LoadManager lm;

	public GameObject LoadingPanel;

	//starts coroutine because coroutine cannot be called by UI
	public void startSearchCoverImage() {
		StartCoroutine("searchPicCoverImage");
	}

	//makes web call for searching for image in pixabay repo
	public IEnumerator searchPicCoverImage() {
		//holds the search term
		string searchString = coverImageSearchTerm.text;
		Debug.Log(searchString);
		//makes search term url safe
		string urlSafeSearchTerm = searchString.Replace(" ", "+");
		//add delineator to search term for url
		string finalizedSearchTerm = "q=" + urlSafeSearchTerm;
		Debug.Log(finalizedSearchTerm);
		//create full search url
		string thisSearchUrl = searchUrl + finalizedSearchTerm;

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
					if (count == 18)
					{
						break;
					}
					coverImagePreviewUrl[count] = phc.previewURL;
					coverImageUrl[count] = phc.largeImageURL;
					count ++;
				}

				//load the image previews to their UI
				for (int j = 0; j < count; j++)
				{
					Debug.Log("download started");
					StartCoroutine(loadPreviewImageCoverImage(j));
				}
			}
		}
	}

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
				searchedThumbnailsCoverImage[index].sprite = Sprite.Create(previewRequest.texture, new Rect(0, 0, previewRequest.texture.width, previewRequest.texture.height), new Vector2(0, 0));
			}
		}
	}

	public void chooseCoverImageStarter(int index) {
			chosenCoverImageUrl = coverImageUrl[index];

			StartCoroutine(ChooseCoverImage(index));
		}

	

		public IEnumerator ChooseCoverImage(int index) {
			using (WWW imageRequest = new WWW(coverImageUrl[index]))
			{
				yield return imageRequest;
				Debug.Log("request worked");
				//catch errors
				if (imageRequest.error != null)
				{
					Debug.Log("Error getting image:" + imageRequest.error);
					LoadingPanel.SetActive(false);
				}
				else
				{
					coverImage.sprite = Sprite.Create(imageRequest.texture, new Rect(0, 0, imageRequest.texture.width, imageRequest.texture.height), new Vector2(0, 0));
					//coverImage.GetComponent<Renderer>().material.mainTexture = imageRequest.texture;
					
				}

				LoadingPanel.SetActive(false);
				yield return null;

				//clear url arrays and thumbnail images
			
				yield return new WaitForSeconds(2);
				coverImageUrl = new string[18];
				coverImagePreviewUrl = new string[18];
				searchedThumbnailsCoverImage[0].sprite = blankImage.sprite;
				searchedThumbnailsCoverImage[1].sprite = blankImage.sprite;
				searchedThumbnailsCoverImage[2].sprite = blankImage.sprite;
				searchedThumbnailsCoverImage[3].sprite = blankImage.sprite;
				searchedThumbnailsCoverImage[4].sprite = blankImage.sprite;
				searchedThumbnailsCoverImage[5].sprite = blankImage.sprite;
				searchedThumbnailsCoverImage[6].sprite = blankImage.sprite;
				searchedThumbnailsCoverImage[7].sprite = blankImage.sprite;
				searchedThumbnailsCoverImage[8].sprite = blankImage.sprite;
				searchedThumbnailsCoverImage[9].sprite = blankImage.sprite;
				searchedThumbnailsCoverImage[10].sprite = blankImage.sprite;
				searchedThumbnailsCoverImage[11].sprite = blankImage.sprite;
				searchedThumbnailsCoverImage[12].sprite = blankImage.sprite;
				searchedThumbnailsCoverImage[13].sprite = blankImage.sprite;
				searchedThumbnailsCoverImage[14].sprite = blankImage.sprite;
				searchedThumbnailsCoverImage[15].sprite = blankImage.sprite;
				searchedThumbnailsCoverImage[16].sprite = blankImage.sprite;
				searchedThumbnailsCoverImage[17].sprite = blankImage.sprite;

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
		string searchString = searchTerm.text;
		Debug.Log(searchString);
		//makes search term url safe
		string urlSafeSearchTerm = searchString.Replace(" ", "+");
		//add delineator to search term for url
		string finalizedSearchTerm = "q=" + urlSafeSearchTerm;
		Debug.Log(finalizedSearchTerm);
		//create full search url
		string thisSearchUrl = searchUrl + finalizedSearchTerm;

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
					if (count == 18)
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
				searchedThumbnails[index].sprite = Sprite.Create(previewRequest.texture, new Rect(0, 0, previewRequest.texture.width, previewRequest.texture.height), new Vector2(0, 0));
			}
		}
	}

		public void chooseImageStarter(int index) {

			//do nothing if current target not valid
			if (fm.currentTarget < 1)
				return;

			chosenUrls[fm.currentTarget -1] = imageUrl[index];
			lm.scd.imageUrl[fm.currentTarget -1] = imageUrl[index];

			StartCoroutine(ChooseImage(index));
		}

	

		public IEnumerator ChooseImage(int index) {
			using (WWW imageRequest = new WWW(imageUrl[index]))
			{
				yield return imageRequest;
				Debug.Log("request worked");
				//catch errors
				if (imageRequest.error != null)
				{
					Debug.Log("Error getting image:" + imageRequest.error);
					LoadingPanel.SetActive(false);
				}
				else
				{
					switch(fm.currentTarget)
					{
						case 1:
							image1.GetComponent<Renderer>().material.mainTexture = imageRequest.texture;
							StartCoroutine(SetArPairThumbnail(index));
							fm.targetStatus[0] = "image";
							break;
						case 2:
							image2.GetComponent<Renderer>().material.mainTexture = imageRequest.texture;
							StartCoroutine(SetArPairThumbnail(index));
							fm.targetStatus[1] = "image";
							break;
						case 3:
							image3.GetComponent<Renderer>().material.mainTexture = imageRequest.texture;
							StartCoroutine(SetArPairThumbnail(index));
							fm.targetStatus[2] = "image";
							break;
						case 4:
							image4.GetComponent<Renderer>().material.mainTexture = imageRequest.texture;
							StartCoroutine(SetArPairThumbnail(index));
							fm.targetStatus[3] = "image";
							break;
						case 5:
							image5.GetComponent<Renderer>().material.mainTexture = imageRequest.texture;
							StartCoroutine(SetArPairThumbnail(index));
							fm.targetStatus[4] = "image";
							break;
					}
				}

				LoadingPanel.SetActive(false);
				yield return null;

				//clear url arrays and thumbnail images
			/* 	
				yield return new WaitForSeconds(2);
				imageUrl = new string[18];
				imagePreviewUrl = new string[18];
				searchedThumbnails[0].sprite = blankImage.sprite;
				searchedThumbnails[1].sprite = blankImage.sprite;
				searchedThumbnails[2].sprite = blankImage.sprite;
				searchedThumbnails[3].sprite = blankImage.sprite;
				searchedThumbnails[4].sprite = blankImage.sprite;
				searchedThumbnails[5].sprite = blankImage.sprite;
				searchedThumbnails[6].sprite = blankImage.sprite;
				searchedThumbnails[7].sprite = blankImage.sprite;
				searchedThumbnails[8].sprite = blankImage.sprite;
				searchedThumbnails[9].sprite = blankImage.sprite;
				searchedThumbnails[10].sprite = blankImage.sprite;
				searchedThumbnails[11].sprite = blankImage.sprite;
				searchedThumbnails[12].sprite = blankImage.sprite;
				searchedThumbnails[13].sprite = blankImage.sprite;
				searchedThumbnails[14].sprite = blankImage.sprite;
				searchedThumbnails[15].sprite = blankImage.sprite;
				searchedThumbnails[16].sprite = blankImage.sprite;
				searchedThumbnails[17].sprite = blankImage.sprite;
			*/
			}
		}


		public IEnumerator SetArPairThumbnail(int index)
		{

			apdm.targetObjectThumbs[fm.currentTarget-1].sprite = searchedThumbnails[index].sprite;
/*/// 
				Image chosenThumb = apdm.chosenThumb1;
				switch(fm.currentTarget)
				{
					case 1:
						chosenThumb.sprite = apdm.targetObjectThumbs[0].sprite;
						break;
					case 2:
						chosenThumb = apdm.chosenThumb2;
						chosenThumb.sprite = apdm.targetObjectThumbs[1].sprite;
						break;
					case 3:
						chosenThumb = apdm.chosenThumb3;
						chosenThumb.sprite = apdm.targetObjectThumbs[2].sprite;
						break;
					case 4:
						chosenThumb = apdm.chosenThumb4;
						chosenThumb.sprite = apdm.targetObjectThumbs[3].sprite;
						break;
					case 5:
						chosenThumb = apdm.chosenThumb5;
						chosenThumb.sprite = apdm.targetObjectThumbs[4].sprite;
						break;
				}
///*/

				yield return null;
				//clear the url array
		}
}
