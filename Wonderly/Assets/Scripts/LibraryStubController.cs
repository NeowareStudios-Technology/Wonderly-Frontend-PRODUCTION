using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibraryStubController : MonoBehaviour {

	public CloudEndpointsApiManager ceam;

	//makes sure that library is clear whenever library exited
	void OnDisable() 
	{
		ceam.deactivateLibraryStubs();
	}

	//makes sure that library is populated when active
	void OnEnable() 
	{
		ceam.getOwnedCodes();
	}

}
