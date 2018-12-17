using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WonderTitleDescCopier : MonoBehaviour {
	public InputField newWonderTitle;
	public InputField newWonderDesc;

	public Text[] currentWonderTitles = new Text[5];
	public Text[] currentWonderDescs = new Text[5];
	
	public void copyWonderTitleDesc(int index)
	{
		newWonderTitle.text = currentWonderTitles[index-1].text;
		newWonderDesc.text = currentWonderDescs[index-1].text;
	}
}
