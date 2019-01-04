/******************************************************
*Project: Wonderly
*Created by: David Lee Ramirez
*Date: 12/28/18
*Description: Used to control behavior of AR target.
*Copyright 2018 LeapWithAlice,LLC. All rights reserved
 ******************************************************/

using UnityEngine;
using EasyAR;


public class DynamicImageTargetBehaviour : ImageTargetBehaviour
{
    public FilesManager fm;
    public ImageTargetManager itm;
    public targetObjectManager tom;
    public int whichTargetAmI;

    protected override void Awake()
    {
        base.Awake();
        fm = FindObjectOfType<FilesManager>();
        itm = FindObjectOfType<ImageTargetManager>();
        tom = FindObjectOfType<targetObjectManager>();
        TargetFound += OnTargetFound;
        TargetLost += OnTargetLost;
        TargetLoad += OnTargetLoad;
        TargetUnload += OnTargetUnload;
    }

    void OnTargetFound(TargetAbstractBehaviour behaviour)
    {
        Debug.Log("Found: " + Target.Id);
        //accesses fm to see if a video is assigned to this target and displays or does not display the video accordingly
        switch(whichTargetAmI)
        {
            case 1:
                    itm.activeTarget1 = true;
                    tom.videoPlayers[0].gameObject.transform.position = new Vector3(0f,0f,0f);
                    GameObject.FindWithTag("videoPlayer1").GetComponent<SimplePlayback>().unityVideoPlayer.Play();
                break;
            case 2:
                    itm.activeTarget2 = true;
                    tom.videoPlayers[1].gameObject.transform.position = new Vector3(0f,0f,0f);
                    GameObject.FindWithTag("videoPlayer2").GetComponent<SimplePlayback>().unityVideoPlayer.Play();
                break;
            case 3:
                    itm.activeTarget3 = true;
                    tom.videoPlayers[2].gameObject.transform.position = new Vector3(0f,0f,0f);
                    GameObject.FindWithTag("videoPlayer3").GetComponent<SimplePlayback>().unityVideoPlayer.Play();
                break;
            case 4:
                    itm.activeTarget4 = true;
                    tom.videoPlayers[3].gameObject.transform.position = new Vector3(0f,0f,0f);
                    GameObject.FindWithTag("videoPlayer4").GetComponent<SimplePlayback>().unityVideoPlayer.Play();
                break;
            case 5:
                    itm.activeTarget5 = true;
                    tom.videoPlayers[4].gameObject.transform.position = new Vector3(0f,0f,0f);
                    GameObject.FindWithTag("videoPlayer4").GetComponent<SimplePlayback>().unityVideoPlayer.Play();
                break;
        }
    }

    void OnTargetLost(TargetAbstractBehaviour behaviour)
    {
        Debug.Log("Lost: " + Target.Id);
        
        switch(whichTargetAmI)
        {
            case 1:
                Debug.Log("new position");
                itm.activeTarget1 = false;
                itm.target1.transform.position = new Vector3(0f,0f,0f);
                tom.videoPlayers[0].gameObject.transform.position = new Vector3(2000f,0f,0f);
                break;
            case 2:
                itm.activeTarget2 = false;
                itm.target2.transform.position = new Vector3(0f,0f,0f);
                tom.videoPlayers[1].gameObject.transform.position = new Vector3(2000f,0f,0f);
                break;
            case 3:
                itm.activeTarget3 = false;
                itm.target3.transform.position = new Vector3(0f,0f,0f);
                tom.videoPlayers[2].gameObject.transform.position = new Vector3(2000f,0f,0f);
                break;
            case 4:
                itm.activeTarget4 = false;
                itm.target4.transform.position = new Vector3(0f,0f,0f);
                tom.videoPlayers[3].gameObject.transform.position = new Vector3(2000f,0f,0f);
                break;
            case 5:
                itm.activeTarget5 = false;
                itm.target5.transform.position = new Vector3(0f,0f,0f);
                tom.videoPlayers[4].gameObject.transform.position = new Vector3(2000f,0f,0f);
                break;
        }
    }

    void OnTargetLoad(ImageTargetBaseBehaviour behaviour, ImageTrackerBaseBehaviour tracker, bool status)
    {
        Debug.Log("Load target (" + status + "): " + Target.Id + " (" + Target.Name + ") " + " -> " + tracker);
    }

    void OnTargetUnload(ImageTargetBaseBehaviour behaviour, ImageTrackerBaseBehaviour tracker, bool status)
    {
        Debug.Log("Unload target (" + status + "): " + Target.Id + " (" + Target.Name + ") " + " -> " + tracker);
    }
}
