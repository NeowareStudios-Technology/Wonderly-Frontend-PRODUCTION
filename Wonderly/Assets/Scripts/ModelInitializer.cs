/******************************************************
*Project: Wonderly
*Created by: David Lee Ramirez
*Date: 12/28/18
*Description: Used for searching for 3d models in Google
              Poly repo and producing thumbnails.
*Copyright 2018 LeapWithAlice,LLC. All rights reserved
 ******************************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PolyToolkit;
using Sample;

public class ModelInitializer : MonoBehaviour {
    public InputField keyword;
    //public RawImage thumbPrefab;
    //public GameObject content;
    public Image thumbImage1;
    public Image thumbImage2;
    public Image thumbImage3;
    public Image thumbImage4;
    public Image thumbImage5;
    public Image thumbImage6;
    public Image thumbImage7;
    public Image thumbImage8;
    public Image thumbImage9;
    public Image thumbImage10;
    public Image thumbImage11;
    public Image thumbImage12;
    public Image thumbImage13;
    public Image thumbImage14;
    public Image thumbImage15;
    public Image thumbImage16;
    public Image thumbImage17;
    public Image thumbImage18;

    public PolyAssetHolderClass thumbAsset1;
    public PolyAssetHolderClass thumbAsset2;
    public PolyAssetHolderClass thumbAsset3;
    public PolyAssetHolderClass thumbAsset4;
    public PolyAssetHolderClass thumbAsset5;
    public PolyAssetHolderClass thumbAsset6;
    public PolyAssetHolderClass thumbAsset7;
    public PolyAssetHolderClass thumbAsset8;
    public PolyAssetHolderClass thumbAsset9;
    public PolyAssetHolderClass thumbAsset10;
    public PolyAssetHolderClass thumbAsset11;
    public PolyAssetHolderClass thumbAsset12;
    public PolyAssetHolderClass thumbAsset13;
    public PolyAssetHolderClass thumbAsset14;
    public PolyAssetHolderClass thumbAsset15;
    public PolyAssetHolderClass thumbAsset16;
    public PolyAssetHolderClass thumbAsset17;
    public PolyAssetHolderClass thumbAsset18;

    public int maxThumbResults = 50;
    public GameObject modelThumbPrefab;
    public GameObject thumbNailParentContent;
    //used by second edit AR object screen marked SINGLE
    public GameObject thumbNailParentContent2;
    public InputField keyword2;
    public Animator viewLibraryContentPanel2;

    public GameObject mainCanvas;
    public Animator viewLibraryContentPanel;
    public GameObject localScriptHolder;
    public GameObject[] thumbnailResults;


    //need to implement attributes
    //public Text modelAttributes;
    //public int assetIndex = 0;
    //public List<PolyAsset> assetsInUse;
    public ModelRenderer mr;
    public FilesManager fm;
    public int thumbnailCount = 0;
    private RawImage thumb;

    void Awake(){
        thumbnailResults = new GameObject[maxThumbResults];
    }

    //Flow for get thumbnails: GetThumbnails -> ListAssetsCallback -> myThumbnailCallback (this one makes the thumbnails)
    public void GetThumbnails()
    {
        PolyListAssetsRequest req = new PolyListAssetsRequest();
        req.keywords = keyword.text;
        req.curated = true;
        req.orderBy = PolyOrderBy.BEST;
        req.maxComplexity = PolyMaxComplexityFilter.UNSPECIFIED;
        PolyApi.ListAssets(req, ListAssetsCallback);
        thumbnailCount = 0;
    }

    // Callback invoked when the featured assets results are returned.
    private void ListAssetsCallback(PolyStatusOr<PolyListAssetsResult> result) {
        
        if (!result.Ok) {
            Debug.LogError("Failed to get featured assets. :( Reason: " + result.Status);
            return;
        }
 



        fm.unloadUnused();
        ClearSearchText();

        for (int i = 0; i < Mathf.Min(maxThumbResults, result.Value.assets.Count); i++) { 
            Debug.Log(i+" "+ result.Value.assets[i]);
            PolyApi.FetchThumbnail(result.Value.assets[i], MyThumbnailCallback);
        }
    }
    public void DeleteThumbnails(){
       
			foreach (Transform child in thumbNailParentContent.transform) {
				GameObject.Destroy(child.gameObject);
			}
            Debug.Log("done destroying");
            TurnOffLoadingPanel();
            Resources.UnloadUnusedAssets();
	
    }
    public void TurnOffLoadingPanel(){
        
		localScriptHolder.GetComponent<UiManager>().SetLoadingPanelActive(false);
    }
    public void ClearSearchText(){
			keyword.text = "";
		}
    void MyThumbnailCallback(PolyAsset asset, PolyStatus status)
    {
        Debug.Log("in callback");
        if (!status.ok)
        {
            Debug.Log("Loading thumbnails fail");
            // Handle error;
            return;
        }
        // Display the asset.thumbnailTexture.
        Debug.Log("Loading thumbnails");
        //thumb = Instantiate(thumbPrefab,content.transform);
        Rect rec = new Rect(0, 0, asset.thumbnailTexture.width, asset.thumbnailTexture.height);

        string thumbNailName = "modelThumbnail" + thumbnailCount;
        GameObject newThumbnail = Instantiate(modelThumbPrefab);
        newThumbnail.name = thumbNailName;

        newThumbnail.GetComponent<Image>().sprite = Sprite.Create(asset.thumbnailTexture, rec, new Vector2(0.5f, 0.5f), 100);

        newThumbnail.transform.SetParent(thumbNailParentContent.GetComponent<Transform>());
        thumbnailResults[thumbnailCount] = newThumbnail;
        
        newThumbnail.GetComponent<RectTransform>().localScale = new Vector3(1.0f,1.0f,1.0f);
        newThumbnail.GetComponent<Button>().onClick.AddListener(delegate {localScriptHolder.GetComponent<UiManager>().SetLoadingPanelActive(true);});
        newThumbnail.GetComponent<Button>().onClick.AddListener(delegate {mainCanvas.GetComponent<PanelController>().OpenPanel(viewLibraryContentPanel);});
        newThumbnail.GetComponent<Button>().onClick.AddListener(delegate {mr.renderModel(newThumbnail);});

        //loading panel is turned off at the end of this thread after DeleteThumbnails()
        newThumbnail.GetComponent<Button>().onClick.AddListener(delegate {localScriptHolder.GetComponent<ArPairDisplayManager>().setModelThumbnailArPair(newThumbnail);});
        
       
        newThumbnail.GetComponent<PolyAssetHolderClass>().heldAsset = asset;
    
        thumbnailCount++;

    }


//Above: called by initial set AR object screen
/***************************************************************************************************************************************************************/
//Below: called by second set AR screen activated when editing AR object from summary screen

    //Flow for get thumbnails: GetThumbnails2 -> ListAssetsCallback2-> myThumbnailCallback2 (this one makes the thumbnails)
    public void GetThumbnails2()
    {
        PolyListAssetsRequest req = new PolyListAssetsRequest();
        req.keywords = keyword2.text;
        req.curated = true;
        req.orderBy = PolyOrderBy.BEST;
        req.maxComplexity = PolyMaxComplexityFilter.UNSPECIFIED;
        PolyApi.ListAssets(req, ListAssetsCallback2);
        thumbnailCount = 0;
    }

    // Callback invoked when the featured assets results are returned.
    private void ListAssetsCallback2(PolyStatusOr<PolyListAssetsResult> result) {
        
        if (!result.Ok) {
            Debug.LogError("Failed to get featured assets. :( Reason: " + result.Status);
            return;
        }
 



        fm.unloadUnused();
        ClearSearchText2();

        for (int i = 0; i < Mathf.Min(maxThumbResults, result.Value.assets.Count); i++) { 
            Debug.Log(i+" "+ result.Value.assets[i]);
            PolyApi.FetchThumbnail(result.Value.assets[i], MyThumbnailCallback2);
        }
    }
    public void DeleteThumbnails2(){
       
			foreach (Transform child in thumbNailParentContent2.transform) {
				GameObject.Destroy(child.gameObject);
			}
            Resources.UnloadUnusedAssets();
    }
    public void ClearSearchText2(){
			keyword2.text = "";
		}
    void MyThumbnailCallback2(PolyAsset asset, PolyStatus status)
    {
        Debug.Log("in callback");
        if (!status.ok)
        {
            Debug.Log("Loading thumbnails fail");
            // Handle error;
            return;
        }
        // Display the asset.thumbnailTexture.
        Debug.Log("Loading thumbnails");
        //thumb = Instantiate(thumbPrefab,content.transform);
        Rect rec = new Rect(0, 0, asset.thumbnailTexture.width, asset.thumbnailTexture.height);

        string thumbNailName = "modelThumbnail" + thumbnailCount;
        GameObject newThumbnail = Instantiate(modelThumbPrefab);
        newThumbnail.name = thumbNailName;

        newThumbnail.GetComponent<Image>().sprite = Sprite.Create(asset.thumbnailTexture, rec, new Vector2(0.5f, 0.5f), 100);

        newThumbnail.transform.SetParent(thumbNailParentContent2.GetComponent<Transform>());
        thumbnailResults[thumbnailCount] = newThumbnail;
        
        newThumbnail.GetComponent<RectTransform>().localScale = new Vector3(1.0f,1.0f,1.0f);
        newThumbnail.GetComponent<Button>().onClick.AddListener(delegate {mainCanvas.GetComponent<PanelController>().OpenPanel(viewLibraryContentPanel2);});
        newThumbnail.GetComponent<Button>().onClick.AddListener(delegate {mr.renderModel(newThumbnail);});
        newThumbnail.GetComponent<Button>().onClick.AddListener(delegate {localScriptHolder.GetComponent<ArPairDisplayManager>().setModelThumbnailArPair(newThumbnail);});
        newThumbnail.GetComponent<PolyAssetHolderClass>().heldAsset = asset;
    
        thumbnailCount++;

    }
}