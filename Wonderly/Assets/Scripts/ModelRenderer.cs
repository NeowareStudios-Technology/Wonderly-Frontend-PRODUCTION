using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using Vuforia;
using PolyToolkit;
using Sample;

public class ModelRenderer : MonoBehaviour {

	//private TrackableBehaviour mTrackableBehaviour;
    public ImageTargetManager itm;
    public targetObjectManager tom;
    public ModelInitializer mi;
    public FilesManager fm;

    public GameObject currentTarget;
 
    public Transform myModelPrefab;
		
    public InputField keyword;

    public int modelFlag = 0;

    public string attributeString;

    public Text modelAttrib;

    public GameObject LoadingPanel;
 
    //Destroys children of target passed to it
    private void destroyChildren(GameObject currentTarget) {
        for (int x = 0; x < currentTarget.transform.childCount; x++)
            Destroy(currentTarget.transform.GetChild(x).gameObject);
    }

    // Callback invoked when the featured assets results are returned.
    public void renderModel(int whichModel) {
        //if target not crerated for index yet or no targets exist, do nothing
        if (fm.currentTarget == 0)
            return;
        if (fm.targetStatus[fm.currentTarget-1] == "none")
            return;


        List<PolyAsset> renderList = new List<PolyAsset>();
        switch(whichModel)
        {
            case 0:
                renderList.Add(mi.thumbAsset1.heldAsset);
                break;
            case 1:
                renderList.Add(mi.thumbAsset2.heldAsset);
                break;
            case 2:
                renderList.Add(mi.thumbAsset3.heldAsset);
                break;
            case 3:
                renderList.Add(mi.thumbAsset4.heldAsset);
                break;
            case 4:
                renderList.Add(mi.thumbAsset5.heldAsset);
                break;
            case 5:
                renderList.Add(mi.thumbAsset6.heldAsset);
                break;
            case 6:
                renderList.Add(mi.thumbAsset7.heldAsset);
                break;
            case 7:
                renderList.Add(mi.thumbAsset8.heldAsset);
                break;
            case 8:
                renderList.Add(mi.thumbAsset9.heldAsset);
                break;
            case 9:
                renderList.Add(mi.thumbAsset10.heldAsset);
                break;
            case 10:
                renderList.Add(mi.thumbAsset11.heldAsset);
                break;
            case 11:
                renderList.Add(mi.thumbAsset12.heldAsset);
                break;
            case 12:
                renderList.Add(mi.thumbAsset13.heldAsset);
                break;
            case 13:
                renderList.Add(mi.thumbAsset14.heldAsset);
                break;
            case 14:
                renderList.Add(mi.thumbAsset15.heldAsset);
                break;
            case 15:
                renderList.Add(mi.thumbAsset16.heldAsset);
                break;
            case 16:
                renderList.Add(mi.thumbAsset17.heldAsset);
                break;
            case 17:
                renderList.Add(mi.thumbAsset18.heldAsset);
                break;
        }
        
        attributeString = PolyApi.GenerateAttributions(includeStatic: true, runtimeAssets: renderList);


        //get rid of previous import and get asset and save model ID
        switch(fm.currentTarget)
        {
            case 0:
                return;
            case 1:
                modelAttrib.text = attributeString;
                if(itm.target1.transform.childCount == 4)
                    Destroy(itm.target1.transform.GetChild(3).gameObject);
                tom.modelIds[0] = ParseForModelId(attributeString);
                if (!tom.attribs.Contains(attributeString))
                    tom.attribs.Add(attributeString);
                PolyApi.GetAsset(tom.modelIds[0], GetAssetCallback);
                break;
            case 2:
                modelAttrib.text = attributeString;
                if(itm.target2.transform.childCount == 4)
                    Destroy(itm.target2.transform.GetChild(3).gameObject);
                tom.modelIds[1] = ParseForModelId(attributeString);
                if (!tom.attribs.Contains(attributeString))
                    tom.attribs.Add(attributeString);
                PolyApi.GetAsset(tom.modelIds[1], GetAssetCallback);
                break;
            case 3:
                modelAttrib.text = attributeString;
                if(itm.target3.transform.childCount == 4)
                    Destroy(itm.target3.transform.GetChild(3).gameObject);
                tom.modelIds[2] = ParseForModelId(attributeString);
                if (!tom.attribs.Contains(attributeString))
                    tom.attribs.Add(attributeString);
                PolyApi.GetAsset(tom.modelIds[2], GetAssetCallback);
                break;
            case 4:
                modelAttrib.text = attributeString;
                if(itm.target4.transform.childCount == 4)
                    Destroy(itm.target4.transform.GetChild(3).gameObject);
                tom.modelIds[3] = ParseForModelId(attributeString);
                if (!tom.attribs.Contains(attributeString))
                    tom.attribs.Add(attributeString);
                PolyApi.GetAsset(tom.modelIds[3], GetAssetCallback);
                break;
            case 5:
                modelAttrib.text = attributeString;
                if(itm.target5.transform.childCount == 4)
                    Destroy(itm.target5.transform.GetChild(3).gameObject);
                tom.modelIds[4] = ParseForModelId(attributeString);
                if (!tom.attribs.Contains(attributeString))
                    tom.attribs.Add(attributeString);
                PolyApi.GetAsset(tom.modelIds[4], GetAssetCallback);
                break;
        }

        //ClearPolyAssetHolders();
    }

