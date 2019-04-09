using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeRotationOfMyModel : MonoBehaviour {

	public Text rotationAxis;

	public void changeButtonAxis(){
		if (rotationAxis.text == "X")
		{
			rotationAxis.text = "Y";
		}
		else if (rotationAxis.text == "Y")
		{
			rotationAxis.text = "Z";
		}
		else if (rotationAxis.text == "Z")
		{
			rotationAxis.text = "X";
		}
	}
}
