using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PolyToolkit;
using Sample;

public class UiManager : MonoBehaviour {
	public FilesManager fm;
	public FirebaseManager fbm;
	public SaveManager sm;
	public ArPairDisplayManager apdm;
	public ModelInitializer mi;

	public int currentTargetNum;

	//public Image videoHighlight;
	//public Image modelHighlight;
	//public Image imageHighlight;
	//public Image textHighlight;

	public Text ArLabel1;
	public Text ArLabel2;
	public Text ArLabel3;
	public Text ArLabel4;
	public Text ArLabel5;

	public GameObject arPair1;
	public GameObject arPair2;
	public GameObject arPair3;
	public GameObject arPair4;
	public GameObject arPair5;

	//public GameObject arPairCover1;
	//public GameObject arPairCover2;
	//public GameObject arPairCover3;
	//public GameObject arPairCover4;
	//public GameObject arPairCover5;

	//public GameObject deletePair1;
	//public GameObject deletePair2;
	//public GameObject deletePair3;
	//public GameObject deletePair4;
	//public GameObject deletePair5;

	public GameObject modelIcon1;
	public GameObject imageIcon1;
	public GameObject videoIcon1;
	public GameObject modelIcon2;
	public GameObject imageIcon2;
	public GameObject videoIcon2;
	public GameObject modelIcon3;
	public GameObject imageIcon3;
	public GameObject videoIcon3;
	public GameObject modelIcon4;
	public GameObject imageIcon4;
	public GameObject videoIcon4;
	public GameObject modelIcon5;
	public GameObject imageIcon5;
	public GameObject videoIcon5;

	//public GameObject chosenObjectDisplay1;
	//public GameObject chosenObjectDisplay2;
	//public GameObject chosenObjectDisplay3;
	//public GameObject chosenObjectDisplay4;
	//public GameObject chosenObjectDisplay5;
	public Text chosenModelText;
	public Text chosenVideoText;
	public Text chosenImageText;
	public GameObject loadingPanel;

	//used to disable button if there are 3 targets
	public GameObject addWonderButton;

	//public GameObject rotateButton1;
	//public Text rotateButton1Text;
	//public GameObject rotateButton2;

	public Text description;

    //public InputField targetSwitchTitle;

    //public InputField summaryTitle;
    public RectTransform wonderPanelRectTrans;
    public Vector2 noWondersHeight = new Vector2(0,1600);
    public Vector2 oneWondersHeight= new Vector2(0,1850);
    public Vector2 twoWondersHeight =  new Vector2(0,2550);
    public Vector2 fullWondersHeight = new Vector2(0,3200);
	// Use this for initialization
	void Start ()
    {
		//videoHighlight.gameObject.SetActive(false);
		//modelHighlight.gameObject.SetActive(false);
		//imageHighlight.gameObject.SetActive(false);
		//textHighlight.gameObject.SetActive(false);

		//summaryTitle.onValueChange.AddListener(delegate {OnSummaryTitleChange(); });
		//targetSwitchTitle.onValueChange.AddListener(delegate {OnSwitchTitleChange(); });
	}


	public void OnSummaryTitleChange()
	{
			//targetSwitchTitle.text = summaryTitle.text;
	}

	public void OnSwitchTitleChange()
	{
			//summaryTitle.text = targetSwitchTitle.text;
	}

	
	
