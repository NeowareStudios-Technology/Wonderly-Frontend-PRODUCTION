using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Sample;
using PolyToolkit;
using EasyAR;

public class LoadManager : MonoBehaviour {
	public FilesManager fm;
	public SaveClassDeclaration scd;
	public targetObjectManager tom; 
	public ImageTargetManager itm;
	public pixabayManager pm;
	public ArPairDisplayManager apdm;
	public SaveManager sm;
	public UnityEngine.UI.Image targetPreview1;
	public UnityEngine.UI.Image targetPreview2;
	public UnityEngine.UI.Image targetPreview3;
	public UnityEngine.UI.Image targetPreview4;
	public UnityEngine.UI.Image targetPreview5;

	public GameObject filledIn1;
	public GameObject filledIn2;
	public GameObject filledIn3;
	public GameObject filledIn4;
	public GameObject filledIn5;

	public GameObject unfilled1;
	public GameObject unfilled2;
	public GameObject unfilled3;
	public GameObject unfilled4;
	public GameObject unfilled5;

	public UnityEngine.UI.Image linkedThumb1;
	public UnityEngine.UI.Image linkedThumb2;
	public UnityEngine.UI.Image linkedThumb3;
	public UnityEngine.UI.Image linkedThumb4;
	public UnityEngine.UI.Image linkedThumb5;

	public GameObject loadingPanel;

	public UnityEngine.UI.Image coverImage;
	public InputField titleDisplay;
	public Text summaryTitleDisplay;
	public InputField descriptionDisplay;
	public Text summaryDescriptionDisplay;

	public GameObject targetSetter;

	public int previewIndex;

	public PolyAsset[] allAssets = new PolyAsset[5];

	private string coverPath;
	private string linkedPath1;
	private string linkedPath2;
	private string linkedPath3;
	private string linkedPath4;
	private string linkedPath5;
	private string workingPath1;
	private string workingPath2;
	private string workingPath3;
	private string workingPath4;
	private string workingPath5;
	private string targetPath1;
	private string targetPath2;
	private string targetPath3;
	private string targetPath4;
	private string targetPath5;

	//for previewing journey before viewing
	public UnityEngine.UI.Image previewCoverImage;
	public Text previewTitleDisplay;
	public Text previewDescriptionDisplay;
	public UnityEngine.UI.Image viewFlowTargetPreview1;
	public UnityEngine.UI.Image viewFlowTargetPreview2;
	public UnityEngine.UI.Image viewFlowTargetPreview3;
	public UnityEngine.UI.Image viewFlowTargetPreview4;
	public UnityEngine.UI.Image viewFlowTargetPreview5;

	//keeps track of first available index for a model
	public int[] modelIndices = new int[5];

	//keeps track of first available index for a model
	public int[] videoIndices = new int[5];

	public int globalModelArrayIndex;
	public int globalModelIndexTracker;



