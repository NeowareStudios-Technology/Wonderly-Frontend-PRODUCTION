//=============================================================================================================================
//
// Copyright (c) 2015-2018 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
// EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
// and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//=============================================================================================================================

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System;
using EasyAR;

using UnityEngine.UI;


namespace Sample
{
    public class FilesManager : MonoBehaviour
    {
        public targetObjectManager tom;
        public ImageTargetManager itm;
        public FirebaseManager fbm;
        public CloudEndpointsApiManager ceam;
        public string MarksDirectory;
        public string SaveDirectory;
        private bool isWriting;
        //private takeTargetPicture ui;
        public int TARGET_LIMIT = 3;
        public int targetCount = 0;
        public int currentTarget =0;
        //will hold whether each target has image, video, model, is "created" , or "none"
        public string[] targetStatus = {"none","none","none","none","none"};
        //will hold whether each target has text ("none" or "text")
        //public string[] targetText = {"none", "none", "none", "none", "none"};
        //will hold whether each target has been set with an object ("none" or "set")
        //public string[] targetSet = {"none", "none", "none", "none", "none"};

        public GameObject mainCanvas;
        public Animator firstPanelIfLoggedIn;
        public Animator welcomePanel;

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
            //PlayerPrefs.SetInt("isLoggedIn", 1);
            //Debugging ^

            
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

            Directory.CreateDirectory(SaveDirectory);
            //Directory.CreateDirectory(MarksDirectory);
            Debug.Log("MarkPath:" + Application.persistentDataPath);
            Debug.Log("Save Folder Path: " + SaveDirectory);

            Debug.Log("calling get profile info");

            StartCoroutine("delayedReset");

        }

        ///need to fix this coroutine for new UI
        public IEnumerator delayedReset()
        {
            yield return new WaitForSeconds(1);
            ClearTexture();
            tom.clearScene();
            itm.DeleteAllTargetsAndText();
            itm.target1.SetActive(true);
            itm.target2.SetActive(true);
            itm.target3.SetActive(true);
            itm.target4.SetActive(true);
            itm.target5.SetActive(true);
        }

        public void ModifyTargetStatusArray(string status)
        {
            if (currentTarget == 0)
                return;

            targetStatus[currentTarget - 1] = status;
        }

        public void StartTakePhoto()
        {
            //if target limit is reached, do not take another picture
            if (targetCount >= TARGET_LIMIT)
            {
                Debug.Log("Target limit of 5 reached");
                return;
            }
            if (!Directory.Exists(MarksDirectory))
                Directory.CreateDirectory(MarksDirectory);
            if (!isWriting)
                StartCoroutine(ImageCreate());
        }

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
            string testPath4 = Path.Combine(MarksDirectory, "targetPhoto4.jpg");
            string testPath5 = Path.Combine(MarksDirectory, "targetPhoto5.jpg");
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
            else if (!File.Exists(testPath4))
            {
                pathString = "targetPhoto4.jpg";
                currentTarget = 4;
            }
            else   
            {
                pathString = "targetPhoto5.jpg";
                currentTarget = 5;
            }


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

        public Dictionary<string, string> GetDirectoryName_FileDic()
        {
            if (!Directory.Exists(MarksDirectory))
                return new Dictionary<string, string>();
            return GetAllImagesFiles(MarksDirectory);
        }

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

        public void ClearTexture()
        {
            Dictionary<string, string> imageFileDic = GetAllImagesFiles(MarksDirectory);
            foreach (var path in imageFileDic)
                File.Delete(path.Value);
        }

        public void ClearOneTexture()
        {
            Dictionary<string, string> imageFileDic = GetAllImagesFiles(MarksDirectory);
            int count =1;
            foreach (var path in imageFileDic)
            {
                if (currentTarget == count)
                {
                    File.Delete(path.Value);
                }
                count++;
            }
        }

        public void nextTarget()
        {
            if (currentTarget < 5)
            {
                currentTarget++;
            }
        }

        public void prevTarget()
        {
            if (currentTarget > 1)
            {
                currentTarget--;
            }
        }

    }
}
