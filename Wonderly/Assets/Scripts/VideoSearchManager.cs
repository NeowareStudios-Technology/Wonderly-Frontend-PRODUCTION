/******************************************************
*Project: Wonderly
*Modified by: David Lee Ramirez
*Date: 12/28/18
*Description: Handles starting Youtube video search 
            (YoutubeAPIManager handles rest of search)
            and instantiation of video thumbnails. Modified
            script from Lightshaft Youtube plugin.
 ******************************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoSearchManager : MonoBehaviour {
    //script references
    public YoutubeAPIManager youtubeapi;
    public FilesManager fm;
    //input field for searching youtube videos
    public InputField searchField;
    //references to "simple playback" gameobjects
    public GameObject vidReference1;
    public GameObject vidReference2;
    public GameObject vidReference3;
    public GameObject vidReference4;
    public GameObject vidReference5;
    //references to target gameobjects
    public GameObject targetReference1;
    public GameObject targetReference2;
    public GameObject targetReference3;
    public GameObject targetReference4;
    public GameObject targetReference5;
    //urls to youtube video for each thumbnail
    public List<string> thumbUrls = new List<string>();
    //max number of thumbnails
    public int maxThumbResults = 50;
    //video thumbnail prefab
    public GameObject videoThumbPrefab;
    //parent GameObject of thumbnails
    public GameObject thumbNailParentContent;
    //below is for edit flow from summary screen (FLOW2)
    public GameObject thumbNailParentContent2;
    public InputField searchField2;
    //for accessing PanelController
    public GameObject mainCanvas;
    //for switching to these screens
    public Animator viewLibraryContentPanel;
    public Animator viewLibraryContentPanel2;
    //reference to script holder game object
    public GameObject localScriptHolder;
    //list of thumbnail GameObjects (up to 50)
    public GameObject[] videoThumbList;
	

    //initializes video thumb gameobject list
    void Awake(){
        videoThumbList = new GameObject[maxThumbResults];
    }


    //initiates youtube search
	public void Search()
    {
        fm.unloadUnused();
        thumbUrls.Clear();
     
        //do nothing if no targets created yet or if indexed target not created yet
        if (fm.targetStatus[fm.currentTarget-1] == "none")
            return;

        YoutubeAPIManager.YoutubeSearchOrderFilter mainFilter = YoutubeAPIManager.YoutubeSearchOrderFilter.none;
       
        youtubeapi.Search(searchField.text, maxThumbResults, mainFilter, YoutubeAPIManager.YoutubeSafeSearchFilter.none, OnSearchDone);
    }


    //unused
    public void SearchByLocation(string location)
    {
        YoutubeAPIManager.YoutubeSearchOrderFilter mainFilter = YoutubeAPIManager.YoutubeSearchOrderFilter.none;
    
        string[] splited = location.Split(',');
        float latitude = float.Parse(splited[0]);
        float longitude = float.Parse(splited[1]);
        int locationRadius = 10;
        youtubeapi.SearchByLocation(searchField.text, maxThumbResults, locationRadius, latitude, longitude, mainFilter, YoutubeAPIManager.YoutubeSafeSearchFilter.none, OnSearchDone);
    }


    //callback for youtube search
    void OnSearchDone(YoutubeData[] results)
    {
        //videoUIResult.SetActive(true);
        LoadVideosOnUI(results);
    }


    //instantiates youtube thumbnails
    void LoadVideosOnUI(YoutubeData[] videoList)
    {
        for(int i = 0; i < videoList.Length; i++){
            int TempIterator = i;
            string thumbNailName = "videoThumbnail" + i;

            GameObject newThumbnail = Instantiate(videoThumbPrefab);
            newThumbnail.name = thumbNailName;
            newThumbnail.transform.SetParent(thumbNailParentContent.GetComponent<Transform>());
            videoThumbList[i] = newThumbnail;
            newThumbnail.GetComponent<RectTransform>().localScale = new Vector3(1.0f,1.0f,1.0f);
            newThumbnail.GetComponent<YoutubeVideoUi>().videoId = videoList[i].id;
            newThumbnail.GetComponent<YoutubeVideoUi>().thumbUrl = videoList[i].snippet.thumbnails.defaultThumbnail.url;
            thumbUrls.Add(videoList[i].snippet.thumbnails.defaultThumbnail.url);
            newThumbnail.GetComponent<YoutubeVideoUi>().LoadThumbnail();
            newThumbnail.GetComponent<Button>().onClick.AddListener(delegate {newThumbnail.GetComponent<YoutubeVideoUi>().PlayYoutubeVideo();});

             //loading panel is turned off at the end of this thread after DeleteThumbnails()
            newThumbnail.GetComponent<Button>().onClick.AddListener(delegate {localScriptHolder.GetComponent<ArPairDisplayManager>().setYoutubeThumbnailArPair(newThumbnail);});
            
            newThumbnail.GetComponent<Button>().onClick.AddListener(delegate {this.GetComponent<YoutubeAPIManager>().SetVideoInfo(TempIterator);});
            newThumbnail.GetComponent<Button>().onClick.AddListener(delegate {mainCanvas.GetComponent<PanelController>().OpenPanel(viewLibraryContentPanel);});
            //loading Panel deactivated in RequestResolver.DownloadUrl()
            newThumbnail.GetComponent<Button>().onClick.AddListener(delegate {localScriptHolder.GetComponent<UiManager>().SetLoadingPanelActive(true);});
        }    
    }


    //deletes instantiated thumbnails and clears unused memory
    public void DeleteThumbnails(){
		foreach (Transform child in thumbNailParentContent.transform) {
     			GameObject.Destroy(child.gameObject);
        }
        StartCoroutine(TurnOffLoadingPanel());
        Resources.UnloadUnusedAssets();
	}


    //deactivates loading panel
    private IEnumerator TurnOffLoadingPanel(){
        yield return new WaitForSeconds(0.2f);
        localScriptHolder.GetComponent<UiManager>().SetLoadingPanelActive(false);
    }


    //clears search field
    public void ClearSearchField(){
        searchField.text = "";
    }

//above is for searching for videos in normal flow for creating Journey
/*****************************************************************************************************************************************************************************************/
//below is for searching for videos in edit flow from Journey experience (FLOW2)


