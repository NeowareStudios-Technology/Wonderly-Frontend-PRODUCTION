/******************************************************
*Project: Wonderly
*Modified by: David Lee Ramirez
*Date: 12/28/18
*Description: Handles playing of Youtube video from thumbnail.
              Handles playing video on correct AR target/wonder.
 ******************************************************/



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class YoutubeVideoUi : MonoBehaviour {

    public Text videoName;
    public string videoId,thumbUrl;
    public UnityEngine.UI.Image videoThumb;
    private GameObject mainUI;
    public FilesManager fm;
    public targetObjectManager tom;
    public ImageTargetManager itm;
    public GameObject lsh;
    public VideoSearchManager vsm;

    //set all needed script variables here (cant do this in GUI because this is a spawned script)
    void Awake()
    {
        GameObject localScriptHolder = GameObject.Find("Local Script Holder");
        GameObject youtubeScriptHolder = GameObject.Find("Youtube Script Holder");
        fm = localScriptHolder.GetComponent<FilesManager>();
        tom = localScriptHolder.GetComponent<targetObjectManager>();
        itm = localScriptHolder.GetComponent<ImageTargetManager>();
        vsm = youtubeScriptHolder.GetComponent<VideoSearchManager>();
    }

    //play youtube video on correct video player (one player for each AR target/wonder)
    public void PlayYoutubeVideo()
    {

        //clear thumbnails
        vsm.DeleteThumbnails();
        vsm.DeleteThumbnails2();
        //set lsh for loading panel
        lsh = GameObject.Find("Local Script Holder");
        
        if (videoId == "")
        {
            return;
        }

        //loading Panel deactivated in RequestResolver.DownloadUrl()
        lsh.GetComponent<UiManager>().SetLoadingPanelActive(true);

        Debug.Log("Now starting YoutubeVideoUi.PlayYoutubeVideo()");
        Debug.Log("video id: "+videoId);
        Debug.Log("current target: "+fm.currentTarget);
        Debug.Log("CORRECT CURRENT TARGET = 1");

            string videoPlayerString = "";
            switch(fm.currentTarget)
            {
                case 0:
                    return;
                case 1:
                    //if the target is not created yet, do not play video
                    if (fm.targetStatus[0] == "none")
                    {
                        return;
                    }


                    //debug
                    Debug.Log("video player null? :" + tom.videoPlayers[0]);


                    fm.targetStatus[0] = "video";
                    itm.target1.SetActive(true);
                    tom.videoPlayers[0].SetActive(true);
                    Debug.Log(videoId);
                    tom.videoPlayers[0].GetComponent<SimplePlayback>().PlayYoutubeVideo(videoId);
                    tom.videoPlayers[0].GetComponent<SimplePlayback>().unityVideoPlayer.loopPointReached += VideoFinished;
                    tom.videoPlayers[0].transform.position = new Vector3(0f, 0f, 0f);
                    break;
                case 2:
                    if (fm.targetStatus[1] == "none")
                    {
                        return;
                    }
                    fm.targetStatus[1] = "video";
                    itm.target2.SetActive(true);
                    tom.videoPlayers[1].SetActive(true);
                    Debug.Log(videoId);
                    tom.videoPlayers[1].GetComponent<SimplePlayback>().PlayYoutubeVideo(videoId);
                    tom.videoPlayers[1].GetComponent<SimplePlayback>().unityVideoPlayer.loopPointReached += VideoFinished;
                    tom.videoPlayers[1].transform.position = new Vector3(0f, 0f, 0f);
                    break;
                case 3:
                    if (fm.targetStatus[2] == "none")
                    {
                        return;
                    }
                    fm.targetStatus[2] = "video";
                    itm.target3.SetActive(true);
                    tom.videoPlayers[2].SetActive(true);
                    Debug.Log(videoId);
                    tom.videoPlayers[2].GetComponent<SimplePlayback>().PlayYoutubeVideo(videoId);
                    tom.videoPlayers[2].GetComponent<SimplePlayback>().unityVideoPlayer.loopPointReached += VideoFinished;
                    tom.videoPlayers[2].transform.position = new Vector3(0f, 0f, 0f);
                    break;
                case 4:
                    if (fm.targetStatus[3] == "none")
                    {
                        return;
                    }
                    fm.targetStatus[3] = "video";
                    itm.target4.SetActive(true);
                    tom.videoPlayers[3].SetActive(true);
                    Debug.Log(videoId);
                    tom.videoPlayers[3].GetComponent<SimplePlayback>().PlayYoutubeVideo(videoId);
                    tom.videoPlayers[3].GetComponent<SimplePlayback>().unityVideoPlayer.loopPointReached += VideoFinished;
                    tom.videoPlayers[3].transform.position = new Vector3(0f, 0f, 0f);
                    break;
                case 5:
                    if (fm.targetStatus[4] == "none")
                    {
                        return;
                    }
                    fm.targetStatus[4] = "video";
                    itm.target5.SetActive(true);
                    tom.videoPlayers[4].SetActive(true);
                    Debug.Log(videoId);
                    tom.videoPlayers[4].GetComponent<SimplePlayback>().PlayYoutubeVideo(videoId);
                    tom.videoPlayers[4].GetComponent<SimplePlayback>().unityVideoPlayer.loopPointReached += VideoFinished;
                    tom.videoPlayers[4].transform.position = new Vector3(0f, 0f, 0f);
                    break;
            }
    }

    private void VideoFinished(VideoPlayer vPlayer)
    {
        if (GameObject.FindObjectOfType<SimplePlayback>() != null)
        {
            GameObject.FindObjectOfType<SimplePlayback>().unityVideoPlayer.loopPointReached -= VideoFinished;
        }
        else if (GameObject.FindObjectOfType<HighQualityPlayback>() != null)
        {
            GameObject.FindObjectOfType<HighQualityPlayback>().unityVideoPlayer.loopPointReached -= VideoFinished;
        }
        
        Debug.Log("Video Finished");
        mainUI.SetActive(true);
    }

    public IEnumerator PlayVideo(string url)
    {
#if UNITY_ANDROID || UNITY_IOS
        yield return Handheld.PlayFullScreenMovie(url, Color.black, FullScreenMovieControlMode.Full, FullScreenMovieScalingMode.Fill);
#else
        yield return true;
#endif
        Debug.Log("below this line will run when the video is finished");
        VideoFinished();
    }

    public void LoadThumbnail()
    {
        StartCoroutine(DownloadThumb());
    }

    IEnumerator DownloadThumb()
    {
        WWW www = new WWW(thumbUrl);
        yield return www;
        Texture2D thumb = new Texture2D(100, 100);
        www.LoadImageIntoTexture(thumb);
        videoThumb.sprite = Sprite.Create(thumb, new Rect(0, 0, thumb.width, thumb.height), new Vector2(0.5f, 0.5f), 100);
    }

    public void VideoFinished()
    {
        Debug.Log("Finished!");
    }
}
