/******************************************************
*Project: Wonderly
*Created by: David Lee Ramirez
*Date: 12/28/18
*Description: Used for selecting and downloading a 
*             Google Poly 3D model into the working
*             scene
*Copyright 2018 LeapWithAlice,LLC. All rights reserved
 ******************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PolyToolkit;

public class ModelRenderer : MonoBehaviour {

	//script references
    public ImageTargetManager itm;
    public targetObjectManager tom;
    public ModelInitializer mi;
    public FilesManager fm;
    //used to delete children of the current target GameObject
    public GameObject currentTarget;
    //transform for the model imported from Poly
    public Transform myModel;
    //search term for 
    public InputField keyword;
    //holds attributes for Google Poly model
    public string attributeString;
    //UI for displaying model attributes (Create flow)
    public Text modelAttrib;
    //UI for displaying model attributes (Edit flow)
    public Text modelAttrib2;
    //for activating/deactivating loading panel
    public GameObject localScriptHolder;

 
    //Destroys children of target passed to it
    private void destroyChildren(GameObject currentTarget) {
        for (int x = 0; x < currentTarget.transform.childCount; x++)
            Destroy(currentTarget.transform.GetChild(x).gameObject);
    }


    // Callback invoked when the featured assets results are returned.
    public void renderModel(GameObject whichModel) {
        //if target not crerated for index yet or no targets exist, do nothing
        if (fm.currentTarget == 0)
            return;
        if (fm.targetStatus[fm.currentTarget-1] == "none")
            return;

        fm.targetStatus[fm.currentTarget-1] = "model";
        List<PolyAsset> renderList = new List<PolyAsset>();
        renderList.Add(whichModel.GetComponent<PolyAssetHolderClass>().heldAsset);
        attributeString = PolyApi.GenerateAttributions(includeStatic: true, runtimeAssets: renderList);

        //get rid of previous import and get asset and save model ID
        switch(fm.currentTarget)
        {
            case 0:
                return;
            case 1:
                modelAttrib.text = attributeString;
                modelAttrib2.text =attributeString;
                if(itm.target1.transform.childCount == 4)
                    Destroy(itm.target1.transform.GetChild(3).gameObject);
                tom.modelIds[0] = ParseForModelId(attributeString);
                if (!tom.attribs.Contains(attributeString))
                    tom.attribs.Add(attributeString);
                PolyApi.GetAsset(tom.modelIds[0], GetAssetCallback);
                break;
            case 2:
                modelAttrib.text = attributeString;
                modelAttrib2.text =attributeString;
                if(itm.target2.transform.childCount == 4)
                    Destroy(itm.target2.transform.GetChild(3).gameObject);
                tom.modelIds[1] = ParseForModelId(attributeString);
                if (!tom.attribs.Contains(attributeString))
                    tom.attribs.Add(attributeString);
                PolyApi.GetAsset(tom.modelIds[1], GetAssetCallback);
                break;
            case 3:
                modelAttrib.text = attributeString;
                modelAttrib2.text =attributeString;
                if(itm.target3.transform.childCount == 4)
                    Destroy(itm.target3.transform.GetChild(3).gameObject);
                tom.modelIds[2] = ParseForModelId(attributeString);
                if (!tom.attribs.Contains(attributeString))
                    tom.attribs.Add(attributeString);
                PolyApi.GetAsset(tom.modelIds[2], GetAssetCallback);
                break;
            case 4:
                modelAttrib.text = attributeString;
                modelAttrib2.text =attributeString;
                if(itm.target4.transform.childCount == 4)
                    Destroy(itm.target4.transform.GetChild(3).gameObject);
                tom.modelIds[3] = ParseForModelId(attributeString);
                if (!tom.attribs.Contains(attributeString))
                    tom.attribs.Add(attributeString);
                PolyApi.GetAsset(tom.modelIds[3], GetAssetCallback);
                break;
            case 5:
                modelAttrib.text = attributeString;
                modelAttrib2.text =attributeString;
                if(itm.target5.transform.childCount == 4)
                    Destroy(itm.target5.transform.GetChild(3).gameObject);
                tom.modelIds[4] = ParseForModelId(attributeString);
                if (!tom.attribs.Contains(attributeString))
                    tom.attribs.Add(attributeString);
                PolyApi.GetAsset(tom.modelIds[4], GetAssetCallback);
                break;
        }

        //delete thumbnails and clear memory
        mi.DeleteThumbnails();
        mi.DeleteThumbnails2();
    }


    //sets import specs for model from Google Poly and calls import function
    void GetAssetCallback(PolyStatusOr<PolyAsset> result) 
    {
        if (!result.Ok) 
            {
                Debug.Log("There was an error importing the loaded asset");
                return;
                //LoadingPanel.SetActive(false);
            }

        // Set the import options.
        PolyImportOptions options = PolyImportOptions.Default();
        // We want to rescale the imported meshes to a specific size.
        options.rescalingMode = PolyImportOptions.RescalingMode.FIT;
        // The specific size we want assets rescaled to (fit in a 1x1x1 box):
        options.desiredSize = 2.0f;
        // We want the imported assets to be recentered such that their centroid coincides with the origin:
        options.recenter = true;

        PolyApi.Import(result.Value, options, ImportAssetCallback);
    }


    // Callback invoked when an asset has just been imported.
    private void ImportAssetCallback(PolyAsset asset, PolyStatusOr<PolyImportResult> result) {
        //only set "model" status on an already created target
        if (fm.targetStatus[fm.currentTarget-1] != "none")
        {
            GameObject myModelObject = result.Value.gameObject;
            myModel = result.Value.gameObject.GetComponent(typeof(Transform)) as Transform;
            myModel.transform.position = new Vector3(0.0f, 0.65f, 0f);

             //to decide which target to render the model to
            switch(fm.currentTarget)
            {
                case 0:
                    return;
                case 1:
                    myModel.tag = "importedModel1";
                    myModel.transform.parent = itm.target1.transform;
                    //model1 needs to get the model ID of the first model from attributesString
                    tom.modelIds[0] = ParseForModelId(attributeString);
                    tom.models[0] = myModelObject;
                    break;
                case 2:
                    myModel.tag = "importedModel2";
                    myModel.transform.parent = itm.target2.transform;
                    tom.modelIds[1] = ParseForModelId(attributeString);
                    tom.models[1] = myModelObject;
                    break;
                case 3:
                    myModel.tag = "importedModel3";
                    myModel.transform.parent = itm.target3.transform;
                    tom.modelIds[2] = ParseForModelId(attributeString);
                    tom.models[2] = myModelObject;
                    break;
                case 4:
                    myModel.tag = "importedModel4";
                    myModel.transform.parent = itm.target4.transform;
                    tom.modelIds[3] = ParseForModelId(attributeString);
                    tom.models[3] = myModelObject;
                    break;
                case 5:
                    myModel.tag = "importedModel5";
                    myModel.transform.parent = itm.target5.transform;
                    tom.modelIds[4] = ParseForModelId(attributeString);
                    tom.models[4] = myModelObject;
                    break;
            }
        }
        localScriptHolder.GetComponent<UiManager>().SetLoadingPanelActive(false);
    }


    //parses the attribute string for model from Google Poly API for model ID
    //-the model ID is needed for the save file, so that when downloading the 
    //experience the model is downloaded from Poly
    private string ParseForModelId(string attribString)
    {
        //get beginning index of model ID
        int position1 = attribString.IndexOf("/view/");
        position1 += 6;

        //get ending index of model ID
        int position2 = attribString.IndexOf("License");
        position2 -= 1;

        string modelID = attribString.Substring(position1, position2-position1);

        return modelID;
    }
}