	//Main function in this file that calls other helper functions 
	public void LoadFile() {

		previewIndex = 0;
		targetSetter.SetActive(false);

		//for debugging iOS load issue
		Debug.Log("1. lm70, Starting LoadFile()");

		//clear the scene
		tom.clearScene();


		//make sure save directory to load from exists, if it does import the save info from json save file to scd class
		string savePath = Path.Combine(fm.SaveDirectory, "aoSave.json");
		if (File.Exists(savePath))
		{
			string jsonString = File.ReadAllText(savePath);
			//for debugging iOS load issue
			Debug.Log("2. lm82, Contents of save file = "+jsonString);
			scd = SaveClassDeclaration.CreateFromJSON(jsonString);
		}
		else 
		{
			Debug.Log("no save file");
			return;
		}

		//set up paths for all local images
		workingPath1 = Path.Combine(fm.MarksDirectory, "targetPhoto1.jpg");
		workingPath2 = Path.Combine(fm.MarksDirectory, "targetPhoto2.jpg");
		workingPath3 = Path.Combine(fm.MarksDirectory, "targetPhoto3.jpg");
		workingPath4 = Path.Combine(fm.MarksDirectory, "targetPhoto4.jpg");
		workingPath5 = Path.Combine(fm.MarksDirectory, "targetPhoto5.jpg");
		targetPath1 = Path.Combine(fm.SaveDirectory, "targetPhoto1.jpg");
		targetPath2 = Path.Combine(fm.SaveDirectory, "targetPhoto2.jpg");
		targetPath3 = Path.Combine(fm.SaveDirectory, "targetPhoto3.jpg");
		targetPath4 = Path.Combine(fm.SaveDirectory, "targetPhoto4.jpg");
		targetPath5 = Path.Combine(fm.SaveDirectory, "targetPhoto5.jpg");
		coverPath = Path.Combine(fm.SaveDirectory, "coverImage.jpg");
		linkedPath1 = Path.Combine(fm.SaveDirectory, "linkedImage1.jpg");
		linkedPath2 = Path.Combine(fm.SaveDirectory, "linkedImage2.jpg");
		linkedPath3 = Path.Combine(fm.SaveDirectory, "linkedImage3.jpg");
		linkedPath4 = Path.Combine(fm.SaveDirectory, "linkedImage4.jpg");
		linkedPath5 = Path.Combine(fm.SaveDirectory, "linkedImage5.jpg");

		//load journey cover image
		StartCoroutine("loadJourneyCoverImage");

		Debug.Log("3. lm91, Title saved to save class = "+scd.title);
		//set the experience title
		Debug.Log(scd.title);
		if (scd.title == "")
		{
			titleDisplay.text= "  ";
			summaryTitleDisplay.text = "  ";
			previewTitleDisplay.text = "  ";
		}
		else
		{
			titleDisplay.text = scd.title;
			summaryTitleDisplay.text = scd.title;
			previewTitleDisplay.text = scd.title;
		}

		//set the experience description
		if (scd.description == "")
		{
			descriptionDisplay.text = "  ";
			summaryDescriptionDisplay.text = "   ";
			previewDescriptionDisplay.text = "   " ;
		}
		else
		{
			descriptionDisplay.text = scd.description;
			summaryDescriptionDisplay.text = scd.description;
			previewDescriptionDisplay.text = scd.description;
		}
		Debug.Log("1");

		//set cover image url
		//sm.coverImageUrl = scd.coverImageUrl;
		pm.chosenCoverImageUrl = scd.coverImageUrl;
		Debug.Log("2");

		//set each wonder title
		for (int g = 0; g < 3; g++)
		{
			Debug.Log("in wonder title set");
			apdm.wonderTitles[g].text = scd.wonderTitle[g];
			apdm.previewWonderTitles[g].text = scd.wonderTitle[g];
			sm.wonderTitles[g] = scd.wonderTitle[g];

		}
		Debug.Log("2");

		//set each wonder description (CHANGE THIS TO % IF NEED TO INCREASE TARGETS TO 5)
		for (int h = 0; h < 3; h++)
		{
			apdm.wonderDescriptions[h].text = scd.wonderDescription[h];
			apdm.previewWonderDescriptions[h].text = scd.wonderDescription[h];
			sm.wonderDescriptions[h] = scd.wonderDescription[h];
		}
		Debug.Log("3");

		for (int i =0; i <5; i++)
		{
			Debug.Log("4~. lm105, targetStatus["+i+"] from save class = "+scd.targetStatus[i]);
			fm.targetStatus[i] = scd.targetStatus[i];
		}

		//for debugging iOS
		Debug.Log("5a. lm122, target1 working path = "+workingPath1);
		Debug.Log("5b. lm123, target2 working path = "+workingPath2);
		Debug.Log("5c. lm124, target3 working path = "+workingPath3);
		Debug.Log("5d. lm125, target4 working path = "+workingPath4);
		Debug.Log("5e. lm126, target5 working path = "+workingPath5);
		Debug.Log("6a. lm127, target1 save file path = "+targetPath1);
		Debug.Log("6b. lm128, target2 save file path = "+targetPath2);
		Debug.Log("6c. lm129, target3 save file path = "+targetPath3);
		Debug.Log("6d. lm130, target4 save file path = "+targetPath4);
		Debug.Log("6e. lm131, target5 save file path = "+targetPath5);

		//copy the target images from the save directory to the working directory
		if (File.Exists(targetPath1))
		{
			//for debugging iOS
			Debug.Log("7. lm137, target1 save file exists");
			System.IO.File.Copy(targetPath1, workingPath1, true);
			fm.targetCount++;
		}
		if (File.Exists(targetPath2))
		{
			//for debugging iOS
			Debug.Log("8. lm144, target2 save file exists");
			System.IO.File.Copy(targetPath2, workingPath2, true);
			fm.targetCount++;
		}
		if (File.Exists(targetPath3))
		{
			//for debugging iOS
			Debug.Log("9. lm151, target3 save file exists");
			System.IO.File.Copy(targetPath3, workingPath3, true);
			fm.targetCount++;
		}
		if (File.Exists(targetPath4))
		{
			//for debugging iOS
			Debug.Log("10. lm158, target4 save file exists");
			System.IO.File.Copy(targetPath4, workingPath4, true);
			fm.targetCount++;
		}
		if (File.Exists(targetPath5))
		{
			//for debugging iOS
			Debug.Log("11. lm165, target5 save file exists");
			System.IO.File.Copy(targetPath5, workingPath5, true);
			fm.targetCount++;
		}

				
					if(File.Exists(workingPath1))
					{
						//for debugging iOS
						Debug.Log("12. lm174, target1 working file exists");
						//creates new target game object
						//GameObject imageTarget1 = new GameObject(obj.Key);
						//target1 = imageTarget1;
						GameObject imageTarget1 = itm.target1;
						imageTarget1.SetActive(true);
						imageTarget1.tag = "target1";
						var behaviour1 = imageTarget1.AddComponent<DynamicImageTagetBehaviour>();
						behaviour1.whichTargetAmI = 1;
						behaviour1.Name = "target1";
						behaviour1.Path = workingPath1;
						behaviour1.Storage = StorageType.Absolute;
						//binds tracking behaviour to target behavior script (required)
						behaviour1.Bind(itm.tracker1);
						//keeps track of name of target and behavior
						//imageTargetDic.Add(obj.Key, behaviour1);
						//set the target status array to reflect that this target has been created

					}
					if(File.Exists(workingPath2))
					{
						//for debugging iOS
						Debug.Log("13. lm196, target2 working file exists");
						//creates new target game object
						//GameObject imageTarget1 = new GameObject(obj.Key);
						//target1 = imageTarget1;
						GameObject imageTarget2 = itm.target2;
						imageTarget2.SetActive(true);
						imageTarget2.tag = "target2";
						var behaviour2 = imageTarget2.AddComponent<DynamicImageTagetBehaviour>();
						behaviour2.whichTargetAmI = 2;
						behaviour2.Name = "target1";
						behaviour2.Path = workingPath2;
						behaviour2.Storage = StorageType.Absolute;
						//binds tracking behaviour to target behavior script (required)
						behaviour2.Bind(itm.tracker2);
						//keeps track of name of target and behavior
						//imageTargetDic.Add(obj.Key, behaviour1);
						//set the target status array to reflect that this target has been created

					}
					if(File.Exists(workingPath3))
					{
						//for debugging iOS
						Debug.Log("14. lm218, target3 working file exists");
						//creates new target game object
						//GameObject imageTarget3 = new GameObject(obj.Key);
						//target3 = imageTarget3;
						GameObject imageTarget3 = itm.target3;
						imageTarget3.SetActive(true);
						imageTarget3.tag = "target3";
						var behaviour3 = imageTarget3.AddComponent<DynamicImageTagetBehaviour>();
						behaviour3.whichTargetAmI = 3;
						behaviour3.Name = "target3";
						behaviour3.Path = workingPath3;
						behaviour3.Storage = StorageType.Absolute;
						//binds tracking behaviour to target behavior script (required)
						behaviour3.Bind(itm.tracker3);
						//keeps track of name of target and behavior
						//imageTargetDic.Add(obj.Key, behaviour3);
						//set the target status array to reflect that this target has been created
					}
					if(File.Exists(workingPath4))
					{
						//for debugging iOS
						Debug.Log("15. lm239, target4 working file exists");
						//creates new target game object
						//GameObject imageTarget4 = new GameObject(obj.Key);
						//target4 = imageTarget4;
						GameObject imageTarget4 = itm.target4;
						imageTarget4.SetActive(true);
						imageTarget4.tag = "target4";
						var behaviour4 = imageTarget4.AddComponent<DynamicImageTagetBehaviour>();
						behaviour4.whichTargetAmI = 4;
						behaviour4.Name = "target4";
						behaviour4.Path = workingPath4;
						behaviour4.Storage = StorageType.Absolute;
						//binds tracking behaviour to target behavior script (required)
						behaviour4.Bind(itm.tracker4);
						//keeps track of name of target and behavior
						//imageTargetDic.Add(obj.Key, behaviour4);
						//set the target status array to reflect that this target has been created
					}
					if(File.Exists(workingPath5))
					{
						//for debugging iOS
						Debug.Log("16. lm260, target5 working file exists");
						//creates new target game object
						//GameObject imageTarget5 = new GameObject(obj.Key);
						//target5 = imageTarget5;
						GameObject imageTarget5 = itm.target5;
						imageTarget5.SetActive(true);
						imageTarget5.tag = "target5";
						var behaviour5 = imageTarget5.AddComponent<DynamicImageTagetBehaviour>();
						behaviour5.whichTargetAmI = 5;
						behaviour5.Name = "target5";
						behaviour5.Path = workingPath5;
						behaviour5.Storage = StorageType.Absolute;
						//binds tracking behaviour to target behavior script (required)
						behaviour5.Bind(itm.tracker5);
						//keeps track of name of target and behavior
						//imageTargetDic.Add(obj.Key, behaviour5);
						//set the target status array to reflect that this target has been created
	
					}
					

		//set preivew images
		if (File.Exists(workingPath1))
		{
			targetPreview1.sprite = IMG2Sprite.LoadNewSprite(workingPath1);
			viewFlowTargetPreview1.sprite = IMG2Sprite.LoadNewSprite(workingPath1);
		}
		if (File.Exists(workingPath2))
		{
			targetPreview2.sprite = IMG2Sprite.LoadNewSprite(workingPath2);
			viewFlowTargetPreview2.sprite = IMG2Sprite.LoadNewSprite(workingPath2);
		}
		if (File.Exists(workingPath3))
		{
			targetPreview3.sprite = IMG2Sprite.LoadNewSprite(workingPath3);
			viewFlowTargetPreview3.sprite = IMG2Sprite.LoadNewSprite(workingPath3);
		}
		if (File.Exists(workingPath4))
		{
			targetPreview4.sprite = IMG2Sprite.LoadNewSprite(workingPath4);
			viewFlowTargetPreview4.sprite = IMG2Sprite.LoadNewSprite(workingPath4);
		}
		if (File.Exists(workingPath5))
	 	{
			targetPreview5.sprite = IMG2Sprite.LoadNewSprite(workingPath5);
			viewFlowTargetPreview5.sprite = IMG2Sprite.LoadNewSprite(workingPath5);
		 }
		//call function to imported all loaded AR objects (pics/videos/models)
		StartCoroutine("ImportLoadedItems");
		targetSetter.SetActive(true);
	}

