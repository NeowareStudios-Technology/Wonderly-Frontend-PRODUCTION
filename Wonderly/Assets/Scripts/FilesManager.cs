/******************************************************
*Project: Wonderly
*Created by: David Lee Ramirez
*Date: 12/28/18
*Description: Used for handling files and memory management
*Copyright 2018 LeapWithAlice,LLC. All rights reserved
 ******************************************************/

using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System;
using EasyAR;
using UnityEngine.UI;


public class FilesManager : MonoBehaviour
{
    public targetObjectManager tom;
    public ImageTargetManager itm;
    public FirebaseManager fbm;
    public CloudEndpointsApiManager ceam;
    public ModelInitializer mi;
    public VideoSearchManager vsm;
    public pixabayManager pm;
    public string MarksDirectory;
    public string SaveDirectory;
    private bool isWriting;
    public int TARGET_LIMIT = 3;
    public int targetCount = 0;
    public int currentTarget =0;
    //will hold whether each target has image, video, model, is "created" , or "none"
    public string[] targetStatus = {"none","none","none","none","none"};
    //paths for target images
    public string targetPath1;
    public string targetPath2;
    public string targetPath3;
    public string targetPath4;
    public string targetPath5;

    public GameObject mainCanvas;
    public Animator firstPanelIfLoggedIn;
    public Animator welcomePanel;
    //for deactivating AR camera to save cpu
    public GameObject arCamera;

    //sets the current target (wonder)
    public void setCurrentTarget(int current)
    {
        currentTarget = current;
    }

    private IEnumerator openHome(){
        mainCanvas.GetComponent<PanelController>().initiallyOpen = firstPanelIfLoggedIn;
        mainCanvas.GetComponent<PanelController>().SetBottomPanelActive(true);
        yield return new WaitForSeconds(1.0f);
        mainCanvas.GetComponent<PanelController>().OpenPanel(firstPanelIfLoggedIn);
        
            Debug.Log("logged in auto");
    }

    void Awake()
    {   
        if (PlayerPrefs.GetInt("isLoggedIn")==1){
            //coroutine OpenHome esures homeScreen is opened whether or not mainCanvas already opened InitialPanel
            StartCoroutine(openHome());
            StartCoroutine(fbm.InternalLoginProcessAutomatic(PlayerPrefs.GetString("email"), PlayerPrefs.GetString("password")));
        }
        //if not logged in open the welcomePanel
        else{
            mainCanvas.GetComponent<PanelController>().initiallyOpen = welcomePanel;
        }
                        
        //delete all previous scene targets and objects

        //ui = FindObjectOfType<takeTargetPicture>();
        MarksDirectory = Application.persistentDataPath;
        SaveDirectory = Path.Combine(MarksDirectory, "SaveFolder");
        //clear save file if it exists
        if (Directory.Exists(SaveDirectory))
        {
            DirectoryInfo di = new DirectoryInfo(SaveDirectory);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete(); 
            }
        }
        //if save file doesnt exist, make one
        else
        {
            Directory.CreateDirectory(SaveDirectory);
        }

        //Directory.CreateDirectory(MarksDirectory);
        Debug.Log("MarkPath:" + Application.persistentDataPath);
        Debug.Log("Save Folder Path: " + SaveDirectory);

        Debug.Log("calling get profile info");

        StartCoroutine("delayedReset");

        StartCoroutine("delayedDeactivateArAfterInit");
        
