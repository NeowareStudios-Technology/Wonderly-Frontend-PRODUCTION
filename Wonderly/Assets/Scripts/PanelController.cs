/******************************************************
*Project: Wonderly
*Created by: 
*Date: 12/28/18
*Description: 
*Copyright 2018 LeapWithAlice,LLC. All rights reserved
 ******************************************************/
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
public class PanelController : MonoBehaviour
{
    #region Variables
    //Screen to open automatically at the start of the Scene
    public GameObject backgroundImg;
    public Animator initiallyOpen;
    public GameObject bottomPanel;
    public GameObject lsh;
    //Currently Open Screen
    private Animator m_Open;
    //Hash of the parameter we use to control the transitions.
    private int m_OpenParameterId;
    //The GameObject Selected before we opened the current Screen.
    //Used when closing a Screen, so we can go back to the button that opened it.
    private GameObject m_PreviouslySelected;
    //Animator State and Transition names we need to check against.
    const string k_OpenTransitionName = "Open";
    const string k_ClosedStateName = "Closed";
    //used to make sure you cant leave a process while its loading
    public GameObject frontCanvas;
    public GameObject loadingPanel;
    //storing panels in this list to call using the android back button
    public List<GameObject> panels = new List<GameObject>();
    private bool isAndroidPlatform = false;

    public GameObject profileImagePanel;