	private IEnumerator loadJourneyCoverImage() {
		Debug.Log("loadJourneyCoverImage starting");
		Debug.Log(coverPath);
		Debug.Log(File.Exists(coverPath));
		if (File.Exists(coverPath))
		{
			Debug.Log("inside if");
			byte[] coverImageBytes = File.ReadAllBytes(coverPath);
			Texture2D tex = new Texture2D(2000,2000);
			tex.LoadImage(coverImageBytes);
			coverImage.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0));
			previewCoverImage.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0));
			Debug.Log("cover image displayed on UI");
		}
		yield return null;
	}

	//imports all AR objects from save directory
	private IEnumerator ImportLoadedItems() {
		Debug.Log("importLoadedItems started");
		yield return new WaitForSeconds(1);
		for (int i = 0; i < 5; i ++)
		{
			//import model
			if (scd.targetStatus[i] == "model")
			{
				modelIndices[i] = 1;
				switch(i)
				{
					case 0:
						fm.targetStatus[0] = "model";
						ImportModel(scd.modId[0],i);
						yield return new WaitForSeconds(1);
						break;
					case 1:
						fm.targetStatus[1] = "model";
						ImportModel(scd.modId[1],i);
						yield return new WaitForSeconds(1);
						break;
					case 2:
						fm.targetStatus[2] = "model";
						ImportModel(scd.modId[2],i);
						yield return new WaitForSeconds(1);
						break;
					case 3:
						fm.targetStatus[3] = "model";
						ImportModel(scd.modId[3],i);
						yield return new WaitForSeconds(1);
						break;
					case 4:
						fm.targetStatus[4] = "model";
						ImportModel(scd.modId[4],i);
						break;
				}
			}
			//import video
			else if (scd.targetStatus[i] == "video")
			{
				
				videoIndices[i] = 1;
				switch(i)
				{
					case 0:
						fm.targetStatus[0] = "video";
						itm.target1.SetActive(true);
						tom.videoPlayers[0].SetActive(true);
						tom.videoPlayers[0].GetComponent<SimplePlayback>().PlayYoutubeVideo(scd.vId[i]);
						StartCoroutine(setLoadedVideoThumb(i, scd.vId[i]));
						break;
					case 1:
						fm.targetStatus[1] = "video";
						itm.target2.SetActive(true);
						tom.videoPlayers[1].SetActive(true);
						tom.videoPlayers[1].GetComponent<SimplePlayback>().PlayYoutubeVideo(scd.vId[i]);
						StartCoroutine(setLoadedVideoThumb(i, scd.vId[i]));
						break;
					case 2:
						fm.targetStatus[2] = "video";
						itm.target3.SetActive(true);
						tom.videoPlayers[2].SetActive(true);
						tom.videoPlayers[2].GetComponent<SimplePlayback>().PlayYoutubeVideo(scd.vId[i]);
						StartCoroutine(setLoadedVideoThumb(i, scd.vId[i]));
						break;
					case 3:
						fm.targetStatus[3] = "video";
						itm.target4.SetActive(true);
						tom.videoPlayers[3].SetActive(true);
						tom.videoPlayers[3].GetComponent<SimplePlayback>().PlayYoutubeVideo(scd.vId[i]);
						StartCoroutine(setLoadedVideoThumb(i, scd.vId[i]));
						break;
					case 4:
						fm.targetStatus[4] = "video";
						itm.target5.SetActive(true);
						tom.videoPlayers[4].SetActive(true);
						tom.videoPlayers[4].GetComponent<SimplePlayback>().PlayYoutubeVideo(scd.vId[i]);
						StartCoroutine(setLoadedVideoThumb(i, scd.vId[i]));
						break;
				}
				
			}
				else if (scd.targetStatus[i] == "image")
			{
				StartCoroutine(setImage(i));
				//StartCoroutine(setLoadedImageThumb(i));
			}
		}
		loadingPanel.SetActive(false);
	}





	//helper function to set image to AR target
	public IEnumerator setImage(int index)
	{
		//based on which target (1-5) to be set
		switch(index)
				{
					case 0:
						//sets target object image
						fm.targetStatus[0] = "image";
						byte[] linkedImageBytes1 = File.ReadAllBytes(linkedPath1);
						Texture2D tex1 = new Texture2D(2000,2000);
						tex1.LoadImage(linkedImageBytes1);
						pm.image1.GetComponent<Renderer>().material.mainTexture = tex1;
						//sets thumbnail image
						apdm.targetObjectThumbs[0].sprite = Sprite.Create(tex1, new Rect(0, 0, tex1.width, tex1.height), new Vector2(0, 0));
						break;
					case 1:
						fm.targetStatus[1] = "image";
						byte[] linkedImageBytes2 = File.ReadAllBytes(linkedPath2);
						Texture2D tex2 = new Texture2D(2000,2000);
						tex2.LoadImage(linkedImageBytes2);
						pm.image2.GetComponent<Renderer>().material.mainTexture = tex2;
						apdm.targetObjectThumbs[1].sprite = Sprite.Create(tex2, new Rect(0, 0, tex2.width, tex2.height), new Vector2(0, 0));
						break;
					case 2:
						fm.targetStatus[2] = "image";
						byte[] linkedImageBytes3 = File.ReadAllBytes(linkedPath3);
						Texture2D tex3 = new Texture2D(2000,2000);
						tex3.LoadImage(linkedImageBytes3);
						pm.image3.GetComponent<Renderer>().material.mainTexture = tex3;
						apdm.targetObjectThumbs[2].sprite = Sprite.Create(tex3, new Rect(0, 0, tex3.width, tex3.height), new Vector2(0, 0));
						break;
					case 3:
						fm.targetStatus[3] = "image";
						byte[] linkedImageBytes4 = File.ReadAllBytes(linkedPath4);
						Texture2D tex4 = new Texture2D(2000,2000);
						tex4.LoadImage(linkedImageBytes4);
						pm.image4.GetComponent<Renderer>().material.mainTexture = tex4;
						apdm.targetObjectThumbs[3].sprite = Sprite.Create(tex4, new Rect(0, 0, tex4.width, tex4.height), new Vector2(0, 0));
						break;
					case 4:
						fm.targetStatus[4] = "image";
						byte[] linkedImageBytes5 = File.ReadAllBytes(linkedPath5);
						Texture2D tex5 = new Texture2D(2000,2000);
						tex5.LoadImage(linkedImageBytes5);
						pm.image5.GetComponent<Renderer>().material.mainTexture = tex5;
						apdm.targetObjectThumbs[4].sprite = Sprite.Create(tex5, new Rect(0, 0, tex5.width, tex5.height), new Vector2(0, 0));
						break;
				}
				yield return null;
	}

	//set of 3 helper functions to import model
	private void ImportModel(string modelId, int whichIndex) {
		string assetString = "assets/" + modelId;
		PolyApi.GetAsset(modelId, GetAssetCallback);
		//used because callback has preset number of parameters
		globalModelArrayIndex = whichIndex;
	}
	void GetAssetCallback(PolyStatusOr<PolyAsset> result) {
  	if (!result.Ok) 
		{
			Debug.Log("There was an error importing the loaded asset");
			return;
  	}
		List<PolyAsset> assets = new List<PolyAsset>();
		assets.Add(result.Value);
		allAssets[globalModelArrayIndex] = result.Value;
		tom.attribs.Add(PolyApi.GenerateAttributions(includeStatic: true, runtimeAssets: assets));

		PolyImportOptions options = PolyImportOptions.Default();
		// We want to rescale the imported meshes to a specific size.
		options.rescalingMode = PolyImportOptions.RescalingMode.FIT;
		// The specific size we want assets rescaled to (fit in a 1x1x1 box):
		options.desiredSize = 1.0f;
		// We want the imported assets to be recentered such that their centroid coincides with the origin:
		options.recenter = true;
		PolyApi.Import(result.Value, options, GetModelCallback);
	}
	void GetModelCallback(PolyAsset asset, PolyStatusOr<PolyImportResult> result) {
  if (!result.Ok) {
    Debug.Log("There was an error importing the loaded model");
    return;
  }
	for (int j = 0; j < 5 ;j++)
	{
		if (modelIndices[j] == 1)
		{
			//assigns the imported model to GameObject scripts and sets the models parent as the correct indexed target
			switch(j)
			{
				case 0:
					GameObject thisModel1 = result.Value.gameObject;
					Transform transform1 = result.Value.gameObject.GetComponent(typeof(Transform)) as Transform;
					transform1.position = new Vector3(0.0f, 0.75f, 0f);
					transform1.tag = "model1";
					transform1.parent = itm.target1.transform;
					fm.targetStatus[0] = "model";
					tom.models[0] = thisModel1;
					tom.modelIds[0] = scd.modId[0];
					break;
				case 1:
					GameObject thisModel2 = result.Value.gameObject;
					Transform transform2 = result.Value.gameObject.GetComponent(typeof(Transform)) as Transform;
					transform2.position = new Vector3(0.0f, 0.75f, 0f);
					transform2.tag = "model2";
					transform2.parent = itm.target2.transform;
					fm.targetStatus[1] = "model";
					tom.models[1] = thisModel2;
					tom.modelIds[1] = scd.modId[1];
					break;
				case 2:
					GameObject thisModel3 = result.Value.gameObject;
					Transform transform3 = result.Value.gameObject.GetComponent(typeof(Transform)) as Transform;
					transform3.position = new Vector3(0.0f, 0.75f, 0f);
					transform3.tag = "model3";
					transform3.parent = itm.target3.transform;
					fm.targetStatus[2] = "model";
					tom.models[2] = thisModel3;
					tom.modelIds[2] = scd.modId[2];
					break;
				case 3:
					GameObject thisModel4 = result.Value.gameObject;
					Transform transform4 = result.Value.gameObject.GetComponent(typeof(Transform)) as Transform;
					transform4.position = new Vector3(0.0f, 0.75f, 0f);
					transform4.tag = "model4";
					transform4.parent = itm.target4.transform;
					fm.targetStatus[3] = "model";
					tom.models[3] = thisModel4;
					tom.modelIds[3] = scd.modId[3];
					break;
				case 4:
					GameObject thisModel5 = result.Value.gameObject;
					Transform transform5 = result.Value.gameObject.GetComponent(typeof(Transform)) as Transform;
					transform5.position = new Vector3(0.0f, 0.75f, 0f);
					transform5.tag = "model5";
					transform5.parent = itm.target5.transform;
					fm.targetStatus[4] = "model";
					tom.models[4] = thisModel5;
					tom.modelIds[4] = scd.modId[4];
					break;
			}
			modelIndices[j] = 0;
			setLoadedModelThumb(j);
			return;
		}
	}
	}

	//this function has been incorporated into SetImage
