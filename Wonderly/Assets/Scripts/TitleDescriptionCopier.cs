using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleDescriptionCopier : MonoBehaviour {

	public Text currentTitle;
	public Text currentDescription;
	public InputField newTitle;
	public InputField newDescription;


	//copy the existing title and description to the input fields
	void OnEnable () {
		newTitle.text = currentTitle.text;
		newDescription.text = currentDescription.text;
	}
	
	//clear the text on the screen
	void OnDisable () {
		newTitle.text = "";
		newDescription.text = "";
	}
}