        //set all paths of possible target images
        targetPath1 = Path.Combine(MarksDirectory, "targetPhoto1.jpg");
        targetPath2 = Path.Combine(MarksDirectory, "targetPhoto2.jpg");
        targetPath3 = Path.Combine(MarksDirectory, "targetPhoto3.jpg");
        targetPath4 = Path.Combine(MarksDirectory, "targetPhoto4.jpg");
        targetPath5 = Path.Combine(MarksDirectory, "targetPhoto5.jpg");

    }

    //resets working directory after the scripts recognize targets (if does before then they dont get reset)
    public IEnumerator delayedReset()
    {
        yield return new WaitForSeconds(1);
        ClearTextures();
        tom.clearScene();
        itm.DeleteAllTargetsAndText();
        itm.target1.SetActive(true);
        itm.target2.SetActive(true);
        itm.target3.SetActive(true);
        itm.target4.SetActive(true);
        itm.target5.SetActive(true);
    }

    //used to deactivate ar camera to save cpu ONLY AFTER camera is initialized
    public IEnumerator delayedDeactivateArAfterInit()
    {
        yield return new WaitForSeconds(3);
        arCamera.SetActive(false);
    }

    //sets current target to a specified status
    public void ModifyTargetStatusArray(string status)
    {
        if (currentTarget == 0)
            return;

        targetStatus[currentTarget - 1] = status;
    }

    //takes the picture that will be the image target
    public void StartTakePhoto()
    {
        //if target limit is reached, do not take another picture
        if (targetCount >= TARGET_LIMIT)
        {
            Debug.Log("Target limit of 2 reached");
            return;
        }
        if (!Directory.Exists(MarksDirectory))
            Directory.CreateDirectory(MarksDirectory);
        if (!isWriting)
            StartCoroutine(ImageCreate());
    }

    //created image
    IEnumerator ImageCreate()
    {
        //count the target (there can only be 5 targets)
        //targetCount++;

        isWriting = true;
        yield return new WaitForEndOfFrame();

        Texture2D photo = new Texture2D(Screen.width / 2, Screen.height / 2, TextureFormat.RGB24, false);
        photo.ReadPixels(new Rect(Screen.width / 4, Screen.height / 3, Screen.width / 2, Screen.height / 2), 0, 0, false);
        photo.Apply();

        byte[] data = photo.EncodeToJPG(80);
        DestroyImmediate(photo);
        photo = null;
        string pathString ="";

        //create image for earliest possible image "slot"
        string testPath1 = Path.Combine(MarksDirectory, "targetPhoto1.jpg");
        string testPath2 = Path.Combine(MarksDirectory, "targetPhoto2.jpg");
        string testPath3 = Path.Combine(MarksDirectory, "targetPhoto3.jpg");
        //string testPath4 = Path.Combine(MarksDirectory, "targetPhoto4.jpg");
        //string testPath5 = Path.Combine(MarksDirectory, "targetPhoto5.jpg");
        if (!File.Exists(testPath1))
        {
            pathString = "targetPhoto1.jpg";
            currentTarget = 1;
        }
        else if (!File.Exists(testPath2))
        {
            pathString = "targetPhoto2.jpg";
            currentTarget = 2;
        }
        else if (!File.Exists(testPath3))
        {
            pathString = "targetPhoto3.jpg";
            currentTarget = 3;
        }
        /* 
        else if (!File.Exists(testPath4))
        {
            pathString = "targetPhoto4.jpg";
            currentTarget = 4;
        }
        else   
        {
            pathString = "targetPhoto5.jpg";
            currentTarget = 5;
        }*/


        string photoPath = Path.Combine(MarksDirectory, pathString);

        FileStream file = File.Open(photoPath, FileMode.Create);
        file.BeginWrite(data, 0, data.Length, new AsyncCallback(endWriter), file);
    }

    void endWriter(IAsyncResult end)
    {
        using (FileStream file = (FileStream)end.AsyncState)
        {
            file.EndWrite(end);
            isWriting = false;
            //ui.StartShowMessage = true;
        }
        targetCount++;
    }

    //returns dict of fileNames:fileContents in working directory
    public Dictionary<string, string> GetDirectoryName_FileDic()
    {
        if (!Directory.Exists(MarksDirectory))
            return new Dictionary<string, string>();
        return GetAllImagesFiles(MarksDirectory);
    }

    //gets all image files in working directory
    private Dictionary<string, string> GetAllImagesFiles(string path)
    {
        Dictionary<string, string> imgefilesDic = new Dictionary<string, string>();
        foreach (var file in Directory.GetFiles(path))
        {
            if (Path.GetExtension(file) == ".jpg" || Path.GetExtension(file) == ".bmp" || Path.GetExtension(file) == ".png")
            {
                imgefilesDic.Add(Path.GetFileNameWithoutExtension(file), file);
            }

        }
        return imgefilesDic;
    }

    //deletes all image files in working directory
    public void ClearTextures()
    {
        Dictionary<string, string> imageFileDic = GetAllImagesFiles(MarksDirectory);
        foreach (var path in imageFileDic)
            File.Delete(path.Value);
    }


    //destroys all temporary (unneeded) instantiated prefabs and unloads texture memory
    public void unloadUnused()
    {
        //clear pixabay thumbnails
        pm.DestroyChildrenOfCoverImageContent();
        pm.DestroyChildrenOfCoverImageContent2();
        pm.DestroyChildrenOfImageContent();
        pm.DestroyChildrenOfImageContent2();

        //clear poly thumbnails
        mi.DeleteThumbnails();
        mi.DeleteThumbnails2();

        //clear youtube thumbnails
        vsm.DeleteThumbnails();
        vsm.DeleteThumbnails2();

        Resources.UnloadUnusedAssets();
        Debug.Log("unused assets unloaded");
    }
}

