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
	public PanelController pc;

	public int currentTargetNum;

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

	public Text chosenModelText;
	public Text chosenVideoText;
	public Text chosenImageText;
	public GameObject loadingPanel;

	//used to control display of repo logo
	public GameObject youtubeLogo;
	public GameObject pixabayLogo;
	public GameObject polyLogo;


	//used to disable button if there are 3 targets
	public GameObject addWonderButton;

	//public GameObject rotateButton1;
	//public Text rotateButton1Text;
	//public GameObject rotateButton2;

	public Text description;

	//for sizing Summary screen depending on target number
	public RectTransform wonderPanelRectTrans;
	public Vector2 noWondersHeight = new Vector2(0,1600);
	public Vector2 oneWondersHeight= new Vector2(0,1850);
	public Vector2 twoWondersHeight =  new Vector2(0,2550);
	public Vector2 fullWondersHeight = new Vector2(0,3200);

	//for clearing ViewLibraryContent screen
	public Text contentAttribs;
	public Image contentImage;

	//for clearing preview screen
	public Text previewTitle;
	public Text previewDescription;
	public Text[] previewWonderTitles = new Text[5];
	public Text[] previewWonderDescriptions = new Text[5];
	public Image[] previewImages = new Image[5];
	public Image previewCoverImage;

	//for clearing summary screen
	public Text summaryTitle;
	public Text summaryDescription;
	public Text[] summaryWonderTitles = new Text[5];
	public Text[] summaryWonderDescriptions = new Text[5];
	public Image[] summaryTargetImages = new Image[5];
	public Image[] summaryLinkedImages = new Image[5];
	public Image summaryCoverImage;

	//for checking input on journey title screen (main flow)
	public InputField titleInput1;
	public GameObject journeyInputError1;
	public Animator scanWonderAnimator;

	//for checking input on journey title screen (edit flow)
	public InputField titleInput2;
	public GameObject journeyInputError2;
	public Animator summaryAnimator;

	//for checking input on wonder title screen (main flow)
	public InputField wonderTitleInput1;
	public GameObject wonderInputError1;
	//uses summaryAnimator

	//for checking input on wonder title screen (edit flow)
	public InputField wonderTitleInput2;
	public GameObject wonderInputError2;
	//uses summaryAnimator


	public Image blankImage;
	public Image iconImage;

	
	
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


        // //Sets Content Rect Transform based on target count
        // if(fm.targetCount == 0)
        //     wonderPanelRectTrans.sizeDelta = noWondersHeight;        
        // else if(fm.targetCount ==1)        
        //     wonderPanelRectTrans.sizeDelta = oneWondersHeight;
        // else if(fm.targetCount ==2)        
        //     wonderPanelRectTrans.sizeDelta = twoWondersHeight;       
        // else if(fm.targetCount ==3)     
        //     wonderPanelRectTrans.sizeDelta = fullWondersHeight;

			
		//display correct repo logo depending on which repo is being searched
		if (fm.currentTarget > 0)
		{
			switch (fm.targetStatus[fm.currentTarget-1])
			{
				case "none":
					youtubeLogo.SetActive(false);
					polyLogo.SetActive(false);
					pixabayLogo.SetActive(false);
					break;
				case "created":
					youtubeLogo.SetActive(false);
					polyLogo.SetActive(false);
					pixabayLogo.SetActive(false);
					break;
				case "model":
					youtubeLogo.SetActive(false);
					polyLogo.SetActive(true);
					pixabayLogo.SetActive(false);
					break;
				case "image":
					youtubeLogo.SetActive(false);
					polyLogo.SetActive(false);
					pixabayLogo.SetActive(true);
					break;
				case "video":
					youtubeLogo.SetActive(true);
					polyLogo.SetActive(false);
					pixabayLogo.SetActive(false);
					break;
			}
		}

        
		//controls ar pair display
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

	//delayed reset so that UI doesnt look bad with instant reset
	//called by ViewLibraryContent back button
	public void startResetViewLibraryContent()
	{
		StartCoroutine("ResetViewLibraryContent");
	}


	public IEnumerator ResetViewLibraryContent()
	{
		yield return new WaitForSeconds(2);
		contentAttribs.text = "  ";
		contentImage.sprite = blankImage.sprite;
	}

	//called by CompleteJourneyCopy add new wonder button
	public void instantResetViewLibraryContent()
	{
		contentAttribs.text = "  ";
		contentImage.sprite = blankImage.sprite;
	}




	//delayed reset so that UI doesnt look bad with instant reset
	public void startResetPreviewScreen()
	{
		StartCoroutine("ResetPreviewScreen");
	}


	public IEnumerator ResetPreviewScreen()
	{
		yield return new WaitForSeconds(2);
		previewTitle.text = " ";
		previewDescription.text = " ";
		previewCoverImage.sprite = iconImage.sprite;
		for(int i = 0; i<5; i++)
		{
			previewImages[i].sprite = iconImage.sprite;
			previewWonderTitles[i].text = " ";
			previewWonderDescriptions[i].text = " ";
		}
	}


	public void InstantResetPreviewScreen()
	{
		previewTitle.text = " ";
		previewDescription.text = " ";
		previewCoverImage.sprite = iconImage.sprite;
		for(int i = 0; i<5; i++)
		{
			previewImages[i].sprite = iconImage.sprite;
			previewWonderTitles[i].text = " ";
			previewWonderDescriptions[i].text = " ";
		}
	}


	public void InstantResetSummaryScreen()
	{
		summaryTitle.text = " ";
		summaryDescription.text = " ";
		summaryCoverImage.sprite = iconImage.sprite;
		for(int i = 0; i<5; i++)
		{
			summaryLinkedImages[i].sprite = iconImage.sprite;
			summaryTargetImages[i].sprite = iconImage.sprite;
			summaryWonderTitles[i].text = " ";
			summaryWonderDescriptions[i].text = " ";
		}
	}

	public void EnsureJourneyTitleInput1()
	{
		if (titleInput1.text == "")
		{
			journeyInputError1.SetActive(true);
		}
		else 
		{
			pc.OpenPanel(scanWonderAnimator);
		}
	}

	public void EnsureJourneyTitleInput2()
	{
		if (titleInput2.text == "")
		{
			journeyInputError2.SetActive(true);
		}
		else 
		{
			pc.OpenPanel(summaryAnimator);
		}
	}

	public void EnsureWonderTitleInput1()
	{
		if (wonderTitleInput1.text == "")
		{
			wonderInputError1.SetActive(true);
		}
		else 
		{
			pc.OpenPanel(summaryAnimator);
		}
	}

	public void EnsureWonderTitleInput2()
	{
		if (wonderTitleInput2.text == "")
		{
			wonderInputError2.SetActive(true);
		}
		else 
		{
			pc.OpenPanel(summaryAnimator);
		}
	}
}