    void GetAssetCallback(PolyStatusOr<PolyAsset> result) 
    {
        if (!result.Ok) 
            {
                Debug.Log("There was an error importing the loaded asset");
                return;
                LoadingPanel.SetActive(false);
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
            myModelPrefab = result.Value.gameObject.GetComponent(typeof(Transform)) as Transform;
            myModelPrefab.transform.position = new Vector3(0.0f, 0.65f, 0f);
            fm.targetStatus[fm.currentTarget-1] = "model";

             //to decide which target to render the model to
            switch(fm.currentTarget)
            {
                case 0:
                    return;
                case 1:
                    myModelPrefab.tag = "importedModel1";
                    myModelPrefab.transform.parent = itm.target1.transform;
                    //model1 needs to get the model ID of the first model from attributesString
                    tom.modelIds[0] = ParseForModelId(attributeString);
                    tom.models[0] = myModelObject;
                    break;
                case 2:
                    myModelPrefab.tag = "importedModel2";
                    myModelPrefab.transform.parent = itm.target2.transform;
                    tom.modelIds[1] = ParseForModelId(attributeString);
                    tom.models[1] = myModelObject;
                    break;
                case 3:
                    myModelPrefab.tag = "importedModel3";
                    myModelPrefab.transform.parent = itm.target3.transform;
                    tom.modelIds[2] = ParseForModelId(attributeString);
                    tom.models[2] = myModelObject;
                    break;
                case 4:
                    myModelPrefab.tag = "importedModel4";
                    myModelPrefab.transform.parent = itm.target4.transform;
                    tom.modelIds[3] = ParseForModelId(attributeString);
                    tom.models[3] = myModelObject;
                    break;
                case 5:
                    myModelPrefab.tag = "importedModel5";
                    myModelPrefab.transform.parent = itm.target5.transform;
                    tom.modelIds[4] = ParseForModelId(attributeString);
                    tom.models[4] = myModelObject;
                    break;
            }
        }
        LoadingPanel.SetActive(false);
    }

    public void changeModelFlag() {
        if (modelFlag == 1)
        {
            modelFlag = 0;
        }
        else if (modelFlag == 0)
        {
            modelFlag = 1;
            
        }
    }

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

    private void ClearPolyAssetHolders()
    {
        mi.thumbAsset1.heldAsset= null;
        mi.thumbAsset2.heldAsset= null;
        mi.thumbAsset3.heldAsset= null;
        mi.thumbAsset4.heldAsset= null;
        mi.thumbAsset5.heldAsset= null;
        mi.thumbAsset6.heldAsset= null;
        mi.thumbAsset7.heldAsset= null;
        mi.thumbAsset8.heldAsset= null;
        mi.thumbAsset9.heldAsset= null;
        mi.thumbAsset10.heldAsset= null;
        mi.thumbAsset11.heldAsset= null;
        mi.thumbAsset12.heldAsset= null;
        mi.thumbAsset13.heldAsset= null;
        mi.thumbAsset14.heldAsset= null;
        mi.thumbAsset15.heldAsset= null;
        mi.thumbAsset16.heldAsset= null;
        mi.thumbAsset17.heldAsset= null;
        mi.thumbAsset18.heldAsset= null;
    }
}