//initiates youtube search EDIT FLOW
public void Search2()
    {
        fm.unloadUnused();
        thumbUrls.Clear();
        //do nothing if no targets created yet or if indexed target not created yet
        if (fm.targetStatus[fm.currentTarget-1] == "none")
            return;

        YoutubeAPIManager.YoutubeSearchOrderFilter mainFilter = YoutubeAPIManager.YoutubeSearchOrderFilter.none;
       
        youtubeapi.Search(searchField2.text, maxThumbResults, mainFilter, YoutubeAPIManager.YoutubeSafeSearchFilter.none, OnSearchDone2);
    }


    //callback for youtube search EDIT FLOW
    void OnSearchDone2(YoutubeData[] results)
    {
        //videoUIResult.SetActive(true);
        LoadVideosOnUI2(results);
    }


    //instantiates youtube thumbnails EDIT FLOW
    void LoadVideosOnUI2(YoutubeData[] videoList)
    {
        for(int i = 0; i < videoList.Length; i++){
            int TempIterator = i;
            string thumbNailName = "videoThumbnail" + i;

            GameObject newThumbnail = Instantiate(videoThumbPrefab);
            newThumbnail.name = thumbNailName;
            newThumbnail.transform.SetParent(thumbNailParentContent2.GetComponent<Transform>());
            videoThumbList[i] = newThumbnail;
            newThumbnail.GetComponent<RectTransform>().localScale = new Vector3(1.0f,1.0f,1.0f);
            newThumbnail.GetComponent<YoutubeVideoUi>().videoId = videoList[i].id;
            newThumbnail.GetComponent<YoutubeVideoUi>().thumbUrl = videoList[i].snippet.thumbnails.defaultThumbnail.url;
            thumbUrls.Add(videoList[i].snippet.thumbnails.defaultThumbnail.url);
            newThumbnail.GetComponent<YoutubeVideoUi>().LoadThumbnail();
            newThumbnail.GetComponent<Button>().onClick.AddListener(delegate {newThumbnail.GetComponent<YoutubeVideoUi>().PlayYoutubeVideo();});
            newThumbnail.GetComponent<Button>().onClick.AddListener(delegate {localScriptHolder.GetComponent<ArPairDisplayManager>().setYoutubeThumbnailArPair(newThumbnail);});
            newThumbnail.GetComponent<Button>().onClick.AddListener(delegate {this.GetComponent<YoutubeAPIManager>().SetVideoInfo(TempIterator);});
            newThumbnail.GetComponent<Button>().onClick.AddListener(delegate {mainCanvas.GetComponent<PanelController>().OpenPanel(viewLibraryContentPanel2);});
        } 
    }


    //deletes video thumbnails and release memory EDIT FLOW
    public void DeleteThumbnails2(){
		foreach (Transform child in thumbNailParentContent2.transform) {
     			GameObject.Destroy(child.gameObject);
        }
        Resources.UnloadUnusedAssets();
	}


    //clears search inut field EDIT FLOW
    public void ClearSearchField2(){
        searchField2.text = "";
    }

}
