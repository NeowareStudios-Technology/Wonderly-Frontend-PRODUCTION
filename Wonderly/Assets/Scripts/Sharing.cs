using System.Collections;

using System.Collections.Generic;

using UnityEngine;


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System;
using UnityEngine.UI;

public class Sharing : MonoBehaviour
{    
    public Text createdJourneyTitle;
    private string experienceCode = "Experience code unavailable";
    private string experienceTitle = "A Simple Experience";
    //default case 'Anonymous user' should not appear unless broken
    private string fullName = "Anonymous user";
    public string linkToAndroidStore = "www.linkToAliceDownloadAndroid.com";
    public string linkToAppleStore = "www.linkToAliceDownloadApple.com";
    public string defaultLink = "https://leapwithalice.io/";
    private string linkToStore = "https://leapwithalice.io/";
    private string getProfileUrl = "https://aliceone-221018.appspot.com/_ah/api/aliceOne/v1/profile";
    public FirebaseManager fbm;
    public FirebaseStorageManager fsm;
    public ProfileInfoClass pic;
    public CloudEndpointsApiManager ceam;


    public void SharingCode()
    {
        StartCoroutine(CreateAndSendMessage());
    }

	public IEnumerator CreateAndSendMessage() 
	{
		using (UnityWebRequest newProfileInfoRequest = UnityWebRequest.Get(getProfileUrl))
		{
			//set content type
            
			newProfileInfoRequest.SetRequestHeader("Content-Type", "application/json");
			//set auth header
			newProfileInfoRequest.SetRequestHeader("Authorization", "Bearer " + fbm.token);
			yield return newProfileInfoRequest.SendWebRequest();
			Debug.Log(newProfileInfoRequest.responseCode);
			byte[] results = newProfileInfoRequest.downloadHandler.data;
			string jsonString = Encoding.UTF8.GetString(results);
			Debug.Log(jsonString);
			pic = JsonUtility.FromJson<ProfileInfoClass>(jsonString);

            fullName = pic.firstName + " " + pic.lastName;
            //experienceCode = experienceCodeGO.GetComponent<Text>().text;
            //experienceTitle = experienceTitleGO.GetComponent<Text>().text;
            linkToStore = DetermineLinkToStore();
            print("Congratulations! <b>" + fullName + "</b> has just sent you the code '<b>" + experienceCode + "</b>' for '<b>" + experienceTitle + "</b>', an experience within the Wonderly application."+ "\n" +"If you do not have the Wonderly application, download it here. Apple download " + linkToAppleStore + ". Android download " + linkToAndroidStore);
            //new NativeShare().SetText("Congratulations! " + fullName + " has just sent you the code '" + experienceCode + "' for '" + experienceTitle + "' , an experience within the Wonderly application."+ "\n" +"If you do not have the Wonderly application, download it here " + linkToStore).Share();
    	}

	}

    public void SharingCodeJourneyContextMenu(int index){
        Debug.Log("CloudEndpointsMAanage success call to Sharing. The sharing index is " + index);
        StartCoroutine(ShareFromJourneyContextMenu(index));
    }
    public IEnumerator ShareFromJourneyContextMenu(int index){
        
        using (UnityWebRequest newProfileInfoRequest = UnityWebRequest.Get(getProfileUrl))
		{
			//set content type
			newProfileInfoRequest.SetRequestHeader("Content-Type", "application/json");
			//set auth header
			newProfileInfoRequest.SetRequestHeader("Authorization", "Bearer " + fbm.token);
			yield return newProfileInfoRequest.SendWebRequest();
			Debug.Log(newProfileInfoRequest.responseCode);
			byte[] results = newProfileInfoRequest.downloadHandler.data;
			string jsonString = Encoding.UTF8.GetString(results);
			Debug.Log(jsonString);

			pic = ceam.pic;
            experienceCode = ceam.libraryCodes[index];
            fullName = pic.firstName + " " + pic.lastName;
            experienceTitle = ceam.libraryStubs[index].transform.GetChild(1).GetComponent<Text>().text;
            if (experienceTitle == ""){
                experienceTitle = "A Simple Experience";
            }
            linkToStore = DetermineLinkToStore();
            print("Congratulations! " + fullName + " has just sent you the code '" + experienceCode + "' for '" + experienceTitle + "', an experience within the Wonderly application."+ "\n" +"If you do not have the Wonderly application, download it here." + "\n"  + "Apple download " + linkToAppleStore + "\n" + "Android download " + linkToAndroidStore);
            new NativeShare().SetText("Congratulations! " + fullName + " has just sent you the code '" + experienceCode + "' for '" + experienceTitle + "', an experience within the Wonderly application."+ "\n" +"If you do not have the Wonderly application, download it here." + "\n"  + "Apple download " + linkToAppleStore + "\n" + "Android download " + linkToAndroidStore).Share();
    	}
    }

    public void SharingCodeRecentlyCreated()
    {
        StartCoroutine(ShareMessage());
    }

	public IEnumerator ShareMessage() 
	{
		using (UnityWebRequest newProfileInfoRequest = UnityWebRequest.Get(getProfileUrl))
		{
			//set content type
			newProfileInfoRequest.SetRequestHeader("Content-Type", "application/json");
			//set auth header
			newProfileInfoRequest.SetRequestHeader("Authorization", "Bearer " + fbm.token);
			yield return newProfileInfoRequest.SendWebRequest();
			Debug.Log(newProfileInfoRequest.responseCode);
			byte[] results = newProfileInfoRequest.downloadHandler.data;
			string jsonString = Encoding.UTF8.GetString(results);
			Debug.Log(jsonString);
			pic = ceam.pic;

            fullName = pic.firstName + " " + pic.lastName;
            experienceCode = fsm.ecc.code;
            experienceTitle = createdJourneyTitle.text;
            linkToStore = DetermineLinkToStore();
            print("Congratulations! " + fullName + " has just sent you the code '" + experienceCode + "' for '" + experienceTitle + "', an experience within the Wonderly application."+ "\n" +"If you do not have the Wonderly application, download it here." + "\n"  + "Apple download " + linkToAppleStore + "\n" + "Android download " + linkToAndroidStore);
            new NativeShare().SetText("Congratulations! " + fullName + " has just sent you the code '" + experienceCode + "' for '" + experienceTitle + "', an experience within the Wonderly application."+ "\n" +"If you do not have the Wonderly application, download it here." + "\n"  + "Apple download " + linkToAppleStore + "\n" + "Android download " + linkToAndroidStore).Share();
    	}

	}
    
    private string DetermineLinkToStore(){
        if (Application.platform == RuntimePlatform.Android){
            return linkToAndroidStore;
        }
        else if(Application.platform == RuntimePlatform.IPhonePlayer){
            return linkToAppleStore;
        }
        //if non mobile - return default site
        else {
            return defaultLink;
        }
    }
}