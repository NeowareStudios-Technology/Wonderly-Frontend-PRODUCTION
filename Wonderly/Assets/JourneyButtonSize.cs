using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JourneyButtonSize : MonoBehaviour {


	void Start () {
		GameObject mainCanvas = GameObject.Find("Canvas");
		this.GetComponent<LayoutElement>().minWidth = mainCanvas.GetComponent<RectTransform>().sizeDelta.x;
		//this.GetComponent<RectTransform>().sizeDelta = new Vector2( mainCanvas.GetComponent<RectTransform>().sizeDelta.x, 100.0f); 
	}
	
}
