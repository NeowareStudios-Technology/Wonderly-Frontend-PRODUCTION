using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleDescriptionClearer : MonoBehaviour {

	public InputField title;
	public InputField description;

	//clears both input and description input fields to ensure blank upon each opening of this screen
	void OnEnable () {
		title.text = "";
		description.text = "";
	}
}