	// handles what UI will be displayed based on the 1 model/video/pic per target rule
	void Update () {

		//controls whether the addWonder button appears based on number of targets
		if (fm.targetCount == 3)
		{
			addWonderButton.SetActive(false);
		}
		else if (fm.targetCount < 3)
		{
			addWonderButton.SetActive(true);
		}


        //Sets Content Rect Transform based on target count
        if(fm.targetCount == 0)
            wonderPanelRectTrans.sizeDelta = noWondersHeight;        
        else if(fm.targetCount ==1)        
            wonderPanelRectTrans.sizeDelta = oneWondersHeight;
        else if(fm.targetCount ==2)        
            wonderPanelRectTrans.sizeDelta = twoWondersHeight;       
        else if(fm.targetCount ==3)     
            wonderPanelRectTrans.sizeDelta = fullWondersHeight;

        

        switch (fm.targetStatus[0])
		{
			case "none":
				arPair1.SetActive(false);
				//arPairCover1.SetActive(true);
				//deletePair1.SetActive(false);
				break;
			case "created":
				arPair1.SetActive(true);
				//arPairCover1.SetActive(false);
				//deletePair1.SetActive(true);
				modelIcon1.SetActive(false);
				imageIcon1.SetActive(false);
				videoIcon1.SetActive(false);
				break;
			case "model":
				arPair1.SetActive(true);
				//arPairCover1.SetActive(false);
				//deletePair1.SetActive(true);
				modelIcon1.SetActive(true);
				imageIcon1.SetActive(false);
				videoIcon1.SetActive(false);
				break;
			case "image":
				arPair1.SetActive(true);
				//arPairCover1.SetActive(false);
				//deletePair1.SetActive(true);
				modelIcon1.SetActive(false);
				imageIcon1.SetActive(true);
				videoIcon1.SetActive(false);
				break;
			case "video":
				arPair1.SetActive(true);
				//arPairCover1.SetActive(false);
				//deletePair1.SetActive(true);
				modelIcon1.SetActive(false);
				imageIcon1.SetActive(false);
				videoIcon1.SetActive(true);
				break;
		}

		switch(fm.targetStatus[1])
		{
			case "none":
				arPair2.SetActive(false);
				//arPairCover2.SetActive(true);
				//deletePair2.SetActive(false);
				break;
			case "created":
				arPair2.SetActive(true);
				//arPairCover2.SetActive(false);
				//deletePair2.SetActive(true);
				modelIcon2.SetActive(false);
				imageIcon2.SetActive(false);
				videoIcon2.SetActive(false);
				break;
			case "model":
				arPair2.SetActive(true);
				//arPairCover2.SetActive(false);
				//deletePair2.SetActive(true);
				modelIcon2.SetActive(true);
				imageIcon2.SetActive(false);
				videoIcon2.SetActive(false);
				break;
			case "image":
				arPair2.SetActive(true);
				//arPairCover2.SetActive(false);
				//deletePair2.SetActive(true);
				modelIcon2.SetActive(false);
				imageIcon2.SetActive(true);
				videoIcon2.SetActive(false);
				break;
			case "video":
				arPair2.SetActive(true);
				//arPairCover2.SetActive(false);
				//deletePair2.SetActive(true);
				modelIcon2.SetActive(false);
				imageIcon2.SetActive(false);
				videoIcon2.SetActive(true);
				break;
		}

		switch(fm.targetStatus[2])
		{
			case "none":
				arPair3.SetActive(false);
				//arPairCover3.SetActive(true);
				//deletePair3.SetActive(false);
				break;
			case "created":
				arPair3.SetActive(true);
				//arPairCover3.SetActive(false);
				//deletePair3.SetActive(true);
				modelIcon3.SetActive(false);
				imageIcon3.SetActive(false);
				videoIcon3.SetActive(false);
				break;
			case "model":
				arPair3.SetActive(true);
				//arPairCover3.SetActive(false);
				//deletePair3.SetActive(true);
				modelIcon3.SetActive(true);
				imageIcon3.SetActive(false);
				videoIcon3.SetActive(false);
				break;
			case "image":
				arPair3.SetActive(true);
				//arPairCover3.SetActive(false);
				//deletePair3.SetActive(true);
				modelIcon3.SetActive(false);
				imageIcon3.SetActive(true);
				videoIcon3.SetActive(false);
				break;
			case "video":
				arPair3.SetActive(true);
				//arPairCover3.SetActive(false);
				//deletePair3.SetActive(true);
				modelIcon3.SetActive(false);
				imageIcon3.SetActive(false);
				videoIcon3.SetActive(true);
				break;
		}

		switch(fm.targetStatus[3])
		{
			case "none":
				arPair4.SetActive(false);
				//arPairCover4.SetActive(true);
				//deletePair4.SetActive(false);
				break;
			case "created":
				arPair4.SetActive(true);
				//arPairCover4.SetActive(false);
				//deletePair4.SetActive(true);
				modelIcon4.SetActive(false);
				imageIcon4.SetActive(false);
				videoIcon4.SetActive(false);
				break;
			case "model":
				arPair4.SetActive(true);
				//arPairCover4.SetActive(false);
				//deletePair4.SetActive(true);
				modelIcon4.SetActive(true);
				imageIcon4.SetActive(false);
				videoIcon4.SetActive(false);
				break;
			case "image":
				arPair4.SetActive(true);
				//arPairCover4.SetActive(false);
				///deletePair4.SetActive(true);
				modelIcon4.SetActive(false);
				imageIcon4.SetActive(true);
				videoIcon4.SetActive(false);
				break;
			case "video":
				arPair4.SetActive(true);
				//arPairCover4.SetActive(false);
				//deletePair4.SetActive(true);
				modelIcon4.SetActive(false);
				imageIcon4.SetActive(false);
				videoIcon4.SetActive(true);
				break;
		}

		switch(fm.targetStatus[4])
		{
			case "none":
				arPair5.SetActive(false);
				//arPairCover5.SetActive(true);
				//deletePair5.SetActive(false);
				break;
			case "created":
				arPair5.SetActive(true);
				//arPairCover5.SetActive(false);
				//deletePair5.SetActive(true);
				modelIcon5.SetActive(false);
				imageIcon5.SetActive(false);
				videoIcon5.SetActive(false);
				break;
			case "model":
				arPair5.SetActive(true);
				//arPairCover5.SetActive(false);
				//deletePair5.SetActive(true);
				modelIcon5.SetActive(true);
				imageIcon5.SetActive(false);
				videoIcon5.SetActive(false);
				break;
			case "image":
				arPair5.SetActive(true);
				//arPairCover5.SetActive(false);
				//deletePair5.SetActive(true);
				modelIcon5.SetActive(false);
				imageIcon5.SetActive(true);
				videoIcon5.SetActive(false);
				break;
			case "video":
				arPair5.SetActive(true);
				//arPairCover5.SetActive(false);
				//deletePair5.SetActive(true);
				modelIcon5.SetActive(false);
				imageIcon5.SetActive(false);
				videoIcon5.SetActive(true);
				break;
		}
/*/// 
		//activate chosen object gameObject depending on which target is selected
		GameObject chosenObjectDisplay = chosenObjectDisplay1;
		switch(fm.currentTarget)
		{
			//if the current target is 0, return so that index out of range error doesnt occur
			case 0:
				chosenObjectDisplay1.SetActive(false);
				chosenObjectDisplay2.SetActive(false);
				chosenObjectDisplay3.SetActive(false);
				chosenObjectDisplay4.SetActive(false);
				chosenObjectDisplay5.SetActive(false);
				return;
			case 1:
				chosenObjectDisplay1.SetActive(true);
				chosenObjectDisplay2.SetActive(false);
				chosenObjectDisplay3.SetActive(false);
				chosenObjectDisplay4.SetActive(false);
				chosenObjectDisplay5.SetActive(false);
				chosenObjectDisplay = chosenObjectDisplay1;
				break;
			case 2:
				chosenObjectDisplay1.SetActive(false);
				chosenObjectDisplay2.SetActive(true);
				chosenObjectDisplay3.SetActive(false);
				chosenObjectDisplay4.SetActive(false);
				chosenObjectDisplay5.SetActive(false);
				chosenObjectDisplay = chosenObjectDisplay2;
				break;
			case 3:
				chosenObjectDisplay1.SetActive(false);
				chosenObjectDisplay2.SetActive(false);
				chosenObjectDisplay3.SetActive(true);
				chosenObjectDisplay4.SetActive(false);
				chosenObjectDisplay5.SetActive(false);
				chosenObjectDisplay = chosenObjectDisplay3;
				break;
			case 4:
				chosenObjectDisplay1.SetActive(false);
				chosenObjectDisplay2.SetActive(false);
				chosenObjectDisplay3.SetActive(false);
				chosenObjectDisplay4.SetActive(true);
				chosenObjectDisplay5.SetActive(false);
				chosenObjectDisplay = chosenObjectDisplay4;
				break;
			case 5:
				chosenObjectDisplay1.SetActive(false);
				chosenObjectDisplay2.SetActive(false);
				chosenObjectDisplay3.SetActive(false);
				chosenObjectDisplay4.SetActive(false);
				chosenObjectDisplay5.SetActive(true);
				chosenObjectDisplay = chosenObjectDisplay5;
				break;
		}

		//activate/deactivate chosen object text based on object type
		switch(fm.targetStatus[fm.currentTarget-1])
		{
			case "none":
				//rotateButton1.SetActive(false);
				//rotateButton2.SetActive(false);
				chosenObjectDisplay.SetActive(false);
				break;
			case "created":
				//rotateButton1.SetActive(false);
				//rotateButton2.SetActive(false);
				chosenObjectDisplay.SetActive(false);
				break;
			case "model":
				//rotateButton1.SetActive(true);
				//rotateButton1Text.text = "Rotate X";
				//rotateButton2.SetActive(true);
				chosenObjectDisplay.SetActive(true);
				chosenObjectDisplay.transform.GetChild(1).gameObject.SetActive(false);
				chosenObjectDisplay.transform.GetChild(2).gameObject.SetActive(true);
				chosenObjectDisplay.transform.GetChild(3).gameObject.SetActive(false);
				break;
			case "image":
				//rotateButton1.SetActive(true);
				//rotateButton1Text.text = "Rotate";
				//rotateButton2.SetActive(false);
				chosenObjectDisplay.SetActive(true);
				chosenObjectDisplay.transform.GetChild(1).gameObject.SetActive(false);
				chosenObjectDisplay.transform.GetChild(2).gameObject.SetActive(false);
				chosenObjectDisplay.transform.GetChild(3).gameObject.SetActive(true);
				break;
			case "video":	
				//rotateButton1.SetActive(true);
				//rotateButton1Text.text = "Rotate";
				//rotateButton2.SetActive(false);
				chosenObjectDisplay.SetActive(true);
				chosenObjectDisplay.transform.GetChild(1).gameObject.SetActive(true);
				chosenObjectDisplay.transform.GetChild(2).gameObject.SetActive(false);
				chosenObjectDisplay.transform.GetChild(3).gameObject.SetActive(false);
				break;
		}
		///*/

	}

	//used to reset the searched for model thumbnails 
	//called by ArPairDisplayManager
	public void startResetModelThumbs()
	{
		StartCoroutine("resetModelThumbs");
	}

	public IEnumerator resetModelThumbs()
	{
		//wait for 2 seconds for better user experience
		yield return new WaitForSeconds(2);
		for (int i = 0; i < 18; i++)
		{
			apdm.modelThumbs[i].sprite = apdm.blankImage.sprite;
		}
	}

	//used to reset the searched for video thumbnails 
	//called by ArPairDisplayManager
	public void startResetVideoThumbs()
	{
		StartCoroutine("resetVideoThumbs");
	}

	public IEnumerator resetVideoThumbs()
	{
		//wait for 2 seconds for better user experience
		yield return new WaitForSeconds(2);
		for (int i = 0; i < 18; i++)
		{
			apdm.videoThumbs[i].sprite = apdm.blankImage.sprite;
		}
	}
	public void SetLoadingPanelActive(bool settingActive){
		if (settingActive){
			loadingPanel.SetActive(true);
			loadingPanel.transform.parent.gameObject.SetActive(true);
		}
		else{
			loadingPanel.SetActive(false);
			loadingPanel.transform.parent.gameObject.SetActive(false);
		}
	}
}

