using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoCaptureArea : MonoBehaviour {

    public float widthDiv = 4;
    public float heightDiv = 4;
    public float width = 2;
    public float height =2;

    public Rect photoRect;


    private void OnEnable()
    {
        //photoRect = new Rect(Screen.width / widthDiv, Screen.height / heightDiv, Screen.width / width, Screen.height / height);
    }
    void OnGUI()
    {
        GUI.Box(new Rect(Screen.width / 4, Screen.height / 4, Screen.width / 2, Screen.height / 2),"");
       // GUI.Box(photoRect, "");
    }
}
