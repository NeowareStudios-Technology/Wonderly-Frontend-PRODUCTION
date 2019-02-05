/******************************************************
*Project: Wonderly
*Created by: David Lee Ramirez
*Date: 12/28/18
*Description: Controls Preview Screen (NewAddViewContent) UI
*Copyright 2018 LeapWithAlice,LLC. All rights reserved
 ******************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewScreenUiManager : MonoBehaviour {
	//script references
	public FilesManager fm;
	//stubs of preview target images and AR targets
	public GameObject[] previewStubs = new GameObject[5];
	
	//displays preview stubs based on number of targets that exist
	void Update () {
		switch (fm.targetCount)
		{
			case 0:
				previewStubs[0].SetActive(false);
				previewStubs[1].SetActive(false);
				previewStubs[2].SetActive(false);
				previewStubs[3].SetActive(false);
				previewStubs[4].SetActive(false);
				break;
			case 1:
				previewStubs[0].SetActive(true);
				previewStubs[1].SetActive(false);
				previewStubs[2].SetActive(false);
				previewStubs[3].SetActive(false);
				previewStubs[4].SetActive(false);
				break;
			case 2:
				previewStubs[0].SetActive(true);
				previewStubs[1].SetActive(true);
				previewStubs[2].SetActive(false);
				previewStubs[3].SetActive(false);
				previewStubs[4].SetActive(false);
				break;
			case 3:
				previewStubs[0].SetActive(true);
				previewStubs[1].SetActive(true);
				previewStubs[2].SetActive(true);
				previewStubs[3].SetActive(false);
				previewStubs[4].SetActive(false);
				break;
			case 4:
				previewStubs[0].SetActive(true);
				previewStubs[1].SetActive(true);
				previewStubs[2].SetActive(true);
				previewStubs[3].SetActive(true);
				previewStubs[4].SetActive(false);
				break;
			case 5:
				previewStubs[0].SetActive(true);
				previewStubs[1].SetActive(true);
				previewStubs[2].SetActive(true);
				previewStubs[3].SetActive(true);
				previewStubs[4].SetActive(true);
				break;
		}
	}
}
