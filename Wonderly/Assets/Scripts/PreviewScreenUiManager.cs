using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sample;

public class PreviewScreenUiManager : MonoBehaviour {
	public GameObject[] previewStubs = new GameObject[5];
	public FilesManager fm;
	
	// Update is called once per frame
	void Update () {
		int i;
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