/* 
	//helper function to load image to corrent thumbnail (called by loadFile)
	private IEnumerator setLoadedImageThumb(int whichIndex)
	{
		switch (whichIndex)
		{
			case 0:
				byte[] coverImageBytes = File.ReadAllBytes(coverPath);
				Texture2D tex = new Texture2D(2000,2000);
				tex.LoadImage(coverImageBytes);
				coverImage.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0));
				using (WWW imageThumbRequest1 = new WWW(scd.imageUrl[0]))
				{
					yield return imageThumbRequest1;
					linkedThumb1.sprite = Sprite.Create(imageThumbRequest1.texture, new Rect(0, 0, imageThumbRequest1.texture.width, imageThumbRequest1.texture.height), new Vector2(0, 0));
				}
				break;
			case 1:
				using (WWW imageThumbRequest2 = new WWW(scd.imageUrl[1]))
				{
					yield return imageThumbRequest2;
					linkedThumb2.sprite = Sprite.Create(imageThumbRequest2.texture, new Rect(0, 0, imageThumbRequest2.texture.width, imageThumbRequest2.texture.height), new Vector2(0, 0));
				}
				break;
			case 2:
				using (WWW imageThumbRequest3 = new WWW(scd.imageUrl[2]))
				{
					yield return imageThumbRequest3;
					linkedThumb3.sprite = Sprite.Create(imageThumbRequest3.texture, new Rect(0, 0, imageThumbRequest3.texture.width, imageThumbRequest3.texture.height), new Vector2(0, 0));
				}
				break;
			case 3:
				using (WWW imageThumbRequest4 = new WWW(scd.imageUrl[3]))
				{
					yield return imageThumbRequest4;
					linkedThumb4.sprite = Sprite.Create(imageThumbRequest4.texture, new Rect(0, 0, imageThumbRequest4.texture.width, imageThumbRequest4.texture.height), new Vector2(0, 0));
				}
				break;
			case 4:
				using (WWW imageThumbRequest5 = new WWW(scd.imageUrl[4]))
				{
					yield return imageThumbRequest5;
					linkedThumb5.sprite = Sprite.Create(imageThumbRequest5.texture, new Rect(0, 0, imageThumbRequest5.texture.width, imageThumbRequest5.texture.height), new Vector2(0, 0));
				}
				break;
		}
	}
	*/

	private void setLoadedModelThumb(int whichIndex)
	{
		Debug.Log("1. Starting loading model thumbnails...");
		Debug.Log("2. allAsset array: "+allAssets);
		Debug.Log("3. index of asset to get: " +whichIndex);
		//Debug.Log("4. asset at index in allAsset array: "+allAssets[whichIndex]);
		
		globalModelIndexTracker = whichIndex;
		PolyApi.FetchThumbnail(allAssets[whichIndex], MyCallback);
	}

	void MyCallback(PolyAsset asset, PolyStatus status) {
		if (!status.ok) {
			return;
		}
		switch(globalModelIndexTracker)
		{
			case 0:
				apdm.targetObjectThumbs[0].sprite = Sprite.Create(asset.thumbnailTexture, new Rect(0, 0, asset.thumbnailTexture.width, asset.thumbnailTexture.height), new Vector2(0.5f, 0.5f), 100);
				break;
			case 1:
				apdm.targetObjectThumbs[1].sprite = Sprite.Create(asset.thumbnailTexture, new Rect(0, 0, asset.thumbnailTexture.width, asset.thumbnailTexture.height), new Vector2(0.5f, 0.5f), 100);
				break;
			case 2:
				apdm.targetObjectThumbs[2].sprite = Sprite.Create(asset.thumbnailTexture, new Rect(0, 0, asset.thumbnailTexture.width, asset.thumbnailTexture.height), new Vector2(0.5f, 0.5f), 100);
				break;
			case 3:
				apdm.targetObjectThumbs[3].sprite = Sprite.Create(asset.thumbnailTexture, new Rect(0, 0, asset.thumbnailTexture.width, asset.thumbnailTexture.height), new Vector2(0.5f, 0.5f), 100);
				break;
			case 4:
				apdm.targetObjectThumbs[4].sprite = Sprite.Create(asset.thumbnailTexture, new Rect(0, 0, asset.thumbnailTexture.width, asset.thumbnailTexture.height), new Vector2(0.5f, 0.5f), 100);
				break;
		}
	}


	private IEnumerator setLoadedVideoThumb(int whichIndex, string videoId)
	{
		Debug.Log("in the video thumbnail loader");
		string thumbnailUrl = "https://img.youtube.com/vi/"+videoId+"/default.jpg";
		switch (whichIndex)
		{
			case 0:
				using (WWW videoThumbRequest1 = new WWW(thumbnailUrl))
				{
					yield return videoThumbRequest1;
					apdm.targetObjectThumbs[0].sprite = Sprite.Create(videoThumbRequest1.texture, new Rect(0, 0, videoThumbRequest1.texture.width, videoThumbRequest1.texture.height), new Vector2(0, 0));
				}
				break;
			case 1:
				using (WWW videoThumbRequest2 = new WWW(thumbnailUrl))
				{
					yield return videoThumbRequest2;
					apdm.targetObjectThumbs[1].sprite = Sprite.Create(videoThumbRequest2.texture, new Rect(0, 0, videoThumbRequest2.texture.width, videoThumbRequest2.texture.height), new Vector2(0, 0));
				}
				break;
			case 2:
				using (WWW videoThumbRequest3 = new WWW(thumbnailUrl))
				{
					yield return videoThumbRequest3;
					apdm.targetObjectThumbs[2].sprite = Sprite.Create(videoThumbRequest3.texture, new Rect(0, 0, videoThumbRequest3.texture.width, videoThumbRequest3.texture.height), new Vector2(0, 0));
				}
				break;
			case 3:
				using (WWW videoThumbRequest4 = new WWW(thumbnailUrl))
				{
					yield return videoThumbRequest4;
					apdm.targetObjectThumbs[3].sprite = Sprite.Create(videoThumbRequest4.texture, new Rect(0, 0, videoThumbRequest4.texture.width, videoThumbRequest4.texture.height), new Vector2(0, 0));
				}
				break;
			case 4:
				using (WWW videoThumbRequest5 = new WWW(thumbnailUrl))
				{
					yield return videoThumbRequest5;
					apdm.targetObjectThumbs[4].sprite = Sprite.Create(videoThumbRequest5.texture, new Rect(0, 0, videoThumbRequest5.texture.width, videoThumbRequest5.texture.height), new Vector2(0, 0));
				}
				break;
		}
	}


}
