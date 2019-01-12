/******************************************************
*Project: Wonderly
*Created by: David Lee Ramirez
*Date: 12/28/18
*Description: Clears Title and Description text for 
							create journey screen.
*Copyright 2018 LeapWithAlice,LLC. All rights reserved
 ******************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleDescriptionClearer : MonoBehaviour {

	//script references
	public InputField title;
	public InputField description;

    public bool clearOnExit = true;

	//clears both input and description input fields to ensure blank upon each opening of this screen
	void OnEnable ()
    {
        if (clearOnExit)
        {
		title.text = "";
		description.text = "";

        }
	}

    public void SetClearText(bool _value)
    {
        clearOnExit = _value;
    }
}
