using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//place this script on a panel and when it is deactivated, ensure all help animations are turned off;
public class OnDisableEndAnimation : MonoBehaviour {
	
	public List<GameObject> coachmarks;

	void OnDisable(){
		foreach(GameObject cm in coachmarks){
			cm.SetActive(true);
			Color alphaColorZero = Color.white;
			alphaColorZero.a = 0.0f;
			cm.GetComponent<Image>().color = alphaColorZero;
			cm.GetComponent<Image>().enabled = false;
		}
	}
}
