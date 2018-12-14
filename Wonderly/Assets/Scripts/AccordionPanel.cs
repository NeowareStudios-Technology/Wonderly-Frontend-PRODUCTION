using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccordionPanel : MonoBehaviour {

	private Image questionMarkAsset;
	private Image closePanelAsset; 

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void AccordionOpen(Animator anim){
		if(anim.GetBool("Open")){
			anim.SetBool("Open", false);
		}
		else{
			anim.SetBool("Open", true);
		}
	}
	public void ChangeQuestionMarkAsset(GameObject thisButton){
		GameObject questionMarkAsset = thisButton.transform.GetChild(2).gameObject;
		GameObject xAsset = thisButton.transform.GetChild(3).gameObject;

		if (questionMarkAsset.activeSelf){
			questionMarkAsset.SetActive(false);
			xAsset.SetActive(true);
		}
		else{
			questionMarkAsset.SetActive(true);
			xAsset.SetActive(false);
		}
		
	}
}
