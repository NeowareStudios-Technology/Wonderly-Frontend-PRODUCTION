﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PolyToolkit;
using Sample;

public class ModelInitializer : MonoBehaviour {
    public Text keyword;
    //public RawImage thumbPrefab;
    //public GameObject content;
    public Image thumbImage1;
    public Image thumbImage2;
    public Image thumbImage3;
    public Image thumbImage4;
    public Image thumbImage5;
    public Image thumbImage6;
    public Image thumbImage7;
    public Image thumbImage8;
    public Image thumbImage9;
    public Image thumbImage10;
    public Image thumbImage11;
    public Image thumbImage12;
    public Image thumbImage13;
    public Image thumbImage14;
    public Image thumbImage15;
    public Image thumbImage16;
    public Image thumbImage17;
    public Image thumbImage18;

    public PolyAssetHolderClass thumbAsset1;
    public PolyAssetHolderClass thumbAsset2;
    public PolyAssetHolderClass thumbAsset3;
    public PolyAssetHolderClass thumbAsset4;
    public PolyAssetHolderClass thumbAsset5;
    public PolyAssetHolderClass thumbAsset6;
    public PolyAssetHolderClass thumbAsset7;
    public PolyAssetHolderClass thumbAsset8;
    public PolyAssetHolderClass thumbAsset9;
    public PolyAssetHolderClass thumbAsset10;
    public PolyAssetHolderClass thumbAsset11;
    public PolyAssetHolderClass thumbAsset12;
    public PolyAssetHolderClass thumbAsset13;
    public PolyAssetHolderClass thumbAsset14;
    public PolyAssetHolderClass thumbAsset15;
    public PolyAssetHolderClass thumbAsset16;
    public PolyAssetHolderClass thumbAsset17;
    public PolyAssetHolderClass thumbAsset18;



    //need to implement attributes
    //public Text modelAttributes;
    //public int assetIndex = 0;
    //public List<PolyAsset> assetsInUse;
    public ModelRenderer mr;
    public FilesManager fm;
    public int thumbnailCount = 0;
    private RawImage thumb;

    public void GetThumbnails()
    {
        PolyListAssetsRequest req = new PolyListAssetsRequest();
        req.keywords = keyword.text;
        req.curated = true;
        req.orderBy = PolyOrderBy.BEST;
        req.maxComplexity = PolyMaxComplexityFilter.UNSPECIFIED;
        PolyApi.ListAssets(req, ListAssetsCallback);
        thumbnailCount = 0;
    }

    // Callback invoked when the featured assets results are returned.
    private void ListAssetsCallback(PolyStatusOr<PolyListAssetsResult> result) {
        
        if (!result.Ok) {
            Debug.LogError("Failed to get featured assets. :( Reason: " + result.Status);
            return;
        }
 

      

        //string attribs = PolyApi.GenerateAttributions(includeStatic: true, runtimeAssets: assetsInUse);
        //modelAttributes.text = attribs;
        //mr.attributeString = attribs;
            //PolyApi.Import(result.Value.assets[i], options, ImportAssetCallback);
        for (int i = 0; i < Mathf.Min(18, result.Value.assets.Count); i++) { 
            Debug.Log(i+" "+ result.Value.assets[i]);
            PolyApi.FetchThumbnail(result.Value.assets[i], MyThumbnailCallback);
        }
    }

