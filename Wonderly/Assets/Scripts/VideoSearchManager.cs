using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sample;

public class VideoSearchManager : MonoBehaviour {
    public YoutubeAPIManager youtubeapi;

    public InputField searchField;
    public YoutubeVideoUi[] videoListUI;
    //public GameObject videoUIResult;
    //public GameObject mainUI;
    public GameObject vidReference1;
    public GameObject vidReference2;
    public GameObject vidReference3;
    public GameObject vidReference4;
    public GameObject vidReference5;
    public GameObject targetReference1;
    public GameObject targetReference2;
    public GameObject targetReference3;
    public GameObject targetReference4;
    public GameObject targetReference5;

    public Text vidTitle1;
    public Text vidTitle2;
    public Text vidTitle3;
    public Text vidTitle4;
    public Text vidTitle5;

    public FilesManager fm;
    public List<string> thumbUrls = new List<string>();

    public int maxThumbResults = 50;
    public GameObject videoThumbPrefab;
    public GameObject thumbNailParentContent;
    //below is for edit flow from summary screen (FLOW2)
    public GameObject thumbNailParentContent2;
    public InputField searchField2;

    public GameObject mainCanvas;
    public Animator viewLibraryContentPanel;
    public GameObject localScriptHolder;
    public GameObject[] videoThumbList;
	
    void Awake(){
        videoThumbList = new GameObject[maxThumbResults];
    }
	public void Search()
    {
        DeleteThumbnails();
        thumbUrls.Clear();
        //turn on target's video player
        //need to implement this in V2 when assets ready
        /*/// switch(fm.currentTarget)
        {
            case 0:
                return;
            case 1:
                vidReference1.SetActive(true);
                break;
            case 2:
                vidReference2.SetActive(true);
                break;
            case 3:
                vidReference3.SetActive(true);
                break;
            case 4:
                vidReference4.SetActive(true);
                break;
            case 5:
                vidReference5.SetActive(true);
                break;
        }
        ///*/
        //do nothing if no targets created yet or if indexed target not created yet
        if (fm.targetStatus[fm.currentTarget-1] == "none")
            return;

        YoutubeAPIManager.YoutubeSearchOrderFilter mainFilter = YoutubeAPIManager.YoutubeSearchOrderFilter.none;
       
        youtubeapi.Search(searchField.text, maxThumbResults, mainFilter, YoutubeAPIManager.YoutubeSafeSearchFilter.none, OnSearchDone);
    }

    public void SearchByLocation(string location)
    {
        YoutubeAPIManager.YoutubeSearchOrderFilter mainFilter = YoutubeAPIManager.YoutubeSearchOrderFilter.none;
    
        string[] splited = location.Split(',');
        float latitude = float.Parse(splited[0]);
        float longitude = float.Parse(splited[1]);
        int locationRadius = 10;
        youtubeapi.SearchByLocation(searchField.text, maxThumbResults, locationRadius, latitude, longitude, mainFilter, YoutubeAPIManager.YoutubeSafeSearchFilter.none, OnSearchDone);
    }

    void OnSearchDone(YoutubeData[] results)
    {
        //videoUIResult.SetActive(true);
        LoadVideosOnUI(results);
    }

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
            newThumbnail.GetComponent<Button>().onClick.AddListener(delegate {localScriptHolder.GetComponent<ArPairDisplayManager>().setYoutubeThumbnailArPair(newThumbnail);});
            newThumbnail.GetComponent<Button>().onClick.AddListener(delegate {this.GetComponent<YoutubeAPIManager>().SetVideoInfo(TempIterator + 1);});
            newThumbnail.GetComponent<Button>().onClick.AddListener(delegate {mainCanvas.GetComponent<PanelController>().OpenPanel(viewLibraryContentPanel);});
       

        }
         
    }


    public void DeleteThumbnails(){
		foreach (Transform child in thumbNailParentContent.transform) {
     			GameObject.Destroy(child.gameObject);
        }
	}
    public void ClearSearchField(){
        searchField.text = "";
    }

//above is for searching for videos in normal flow for creating Journey
/*****************************************************************************************************************************************************************************************/
//below is for searching for videos in edit flow from Journey experience (FLOW2)

public void Search2()
    {
        DeleteThumbnails2();
        thumbUrls.Clear();
        //turn on target's video player
        //need to implement this in V2 when assets ready
        /*/// switch(fm.currentTarget)
        {
            case 0:
                return;
            case 1:
                vidReference1.SetActive(true);
                break;
            case 2:
                vidReference2.SetActive(true);
                break;
            case 3:
                vidReference3.SetActive(true);
                break;
            case 4:
                vidReference4.SetActive(true);
                break;
            case 5:
                vidReference5.SetActive(true);
                break;
        }
        ///*/
        //do nothing if no targets created yet or if indexed target not created yet
        if (fm.targetStatus[fm.currentTarget-1] == "none")
            return;

        YoutubeAPIManager.YoutubeSearchOrderFilter mainFilter = YoutubeAPIManager.YoutubeSearchOrderFilter.none;
       
        youtubeapi.Search(searchField2.text, maxThumbResults, mainFilter, YoutubeAPIManager.YoutubeSafeSearchFilter.none, OnSearchDone2);
    }


    void OnSearchDone2(YoutubeData[] results)
    {
        //videoUIResult.SetActive(true);
        LoadVideosOnUI2(results);
    }

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
            newThumbnail.GetComponent<Button>().onClick.AddListener(delegate {this.GetComponent<YoutubeAPIManager>().SetVideoInfo(TempIterator + 1);});
            newThumbnail.GetComponent<Button>().onClick.AddListener(delegate {mainCanvas.GetComponent<PanelController>().OpenPanel(viewLibraryContentPanel);});
        } 
    }

    public void DeleteThumbnails2(){
		foreach (Transform child in thumbNailParentContent2.transform) {
     			GameObject.Destroy(child.gameObject);
        }
	}

    public void ClearSearchField2(){
        searchField2.text = "";
    }

}
