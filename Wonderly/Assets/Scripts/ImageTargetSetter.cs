/******************************************************
*Project: Wonderly
*Created by: David Lee Ramirez
*Date: 12/28/18
*Description: Used for detecting any target image files
*             and making them AR targets.
*Copyright 2018 LeapWithAlice,LLC. All rights reserved
 ******************************************************/

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using EasyAR;
using System.Collections;
using System.IO;

public class ImageTargetSetter : MonoBehaviour {
	public FilesManager fm;
    public ImageTargetManager itm;
    //for dict of target:behavioue script
	public Dictionary<string, DynamicImageTagetBehaviour> imageTargetDic = new Dictionary<string, DynamicImageTagetBehaviour>();
    public int[] targetThumbCheck = {0,0,0,0,0};
    public UnityEngine.UI.Image[] targetThumbs = new UnityEngine.UI.Image[5];
    public UnityEngine.UI.Image blankImage;
    //for dict of file names:file contents
    private Dictionary<string, string> imageTargetName_FileDic;
    int[] fileCheck = {0,0,0,0,0};
	

    //Gets files in current working directory of app, checks for image target files, if image target file found 
    // then it gets set as an AR target (with proper EasyAr object) and gets set as a target thumbnail
	void Update()
    {
        //get the current file dict from working directory
        imageTargetName_FileDic = fm.GetDirectoryName_FileDic();

        fileCheck[0] = 0;
        fileCheck[1] = 0;
        fileCheck[2] = 0;
        fileCheck[3] = 0;
        fileCheck[4] = 0;
        
        //go through each file in the directory, see if it is targetPhoto1-targetPhoto5, set the image as an AR target and set thumbails
        foreach (var obj in imageTargetName_FileDic.Where(obj => !imageTargetDic.ContainsKey(obj.Key)))
        {
            //Debug.Log("this is obj.Key: "+obj.Key);
            if (obj.Key == "targetPhoto1")
            {
                //Debug.Log("case recognized: targetPhoto1");
                if (itm.target1.GetComponent<DynamicImageTagetBehaviour>() == null)
                {
                    //Debug.Log("attempting to add DynamicImageTagetBehaviour script 1");
                    itm.target1.SetActive(true);
                    itm.target1.tag = "target1";
                    var behaviour1 = itm.target1.AddComponent<DynamicImageTagetBehaviour>();
                    behaviour1.whichTargetAmI = 1;
                    behaviour1.Name = obj.Key;
                    behaviour1.Path = obj.Value.Replace(@"\", "/");
                    behaviour1.Storage = StorageType.Absolute;
                    //binds tracking behaviour to target behavior script (required)
                    behaviour1.Bind(itm.tracker1);
                    //keeps track of name of target and behavior
                    imageTargetDic.Add(obj.Key, behaviour1);
                    //set the target status array to reflect that this target has been created
                    if (fm.targetStatus[0] == "none")
                        fm.targetStatus[0] = "created";
                    fileCheck[0] = 1;

                    //if the target thumb has not yet been set and the target photo exists, set the target thumb
                    if (targetThumbCheck[0] == 0 && File.Exists(fm.targetPath1))
                    {
                        targetThumbs[0].sprite = IMG2Sprite.LoadNewSprite(fm.targetPath1);
                        targetThumbCheck[0] = 1;
                    }
                    //else if the target thumb has been set, but the target photo has been deleted, blank the target thumb
                    else if(targetThumbCheck[0] == 1 && !File.Exists(fm.targetPath1))
                    {
                        targetThumbs[0].sprite = blankImage.sprite;
                        targetThumbCheck[0] = 0;
                    }
                }
            }

            else if (obj.Key == "targetPhoto2")
            {
                //Debug.Log("case recognized: targetPhoto2");
                if (itm.target2.GetComponent<DynamicImageTagetBehaviour>() == null)
                {
                //Debug.Log("attempting to add DynamicImageTagetBehaviour script 2");
                itm.target2.SetActive(true);
                itm.target2.tag = "target2";
                var behaviour2 = itm.target2.AddComponent<DynamicImageTagetBehaviour>();
                behaviour2.whichTargetAmI = 2;
                behaviour2.Name = obj.Key;
                behaviour2.Path = obj.Value.Replace(@"\", "/");
                behaviour2.Storage = StorageType.Absolute;
                //binds tracking behaviour to target behavior script (required)
                behaviour2.Bind(itm.tracker2);
                //keeps track of name of target and behavior
                imageTargetDic.Add(obj.Key, behaviour2);
                //set the target status array to reflect that this target has been created
                if (fm.targetStatus[1] == "none")
                    fm.targetStatus[1] = "created";
                fileCheck[1] = 1;
                
                if (targetThumbCheck[1] == 0 && File.Exists(fm.targetPath2))
					{
						targetThumbs[1].sprite = IMG2Sprite.LoadNewSprite(fm.targetPath2);
						targetThumbCheck[1] = 1;
					}
					else if(targetThumbCheck[1] == 1 && !File.Exists(fm.targetPath2))
					{
						targetThumbs[1].sprite = blankImage.sprite;
						targetThumbCheck[1] = 0;
					}
                }
            }
            /*
            else if (obj.Key == "targetPhoto3")
            {
                //Debug.Log("case recognized: targetPhoto3");
                if (itm.target3.GetComponent<DynamicImageTagetBehaviour>() == null)
                {
                //Debug.Log("attempting to add DynamicImageTagetBehaviour script 3");
                itm.target3.SetActive(true);
                itm.target3.tag = "target3";
                var behaviour3 = itm.target3.AddComponent<DynamicImageTagetBehaviour>();
                behaviour3.whichTargetAmI = 3;
                behaviour3.Name = obj.Key;
                behaviour3.Path = obj.Value.Replace(@"\", "/");
                behaviour3.Storage = StorageType.Absolute;
                //binds tracking behaviour to target behavior script (required)
                behaviour3.Bind(itm.tracker3);
                //keeps track of name of target and behavior
                imageTargetDic.Add(obj.Key, behaviour3);
                //set the target status array to reflect that this target has been created
                if (fm.targetStatus[2] == "none")
                    fm.targetStatus[2] = "created";
                fileCheck[2] = 1;
                
                if (targetThumbCheck[3] == 0 && File.Exists(fm.targetPath3))
                {
                    targetThumbs[3].sprite = IMG2Sprite.LoadNewSprite(fm.targetPath3);
                    targetThumbCheck[3] = 1;
                }
                else if(targetThumbCheck[3] == 1 && !File.Exists(fm.targetPath3))
                {
                    targetThumbs[3].sprite = blankImage.sprite;
                    targetThumbCheck[3] = 0;
                }
                }
            }

            else if (obj.Key == "targetPhoto4")
            {
                //Debug.Log("case recognized: targetPhoto4");
                if (itm.target4.GetComponent<DynamicImageTagetBehaviour>() == null)
                {
                //Debug.Log("attempting to add DynamicImageTagetBehaviour script 4");
                itm.target4.SetActive(true);
                itm.target4.tag = "target4";
                var behaviour4 = itm.target4.AddComponent<DynamicImageTagetBehaviour>();
                behaviour4.whichTargetAmI = 4;
                behaviour4.Name = obj.Key;
                behaviour4.Path = obj.Value.Replace(@"\", "/");
                behaviour4.Storage = StorageType.Absolute;
                //binds tracking behaviour to target behavior script (required)
                behaviour4.Bind(itm.tracker4);
                //keeps track of name of target and behavior
                imageTargetDic.Add(obj.Key, behaviour4);
                //set the target status array to reflect that this target has been created
                if (fm.targetStatus[3] == "none")
                    fm.targetStatus[3] = "created";
                fileCheck[3] = 1;
                
                if (targetThumbCheck[4] == 0 && File.Exists(fm.targetPath4))
                {
                    targetThumbs[4].sprite = IMG2Sprite.LoadNewSprite(fm.targetPath4);
                    targetThumbCheck[4] = 1;
                }
                else if(targetThumbCheck[4] == 1 && !File.Exists(fm.targetPath4))
                {
                    targetThumbs[4].sprite = blankImage.sprite;
                    targetThumbCheck[4] = 0;
                }
                }
            }

            else if (obj.Key == "targetPhoto5")
            {
                //Debug.Log("case recognized: targetPhoto5");
                if (itm.target5.GetComponent<DynamicImageTagetBehaviour>() == null)
                {
                //Debug.Log("attempting to add DynamicImageTagetBehaviour script 5");
                itm.target5.SetActive(true);
                itm.target5.tag = "target5";
                var behaviour5 = itm.target5.AddComponent<DynamicImageTagetBehaviour>();
                behaviour5.whichTargetAmI = 5;
                behaviour5.Name = obj.Key;
                behaviour5.Path = obj.Value.Replace(@"\", "/");
                behaviour5.Storage = StorageType.Absolute;
                //binds tracking behaviour to target behavior script (required)
                behaviour5.Bind(itm.tracker5);
                //keeps track of name of target and behavior
                imageTargetDic.Add(obj.Key, behaviour5);
                //set the target status array to reflect that this target has been created
                if (fm.targetStatus[4] == "none")
                    fm.targetStatus[4] = "created";
                fileCheck[4] = 1;
                
                if (targetThumbCheck[4] == 0 && File.Exists(fm.targetPath5))
					{
						targetThumbs[4].sprite = IMG2Sprite.LoadNewSprite(fm.targetPath5);
						targetThumbCheck[4] = 1;
					}
					else if(targetThumbCheck[4] == 1 && !File.Exists(fm.targetPath5))
					{
						targetThumbs[4].sprite = blankImage.sprite;
						targetThumbCheck[4] = 0;
					}
                }
            }
            */
        }
    }
}
