using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//place this script on a panel and when it is deactivated, ensure all help animations are turned off;
public class OnDisableEndAnimation : MonoBehaviour {
	
	public UiManager uim;
	public List<GameObject> coachmarks;

	void OnDisable(){
		uim.TurnOffCoachMarks(coachmarks);
	}
}
