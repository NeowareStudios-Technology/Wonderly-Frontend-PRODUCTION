using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewLibContentUiManager : MonoBehaviour {
	//script reference
	public FilesManager fm;
	//used to control display of repo logo
	public GameObject youtubeLogo;
	public GameObject pixabayLogo;
	public GameObject polyLogo;
	public GameObject youtubeLogoEditFlow;
	public GameObject pixabayLogoEditFlow;
	public GameObject polyLogoEditFlow;
	
	
	//display correct repo logo depending on which repo is being searched
	void Update () {
		if (fm.currentTarget > 0)
		{
			switch (fm.targetStatus[fm.currentTarget-1])
			{
				case "none":
					youtubeLogo.SetActive(false);
					polyLogo.SetActive(false);
					pixabayLogo.SetActive(false);
					youtubeLogoEditFlow.SetActive(false);
					polyLogoEditFlow.SetActive(false);
					pixabayLogoEditFlow.SetActive(false);
					break;
				case "created":
					youtubeLogo.SetActive(false);
					polyLogo.SetActive(false);
					pixabayLogo.SetActive(false);
					youtubeLogoEditFlow.SetActive(false);
					polyLogoEditFlow.SetActive(false);
					pixabayLogoEditFlow.SetActive(false);
					break;
				case "model":
					youtubeLogo.SetActive(false);
					polyLogo.SetActive(true);
					pixabayLogo.SetActive(false);
					youtubeLogoEditFlow.SetActive(false);
					polyLogoEditFlow.SetActive(true);
					pixabayLogoEditFlow.SetActive(false);
					break;
				case "image":
					youtubeLogo.SetActive(false);
					polyLogo.SetActive(false);
					pixabayLogo.SetActive(true);
					youtubeLogoEditFlow.SetActive(false);
					polyLogoEditFlow.SetActive(false);
					pixabayLogoEditFlow.SetActive(true);
					break;
				case "video":
					youtubeLogo.SetActive(true);
					polyLogo.SetActive(false);
					pixabayLogo.SetActive(false);
					youtubeLogoEditFlow.SetActive(true);
					polyLogoEditFlow.SetActive(false);
					pixabayLogoEditFlow.SetActive(false);
					break;
			}
		}
	}
}