    #endregion
    public void OnEnable()
    {
        //We cache the Hash to the "Open" Parameter, so we can feed to Animator.SetBool.
        m_OpenParameterId = Animator.StringToHash(k_OpenTransitionName);
        //If set, open the initial Screen now.
        if (initiallyOpen == null)
            return;
        
        //check for platform android
        if (Application.platform == RuntimePlatform.Android)
        {
            isAndroidPlatform = true;
        }
        OpenPanel(initiallyOpen);
    }
    //Android Back button is set as the "Escape" key
    //if Escape key press, go back one panel
    void Update()
    {
        if (isAndroidPlatform)
        {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            for (int i = 0; i < panels.Count; i++)
            {
                if (panels[i].activeInHierarchy)
                {
                    GoBackOnePanel(panels[i]);
                }
            }
            return;
        }
        }
    }
    //goes back 1 panel if loading screen is not active
    private void GoBackOnePanel(GameObject activePanel)
    {
        if (loadingPanel.activeInHierarchy || frontCanvas.activeInHierarchy)
        {
            return;
        }
        switch (activePanel.name)
        {
            case ("Welcome-panel"):
                //Debug.Log("yoyoyo");
                Application.Quit();
                break;
            case ("Audience-panel"):
                //open Welcome
                OpenPanel(panels[0].GetComponent<Animator>());
                break;
            case ("SignIn-panel"):
                //open welcome
                OpenPanel(panels[0].GetComponent<Animator>());
                break;
            case ("ResetPassword-panel"):
                //open sign in
                OpenPanel(panels[2].GetComponent<Animator>());
                break;
            case ("SignUp-panel"):
                //open welcome
                OpenPanel(panels[0].GetComponent<Animator>());
                break;
            case ("SignUp2-panel"):
                //open sign up
                lsh.GetComponent<ErrorMessageFlowManager>().signUpIndex--;
                OpenPanel(panels[4].GetComponent<Animator>());
                break;
            case ("SignUp3-panel"):
                //open sign up2
                lsh.GetComponent<ErrorMessageFlowManager>().signUpIndex--;
                OpenPanel(panels[5].GetComponent<Animator>());
                break;
            case ("SignUp4-panel"):
                //open sign 
                lsh.GetComponent<ErrorMessageFlowManager>().signUpIndex--;
                OpenPanel(panels[6].GetComponent<Animator>());
                break;
            case ("SignUp5-panel"):
                //open sign up4
                lsh.GetComponent<ErrorMessageFlowManager>().signUpIndex--;
                OpenPanel(panels[7].GetComponent<Animator>());
                break;
            case ("MyJourneys-panel"):
                Application.Quit();
                break;
            case ("CreateJourney-panel"):
                //MyJourneys
                //cover image search is something to consider here
                OpenPanel(panels[18].GetComponent<Animator>());
                break;
            case ("CreateWoderScan-panel"):
                //going to have to check if a journey build is in progress.
                //if it is, go back to CompleteJourneyCopy
                //if not, go back to CreateJourney
                OpenPanel(panels[10].GetComponent<Animator>());
                break;
            case ("CreateWonderLink-panel"):
                //CreateWoderScan
                OpenPanel(panels[11].GetComponent<Animator>());
                break;
            case ("CwViewLibraries-panel"):
                //open createwonderLink
                OpenPanel(panels[12].GetComponent<Animator>());
                break;
            case ("CwViewLibraryContent-panel"):
                OpenPanel(panels[13].GetComponent<Animator>());
                //open CwViewLibraries
                break;
            case ("CwPlaceLibraryContent-panel"):
                //open CwViewLibraryContent-panel
                OpenPanel(panels[14].GetComponent<Animator>());
                break;
            case ("CwSetupWonder-panel"):
                //open CwPlaceLibraryContent-panel
                OpenPanel(panels[15].GetComponent<Animator>());
                break;
            case ("CompleteJourneyCopy-panel"):
                //going to have to check if a journey build is in progress.
                //if it is, go back to CwSetupWonder
                //if not, go back to MyJourney
                break;
            case ("UserSettings-panel"):
                //open MyJourneys
                OpenPanel(panels[18].GetComponent<Animator>());
                break;
            case ("OverviewVideo-panel"):
                //open Welcome
                OpenPanel(panels[0].GetComponent<Animator>());
                break;
            case ("Search-panel"):
                //open MyJourney panel
                OpenPanel(panels[18].GetComponent<Animator>());
                break;
            case ("NewAddViewContent-panel"):
                //open MyJourney panel
                OpenPanel(panels[18].GetComponent<Animator>());
                break;
            case ("ViewScreen-panel"):
                //open NewAddViewContent-panel
                OpenPanel(panels[21].GetComponent<Animator>());
                break;
        }
        //this prevents the loading screen from staying open waiting for an action to complete

    }
    public void SetBottomPanelActive(bool setPanelActive)
    {
        if (setPanelActive)
        {
            bottomPanel.SetActive(true);
        }
        else
        {
            bottomPanel.SetActive(false);
        }
    }
    //Closes the currently open panel and opens the provided one.
    //It also takes care of handling the navigation, setting the new Selected element.
    public void OpenPanel(Animator anim)
    {
        if (m_Open == anim)
            return;
        //Activate the new Screen hierarchy so we can animate it.
        anim.gameObject.SetActive(true);
        //Save the currently selected button that was used to open this Screen. (CloseCurrent will modify it)
        var newPreviouslySelected = EventSystem.current.currentSelectedGameObject;
        //Move the Screen to front.

        if (anim.gameObject.name == "MyJourneys-panel")
        {
            //bottomPanel.SetActive(true);
        }
        else
        {
            bottomPanel.SetActive(false);
        }
        if (anim.gameObject.name == "CreateWonderScan-panel"
            || anim.gameObject.name == "CreateWonderLink-panel"
            || anim.gameObject.name == "CwPlaceLibraryContent-panel"
            || anim.gameObject.name == "OverviewVideo-panel"
            || anim.gameObject.name == "ViewScreen-panel")
        {
            backgroundImg.SetActive(false);
        }
        else
        {
            backgroundImg.SetActive(true);
        }

        //set the profile image active or inactive depending on the panel to be opened
        if (anim.gameObject.name == "UserSettings-panel"
            || anim.gameObject.name == "CreateWonderLink-panel"
            || anim.gameObject.name == "CreateWonderScan-panel"
            || anim.gameObject.name == "CwViewLibraries-panel"
            || anim.gameObject.name == "ViewScreen-panel"
            || anim.gameObject.name == "CwPlaceLibraryContent-panel"
            || anim.gameObject.name == "ShareJourney-panel")
        {
            profileImagePanel.SetActive(false);
        }
        else
        {
            profileImagePanel.SetActive(true);
        }
        anim.transform.SetAsLastSibling();
        bottomPanel.transform.SetAsLastSibling();
        CloseCurrent();
        m_PreviouslySelected = newPreviouslySelected;
        //Set the new Screen as then open one.
        m_Open = anim;
        //Start the open animation
        m_Open.SetBool(m_OpenParameterId, true);
        //Set an element in the new screen as the new Selected one.
        GameObject go = FindFirstEnabledSelectable(anim.gameObject);
        SetSelected(go);
    }
    //Finds the first Selectable element in the providade hierarchy.
    static GameObject FindFirstEnabledSelectable(GameObject gameObject)
    {
        GameObject go = null;
        var selectables = gameObject.GetComponentsInChildren<Selectable>(true);
        foreach (var selectable in selectables)
        {
            if (selectable.IsActive() && selectable.IsInteractable())
            {
                go = selectable.gameObject;
                break;
            }
        }
        return go;
    }
    //Closes the currently open Screen
    //It also takes care of navigation.
    //Reverting selection to the Selectable used before opening the current screen.
    public void CloseCurrent()
    {
        if (m_Open == null)
            return;
        //Start the close animation.
        m_Open.SetBool(m_OpenParameterId, false);
        //Reverting selection to the Selectable used before opening the current screen.
        SetSelected(m_PreviouslySelected);
        //Start Coroutine to disable the hierarchy when closing animation finishes.
        StartCoroutine(DisablePanelDeleyed(m_Open));
        //No screen open.
        m_Open = null;
    }
    //Coroutine that will detect when the Closing animation is finished and it will deactivate the
    //hierarchy.
    IEnumerator DisablePanelDeleyed(Animator anim)
    {
        bool closedStateReached = false;
        bool wantToClose = true;
        while (!closedStateReached && wantToClose)
        {
            if (!anim.IsInTransition(0))
                closedStateReached = anim.GetCurrentAnimatorStateInfo(0).IsName(k_ClosedStateName);
            wantToClose = !anim.GetBool(m_OpenParameterId);
            yield return new WaitForEndOfFrame();
        }
        if (wantToClose)
            anim.gameObject.SetActive(false);
    }
    //Make the provided GameObject selected
    //When using the mouse/touch we actually want to set it as the previously selected and 
    //set nothing as selected for now.
    private void SetSelected(GameObject go)
    {
        //Select the GameObject.
        EventSystem.current.SetSelectedGameObject(go);
        //If we are using the keyboard right now, that's all we need to do.
        var standaloneInputModule = EventSystem.current.currentInputModule as StandaloneInputModule;
        if (standaloneInputModule != null)
            return;
        //Since we are using a pointer device, we don't want anything selected. 
        //But if the user switches to the keyboard, we want to start the navigation from the provided game object.
        //So here we set the current Selected to null, so the provided gameObject becomes the Last Selected in the EventSystem.
        EventSystem.current.SetSelectedGameObject(null);
    }
}