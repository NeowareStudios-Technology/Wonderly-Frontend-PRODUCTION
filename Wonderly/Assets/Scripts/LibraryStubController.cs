/******************************************************
*Project: Wonderly
*Created by: David Lee Ramirez
*Date: 12/28/18
*Description: Is attached to MyJourneys panel to activate 
*							and deactivate library stubs
*Copyright 2018 LeapWithAlice,LLC. All rights reserved
 ******************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibraryStubController : MonoBehaviour {

	public CloudEndpointsApiManager ceam;
	public FirebaseManager fm;

	public bool getNewToken = false;

	void Awake()
	{
		//StartCoroutine("delayedFillLibrary");
	}

	private IEnumerator delayedFillLibrary()
	{
		yield return new WaitForSeconds(2);
		if (fm.token != null)
			ceam.deactivateLibraryStubs();
			ceam.startGetOwnedCodes();
	}

	//makes sure that library is clear whenever library exited
	void OnDisable() 
	{
		ceam.deactivateLibraryStubs();
	}

	//makes sure that library is populated when active
  void OnEnable() 
	{
		if (getNewToken){
			getNewToken = false;
		}
		else{
			ceam.startGetOwnedCodes();
		}
			
	}
}
