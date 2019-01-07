/******************************************************
*Project: Wonderly
*Created by: David Lee Ramirez
*Date: 12/28/18
*Description: Used for handling image target deletes
*Copyright 2018 LeapWithAlice,LLC. All rights reserved
 ******************************************************/

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using EasyAR;
using System.Collections;
using System.IO;


public class ImageTargetManager : MonoBehaviour
{
    //script references
    public FilesManager pathManager;
    public targetObjectManager tom;
    public ImageTargetSetter its;
    public FilesManager fm;
    public ArPairDisplayManager apdm;
    public LoadManager lm;
    //references to target GameObjects
    public GameObject target1;
    public GameObject target2;
    public GameObject target3;
    public GameObject target4;
    public GameObject target5;
    //used for default images (blank and icon)
    public UnityEngine.UI.Image blankImage;
    public UnityEngine.UI.Image iconImage;
    //references to AR tracker scripts
    public ImageTrackerBehaviour tracker1;
    public ImageTrackerBehaviour tracker2;
    public ImageTrackerBehaviour tracker3;
    public ImageTrackerBehaviour tracker4;
    public ImageTrackerBehaviour tracker5;
    public int count = 0;
    //bools that keep track of currently active targets
    public bool activeTarget1;
    public bool activeTarget2;
    public bool activeTarget3;
    public bool activeTarget4;
    public bool activeTarget5;
    //UI references
    public Text title;
    public Text editTitle;
    public Text description;
    public InputField titleInput;
    //Dictionary for file:script
    private Dictionary<string, DynamicImageTargetBehaviour> imageTargetDic = new Dictionary<string, DynamicImageTargetBehaviour>();


    //deletes all targets and set text
    public void DeleteAllTargetsAndText()
    {
        Debug.Log("1. starting itm.DeleteAllTargetsAndText...");
        foreach (var obj in imageTargetDic)
        {
            Destroy(obj.Value.gameObject.GetComponent<DynamicImageTargetBehaviour>());
        }

        tom.clearScene();

        Destroy(target1.GetComponent<DynamicImageTargetBehaviour>());
        Destroy(target2.GetComponent<DynamicImageTargetBehaviour>());
        Destroy(target3.GetComponent<DynamicImageTargetBehaviour>());
        Destroy(target4.GetComponent<DynamicImageTargetBehaviour>());
        Destroy(target5.GetComponent<DynamicImageTargetBehaviour>());

        MeshRenderer m1 = target1.GetComponent<MeshRenderer>();
        MeshRenderer m2 = target2.GetComponent<MeshRenderer>();
        MeshRenderer m3 = target3.GetComponent<MeshRenderer>();
        MeshRenderer m4 = target4.GetComponent<MeshRenderer>();
        MeshRenderer m5 = target5.GetComponent<MeshRenderer>();

        m1.enabled = true;
        m2.enabled = true;
        m3.enabled = true;
        m4.enabled = true;
        m5.enabled = true;


        //reset all objects for targets
        for (int i = 0; i<5; i++)
        {
            tom.removeTargetObject(i+1);
            pathManager.targetStatus[i] = "none";
        }

        //set strings for each target photo path
        string target1Path = Path.Combine(pathManager.MarksDirectory, "targetPhoto1.jpg");
        string target2Path = Path.Combine(pathManager.MarksDirectory, "targetPhoto2.jpg");
        string target3Path = Path.Combine(pathManager.MarksDirectory, "targetPhoto3.jpg");
        string target4Path = Path.Combine(pathManager.MarksDirectory, "targetPhoto4.jpg");
        string target5Path = Path.Combine(pathManager.MarksDirectory, "targetPhoto5.jpg");

        //delete each target photo if it exists
        if (File.Exists(target1Path))
        {
            File.Delete(target1Path);
        }
        if (File.Exists(target2Path))
        {
            File.Delete(target2Path);
        }
        if (File.Exists(target3Path))
        {
            File.Delete(target3Path);
        }
        if (File.Exists(target4Path))
        {
            File.Delete(target4Path);
        }
        if (File.Exists(target5Path))
        {
            File.Delete(target5Path);
        }

        //clear ui textures
        for (int j = 0; j<3; j++)
        {
            //remove from summary screen: linked images and target images
            apdm.targetThumbs[j].sprite = blankImage.sprite;
            apdm.targetObjectThumbs[j].sprite = blankImage.sprite;
            //remove from preview screen: target images
            lm.viewFlowTargetPreviews[j].sprite = blankImage.sprite;
        }
        //remove preview cover image
        lm.previewCoverImage.sprite = iconImage.sprite;
        //remove sumary cover image
        lm.coverImage.sprite = iconImage.sprite;
    


        pathManager.currentTarget = 0;
        its.imageTargetDic= new Dictionary<string, DynamicImageTargetBehaviour>();

        pathManager.targetCount = 0;

        //clear save file
        if (Directory.Exists(fm.SaveDirectory))
        {
            DirectoryInfo di = new DirectoryInfo(fm.SaveDirectory);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete(); 
            }
        }

