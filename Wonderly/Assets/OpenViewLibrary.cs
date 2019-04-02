using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenViewLibrary : MonoBehaviour {

	public Animator panelAnim;


	public void OpenAnimState()
	{
		panelAnim.SetBool("open", true);
		Debug.Log(panelAnim.GetBool("open"));
	}
}
