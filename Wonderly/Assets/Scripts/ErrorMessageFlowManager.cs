using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorMessageFlowManager : MonoBehaviour {

	public int signUpIndex;

	//local script holder reference
	public GameObject lsh;

	public GameObject badEmailNotification;
	public GameObject badPasswordNotification;
	public GameObject badPasswordNotification2;
	public GameObject emptyfirstNameNotification;
	public GameObject emptylastNameNotification;
	public GameObject existingEmailNotification;

	public GameObject acceptTermsMenu;


	public InputField email;
	public InputField firstName;
	public InputField lastName;
	public InputField password;
	public InputField password2;

	public CloudEndpointsApiManager ceam;

	public FirebaseManager fbm;

	public PanelController canvasPanelController;

	public Animator[] signUpScreensAnimators = new Animator[6]; 

	// Use this for initialization
	void Start () {
		signUpIndex=0;
	}

	//call this when moving backward through user sign up flow
	public void prevSignUpPanel()
	{
		switch(signUpIndex)
		{
			case 0:
				break;
			case 1:
				canvasPanelController.OpenPanel(signUpScreensAnimators[0]);
				signUpIndex--;
				break;
			case 2:
				canvasPanelController.OpenPanel(signUpScreensAnimators[1]);
				signUpIndex--;
				break;
			case 3:
				canvasPanelController.OpenPanel(signUpScreensAnimators[2]);
				signUpIndex--;
				break;
			case 4:
				canvasPanelController.OpenPanel(signUpScreensAnimators[3]);
				signUpIndex--;
				break;
		}
	}

	//call this when moving forward through user sign up flow
	public void nextSignUpPanel()
	{
		switch(signUpIndex)
		{
			//for email input validation
			case 0:
				//if name is provided
				if (firstName.text != "")
				{
					canvasPanelController.OpenPanel(signUpScreensAnimators[1]);
					signUpIndex++;
				}
				else
				{
					emptyfirstNameNotification.SetActive(true);
					Debug.Log("blank input");
				}
				break;
			//for first name input validation
			case 1:
				//if last name is provided
				if (lastName.text != "")
				{
					canvasPanelController.OpenPanel(signUpScreensAnimators[2]);
					signUpIndex++;
				}
				else
				{
					emptylastNameNotification.SetActive(true);
					Debug.Log("blank input");
				}
				break;
			//for last name input validation
			case 2:
				//if email is properly formatted
				if (email.text.Contains("@") && (email.text.Contains(".com")
				||email.text.Contains(".net")
				||email.text.Contains(".io")
				||email.text.Contains(".org")
				||email.text.Contains(".gov")
				||email.text.Contains(".co")
				||email.text.Contains(".us")
				||email.text.Contains(".de")
				||email.text.Contains(".cn")
				||email.text.Contains(".uk")
				||email.text.Contains(".info")
				||email.text.Contains(".nl")
				||email.text.Contains(".eu")
				||email.text.Contains(".ru")
				))
				{
					//contact backend to make sure email not already in use (signUpIndex increased in ceam)
					lsh.GetComponent<UiManager>().SetLoadingPanelActive(true);
					ceam.startCheckEmail();
				}
				else
				{
						Debug.Log("invalid email");
						badEmailNotification.SetActive(true);
				}
				break;

			//for password input validation
			case 3:
				if (password.text.Length >= 6 && password.text.Length <= 20)
				{
					canvasPanelController.OpenPanel(signUpScreensAnimators[4]);
					signUpIndex++;
				}
				else
				{
					badPasswordNotification.SetActive(true);
				}
				break;
		}
	}	

	//for matching passwords input validation (last part of user signup)
	public void ensureMatchingPasswords()
	{
		if (password.text == password2.text)
		{
			acceptTermsMenu.SetActive(true);
		}
		else
		{
			Debug.Log("password does not match");
			badPasswordNotification2.SetActive(true);
		}
	}

	public void resetIndex()
	{
		signUpIndex =0;
		StartCoroutine("delayedReset");
	}

	//reset of input fields must be delayed so that script can get values from input fields for making account
	private IEnumerator delayedReset()
	{
		yield return new WaitForSeconds(4);
		email.text = "";
		firstName.text = "";
		lastName.text = "";
		password.text = "";
		password2.text = "";
	}

	//needed because logic flow must go to ceam then back to this script
	public void activateSignUpPanel2()
	{
		lsh.GetComponent<UiManager>().SetLoadingPanelActive(false);
		canvasPanelController.OpenPanel(signUpScreensAnimators[3]);
	}

	//needed because logic flow must go to ceam then back to this script
	public void displayEmailError()
	{
		lsh.GetComponent<UiManager>().SetLoadingPanelActive(false);
		existingEmailNotification.SetActive(true);
	}
}