    void MyThumbnailCallback(PolyAsset asset, PolyStatus status)
    {
        Debug.Log("in callback");
        if (!status.ok)
        {
            Debug.Log("Loading thumbnails fail");
            // Handle error;
            return;
        }
        // Display the asset.thumbnailTexture.
        Debug.Log("Loading thumbnails");
        //thumb = Instantiate(thumbPrefab,content.transform);
        Rect rec = new Rect(0, 0, asset.thumbnailTexture.width, asset.thumbnailTexture.height);
        switch(thumbnailCount)
        {
            case 0:
                thumbImage1.sprite = Sprite.Create(asset.thumbnailTexture, rec, new Vector2(0.5f, 0.5f), 100);
                thumbAsset1.heldAsset = asset;
                break;
            case 1:
                thumbImage2.sprite = Sprite.Create(asset.thumbnailTexture, rec, new Vector2(0.5f, 0.5f), 100);
                thumbAsset2.heldAsset = asset;
                break;
            case 2:
                thumbImage3.sprite = Sprite.Create(asset.thumbnailTexture, rec, new Vector2(0.5f, 0.5f), 100);
                thumbAsset3.heldAsset = asset;
                break;
            case 3:
                thumbImage4.sprite = Sprite.Create(asset.thumbnailTexture, rec, new Vector2(0.5f, 0.5f), 100);
                thumbAsset4.heldAsset = asset;
                break;
            case 4:
                thumbImage5.sprite = Sprite.Create(asset.thumbnailTexture, rec, new Vector2(0.5f, 0.5f), 100);
                thumbAsset5.heldAsset = asset;
                break;
            case 5:
                thumbImage6.sprite = Sprite.Create(asset.thumbnailTexture, rec, new Vector2(0.5f, 0.5f), 100);
                thumbAsset6.heldAsset = asset;
                break;
            case 6:
                thumbImage7.sprite = Sprite.Create(asset.thumbnailTexture, rec, new Vector2(0.5f, 0.5f), 100);
                thumbAsset7.heldAsset = asset;
                break;
            case 7:
                thumbImage8.sprite = Sprite.Create(asset.thumbnailTexture, rec, new Vector2(0.5f, 0.5f), 100);
                thumbAsset8.heldAsset = asset;
                break;
            case 8:
                thumbImage9.sprite = Sprite.Create(asset.thumbnailTexture, rec, new Vector2(0.5f, 0.5f), 100);
                thumbAsset9.heldAsset = asset;
                break;
            case 9:
                thumbImage10.sprite = Sprite.Create(asset.thumbnailTexture, rec, new Vector2(0.5f, 0.5f), 100);
                thumbAsset10.heldAsset = asset;
                break;
            case 10:
                thumbImage11.sprite = Sprite.Create(asset.thumbnailTexture, rec, new Vector2(0.5f, 0.5f), 100);
                thumbAsset11.heldAsset = asset;
                break;
            case 11:
                thumbImage12.sprite = Sprite.Create(asset.thumbnailTexture, rec, new Vector2(0.5f, 0.5f), 100);
                thumbAsset12.heldAsset = asset;
                break;
            case 12:
                thumbImage13.sprite = Sprite.Create(asset.thumbnailTexture, rec, new Vector2(0.5f, 0.5f), 100);
                thumbAsset13.heldAsset = asset;
                break;
            case 13:
                thumbImage14.sprite = Sprite.Create(asset.thumbnailTexture, rec, new Vector2(0.5f, 0.5f), 100);
                thumbAsset14.heldAsset = asset;
                break;
            case 14:
                thumbImage15.sprite = Sprite.Create(asset.thumbnailTexture, rec, new Vector2(0.5f, 0.5f), 100);
                thumbAsset15.heldAsset = asset;
                break;
            case 15:
                thumbImage16.sprite = Sprite.Create(asset.thumbnailTexture, rec, new Vector2(0.5f, 0.5f), 100);
                thumbAsset16.heldAsset = asset;
                break;
            case 16:
                thumbImage17.sprite = Sprite.Create(asset.thumbnailTexture, rec, new Vector2(0.5f, 0.5f), 100);
                thumbAsset17.heldAsset = asset;
                break;
            case 17:
                thumbImage18.sprite = Sprite.Create(asset.thumbnailTexture, rec, new Vector2(0.5f, 0.5f), 100);
                thumbAsset18.heldAsset = asset;
                break;
        }

        thumbnailCount++;

    }
/* 
    public void ShowNextThumbnail() {
        if (assetIndex < assetsInUse.Count){
            assetIndex++;
            PolyApi.FetchThumbnail(assetsInUse[assetIndex], MyThumbnailCallback);

            List<PolyAsset> assetAttributes = new List<PolyAsset>();
            assetAttributes.Add(assetsInUse[assetIndex]);
            string attribs = PolyApi.GenerateAttributions(includeStatic: true, runtimeAssets: assetAttributes);
            //modelAttributes.text = attribs;
            mr.attributeString = attribs;
        }
    }

    public void ShowPreviousThumbnail() {
        if (assetIndex > 1){
            assetIndex--;
            PolyApi.FetchThumbnail(assetsInUse[assetIndex], MyThumbnailCallback);

            List<PolyAsset> assetAttributes = new List<PolyAsset>();
            assetAttributes.Add(assetsInUse[assetIndex]);
            string attribs = PolyApi.GenerateAttributions(includeStatic: true, runtimeAssets: assetAttributes);
            //modelAttributes.text = attribs;
            mr.attributeString = attribs;
        }
    }
    */
}