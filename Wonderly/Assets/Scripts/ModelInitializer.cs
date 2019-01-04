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

public class ModelInitializer : MonoBehaviour {
    //script references
    public ModelRenderer mr;
    public FilesManager fm;
    //keyword for Poly model search
    public InputField keyword;
    //max number of thumbnail objects to spawn 
    public int maxThumbResults = 50;
    //model thumbnail prefab
    public GameObject modelThumbPrefab;
    //model thumbnail parent GameObject
    public GameObject thumbNailParentContent;
    //variables marked "2" used by second edit AR object screen marked SINGLE
    public GameObject thumbNailParentContent2;
    public InputField keyword2;
    public Animator viewLibraryContentPanel2;
    //used to get "panel controller" object (for changing screens)
    public GameObject mainCanvas;
    //used to switch screens to MyJourneys screen
    public Animator viewLibraryContentPanel;
    public GameObject localScriptHolder;
    public GameObject[] thumbnailResults;
    public int thumbnailCount = 0;
    private RawImage thumb;


    //initialization of thumbnail list (empty)
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
 
        //unload unused memoyry
        fm.unloadUnused();
        ClearSearchText();

        for (int i = 0; i < Mathf.Min(maxThumbResults, result.Value.assets.Count); i++) { 
            Debug.Log(i+" "+ result.Value.assets[i]);
            PolyApi.FetchThumbnail(result.Value.assets[i], MyThumbnailCallback);
        }
    }

    //clear search field for create flow (called by outside script)
    public void ClearSearchText()
    {
        keyword.text = "";
    }


    //create thumbnail for each found poly model under proper parent for create flow
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
        //keeps track of which model gets put on which thumbnail
        thumbnailCount++;
    }


    //Destroys all model thumbnails (and releases memory) for create flow
    public void DeleteThumbnails(){
       
			foreach (Transform child in thumbNailParentContent.transform) {
				GameObject.Destroy(child.gameObject);
			}
            Debug.Log("done destroying");
            //deactivate loading panel
            localScriptHolder.GetComponent<UiManager>().SetLoadingPanelActive(false);
            Resources.UnloadUnusedAssets();
	
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

    //clear search field for edit flow (called by outside script)
    public void ClearSearchText2()
    {
        keyword2.text = "";
    }


    //create thumbnail for each found poly model under proper parent for edit flow
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

        //keeps track of which model gets put on which thumbnail
        thumbnailCount++;
    }


    //Destroys all model thumbnails (and releases memory) for edit flow (marked SINGLE)
    public void DeleteThumbnails2()
    {
       
        foreach (Transform child in thumbNailParentContent2.transform) {
            GameObject.Destroy(child.gameObject);
        }
        Resources.UnloadUnusedAssets();
    }
}