        fm.unloadUnused();
    }
    
    //gets called by UI to delete currently indexed target
    public void DeleteTarget(int whichTarget)
    {
        //if there is no target loaded to this index, do nothing
        if (pathManager.targetStatus[whichTarget-1] == "none")
            return;
        int localCount = 1;

        Debug.Log("1. Starting DeleteTarget()");

        string thisPath = "";
        switch(whichTarget)
        {
            case 1:
                Debug.Log("2. Deleting targetPhoto1.jpg");
                thisPath = Path.Combine(pathManager.MarksDirectory, "targetPhoto1.jpg");
                File.Delete(thisPath);
                Destroy(target1.GetComponent<DynamicImageTargetBehaviour>());
                its.imageTargetDic.Remove("targetPhoto1");
                tom.removeTargetObject(1);
                fm.targetStatus[0] = "none";
                break;
            case 2:
                thisPath = Path.Combine(pathManager.MarksDirectory, "targetPhoto2.jpg");
                File.Delete(thisPath);
                Destroy(target2.GetComponent<DynamicImageTargetBehaviour>());
                its.imageTargetDic.Remove("targetPhoto2");
                tom.removeTargetObject(2);
                fm.targetStatus[1] = "none";
                break;
            case 3:
                thisPath = Path.Combine(pathManager.MarksDirectory, "targetPhoto3.jpg");
                File.Delete(thisPath);
                Destroy(target3.GetComponent<DynamicImageTargetBehaviour>());
                its.imageTargetDic.Remove("targetPhoto3");
                tom.removeTargetObject(3);
                fm.targetStatus[2] = "none";
                break;
            case 4:
                thisPath = Path.Combine(pathManager.MarksDirectory, "targetPhoto4.jpg");
                File.Delete(thisPath);
                Destroy(target4.GetComponent<DynamicImageTargetBehaviour>());
                its.imageTargetDic.Remove("targetPhoto4");
                tom.removeTargetObject(4);
                fm.targetStatus[3] = "none";
                break;
            case 5:
                thisPath = Path.Combine(pathManager.MarksDirectory, "targetPhoto5.jpg");
                File.Delete(thisPath);
                Destroy(target5.GetComponent<DynamicImageTargetBehaviour>());
                its.imageTargetDic.Remove("targetPhoto5");
                tom.removeTargetObject(5);
                fm.targetStatus[4] = "none";
                break;
        }

        pathManager.targetCount --;

        Resources.UnloadUnusedAssets();
    }

    //gets called by UI to delete currently indexed target
    public void DeleteCurrentTarget()
    {
        //if there is no target loaded to this index, do nothing
        if (pathManager.targetStatus[pathManager.currentTarget-1] == "none")
            return;
        int localCount = 1;

        Debug.Log("1. Starting DeleteTarget()");

        string thisPath = "";
        switch(pathManager.currentTarget)
        {
            case 1:
                Debug.Log("2. Deleting targetPhoto1.jpg");
                thisPath = Path.Combine(pathManager.MarksDirectory, "targetPhoto1.jpg");
                File.Delete(thisPath);
                Destroy(target1.GetComponent<DynamicImageTargetBehaviour>());
                its.imageTargetDic.Remove("targetPhoto1");
                tom.removeTargetObject(1);
                fm.targetStatus[0] = "none";
                break;
            case 2:
                thisPath = Path.Combine(pathManager.MarksDirectory, "targetPhoto2.jpg");
                File.Delete(thisPath);
                Destroy(target2.GetComponent<DynamicImageTargetBehaviour>());
                its.imageTargetDic.Remove("targetPhoto2");
                tom.removeTargetObject(2);
                fm.targetStatus[1] = "none";
                break;
            case 3:
                thisPath = Path.Combine(pathManager.MarksDirectory, "targetPhoto3.jpg");
                File.Delete(thisPath);
                Destroy(target3.GetComponent<DynamicImageTargetBehaviour>());
                its.imageTargetDic.Remove("targetPhoto3");
                tom.removeTargetObject(3);
                fm.targetStatus[2] = "none";
                break;
            case 4:
                thisPath = Path.Combine(pathManager.MarksDirectory, "targetPhoto4.jpg");
                File.Delete(thisPath);
                Destroy(target4.GetComponent<DynamicImageTargetBehaviour>());
                its.imageTargetDic.Remove("targetPhoto4");
                tom.removeTargetObject(4);
                fm.targetStatus[3] = "none";
                break;
            case 5:
                thisPath = Path.Combine(pathManager.MarksDirectory, "targetPhoto5.jpg");
                File.Delete(thisPath);
                Destroy(target5.GetComponent<DynamicImageTargetBehaviour>());
                its.imageTargetDic.Remove("targetPhoto5");
                tom.removeTargetObject(5);
                fm.targetStatus[4] = "none";
                break;
        }

        pathManager.targetCount --;

        Resources.UnloadUnusedAssets();
    }
}


