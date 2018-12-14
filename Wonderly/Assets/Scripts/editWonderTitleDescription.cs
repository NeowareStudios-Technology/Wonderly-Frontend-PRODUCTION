using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class editWonderTitleDescription : MonoBehaviour {

	public GameObject editWonderInfoPanel;
	public InputField wonderTitle;
	public InputField wonderDescription;
	public SaveManager sm;

	public PanelController pc;
	public Animator editWonderInfoAnimator;

	public void openEditWonderInfoScreen(int whichTarget)
	{
	  pc.OpenPanel(editWonderInfoAnimator);
		editWonderInfoPanel.GetComponent<whichTargetHolder>().whichTarget = whichTarget;
	}

	public void SaveTitleDesc()
	{
		sm.LocalSaveWonderTitleDescManualIndex(editWonderInfoPanel.GetComponent<whichTargetHolder>().whichTarget);
	}

	public void startDelayedClearInputFields()
	{
		StartCoroutine("delayedClearInputFields");
	}
	public IEnumerator delayedClearInputFields()
	{
		yield return new WaitForSeconds(2);
		wonderTitle.text = "";
		wonderDescription.text = "";
	}
}
